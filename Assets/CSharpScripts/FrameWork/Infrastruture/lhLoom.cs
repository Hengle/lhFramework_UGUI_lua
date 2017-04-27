using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;

namespace LaoHan.Infrastruture
{
    public class lhLoom
    {
        public static int maxThreads = 8;
        private static int numThreads;
        private struct DelayedQueueItem
        {
            public float time;
            public Action action;
        }
        private int m_count;
        private List<Action> m_currentActions = new List<Action>();
        private List<Action> m_actions = new List<Action>();
        private List<DelayedQueueItem> m_delayed = new List<DelayedQueueItem>();
        private List<DelayedQueueItem> m_currentDelayed = new List<DelayedQueueItem>();

        private static lhLoom m_instance;
        public static lhLoom GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhLoom();
        }
        lhLoom()
        {

        }
        public void Update()
        {
            lock (m_actions)
            {
                m_currentActions.Clear();
                m_currentActions.AddRange(m_actions);
                m_actions.Clear();
            }
            foreach (var a in m_currentActions)
            {
                a();
            }
            lock (m_delayed)
            {
                m_currentDelayed.Clear();
                var range = new List<DelayedQueueItem>();
                foreach (var item in m_delayed)
                {
                    if (item.time <= Time.time)
                        range.Add(item);
                }
                m_currentDelayed.AddRange(range);
                foreach (var item in m_currentDelayed)
                    m_delayed.Remove(item);
            }
            foreach (var delayed in m_currentDelayed)
            {
                delayed.action();
            }

        }
        public void Dispose()
        {
            m_instance = null;
        }
        public static void RunMain(Action action)
        {
            RunMain(action, 0f);
        }
        public static void RunMain(Action action, float time)
        {
            if (time != 0)
            {
                lock (m_instance.m_delayed)
                {
                    m_instance.m_delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                }
            }
            else
            {
                lock (m_instance.m_actions)
                {
                    m_instance.m_actions.Add(action);
                }
            }
        }
        public static void RunAsync(Action a)
        {
            while (numThreads >= maxThreads)
            {
                Thread.Sleep(1);
            }
            Interlocked.Increment(ref numThreads);
            ThreadPool.QueueUserWorkItem(m_instance.RunAction, a);
        }
        private void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch
            {
            }
            finally
            {
                Interlocked.Decrement(ref numThreads);
            }
        }
    }
}
//void ScaleMesh(Mesh mesh, float scale)
//{
//    //Get the vertices of a mesh
//    var vertices = mesh.vertices;
//    //Run the action on a new thread
//    Loom.RunAsync(() => {
//        //Loop through the vertices
//        for (var i = 0; i < vertices.Length; i++)
//        {
//            //Scale the vertex
//            vertices[i] = vertices[i] * scale;
//        }
//        //Run some code on the main thread
//        //to update the mesh
//        Loom.QueueOnMainThread(() => {
//            //Set the vertices
//            mesh.vertices = vertices;
//            //Recalculate the bounds
//            mesh.RecalculateBounds();
//        });

//    });
//}