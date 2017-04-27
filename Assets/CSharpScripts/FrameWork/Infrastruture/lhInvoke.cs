using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace LaoHan.Infrastruture
{
    public class lhInvoke
    {
        private static lhInvoke m_instance;
        private Action OnInvokeDelegate;
        private class InvokePool
        {

            public enum Status
            {
                delay = 0,
                interval = 1,
                complete = 2
            }
            public Status status
            {
                get
                {
                    return m_status;
                }
            }
            public Action invoke
            {
                get
                {
                    return m_invoke;
                }
            }
            private Action m_invoke;
            private int m_count;
            private float m_delay;
            private float m_interval;
            private float m_delayTime;
            private float m_intervalTime;
            private Status m_status;
            public void Create(Action callback, float delay, float interval=0, int count=0)
            {
                this.m_invoke = callback;
                this.m_delay = delay;
                this.m_interval = interval;
                this.m_count = count;
                this.m_delayTime = Time.time;
                this.m_status = 0;
            }
            public void Update()
            {
                if (this.m_status == Status.complete) return;
                if (Time.time - m_delayTime <= m_delay)
                {
                    this.m_status = Status.delay;
                    return;
                }
                if (this.m_status == Status.delay)
                {
                    m_invoke();
                    m_intervalTime = Time.time;
                    this.m_status = Status.interval;
                }
                if (m_interval == 0)
                {
                    this.m_status = Status.complete;
                    return;
                }
                if (Time.time - m_intervalTime >= m_interval)
                {
                    m_invoke();
                    m_intervalTime = Time.time;
                    if (m_count != 0)
                    {
                        m_count--;
                        if (m_count == 0)
                            this.m_status = Status.complete;
                    }
                }
            }
            public void Release()
            {
                this.m_invoke = null;
                this.m_delay = 0;
                this.m_interval = 0;
                this.m_delayTime = 0;
                this.m_intervalTime = 0;
                this.m_status = Status.complete;
            }
        }
        private List<InvokePool> m_usingList = new List<InvokePool>();
        private List<InvokePool> m_freeList = new List<InvokePool>();
        public static lhInvoke GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhInvoke();
        }
        lhInvoke() { }
        public void Dispose()
        {
            m_instance = null;
        }
        public void Update()
        {
            if (m_usingList.Count == 0) return;
            for (int i = 0; i < m_usingList.Count; i++)
            {
                if (m_usingList[i].status == InvokePool.Status.complete)
                {
                    m_usingList[i].Release();
                    m_freeList.Add(m_usingList[i]);
                    m_usingList.RemoveAt(i);
                    i--;
                    continue;
                }
                m_usingList[i].Update();
            }
        }
        public static void Invoke(Action callback, float delay, float interval=0, int count=0)
        {
            if (m_instance.m_freeList.Count == 0)
            {
                InvokePool invokePool = new InvokePool();
                m_instance.m_freeList.Add(invokePool);
            }
            m_instance.m_freeList[0].Create(callback, delay, interval, count);
            m_instance.m_usingList.Add(m_instance.m_freeList[0]);
            m_instance.m_freeList.RemoveAt(0);
        }
        public static void CancelInvoke(Action callback)
        {
            for (int i = 0; i < m_instance.m_usingList.Count; i++)
                if (m_instance.m_usingList[i].invoke == callback)
                {
                    m_instance.m_usingList[i].Release();
                    m_instance.m_freeList.Add(m_instance.m_usingList[i]);
                    m_instance.m_usingList.RemoveAt(i);
                    break;
                }
        }
        public static void CancelAllInvoke()
        {
            for (int i = 0; i < m_instance.m_usingList.Count; i++)
            {
                m_instance.m_usingList[i].Release();
                m_instance.m_freeList.Add(m_instance.m_usingList[i]);
                m_instance.m_usingList.RemoveAt(i);
                i--;
            }
        }
    }
}