//#define POOLPRINT
using System;
using System.Collections;
using System.Collections.Generic;
#if POOLPRINT
using System.IO;
using System.Text;
#endif
using UnityEngine;

namespace LaoHan.Infrastruture
{
    public class ClassStoreData
    {
        public Type type;
        public int count;
        public Action<object> onCreateHandler;
        public Action<object> onFreeHandler;
    }
    public class lhClassManager
    {
        private class ClassPool
        {
            private Queue<object> m_queue;
            private Type m_type;
            public Action<object> createHandler;
            public Action<object> freeHandler;
            public ClassPool(Type type, int count, Action<object> onCreateHandler, Action<object> onFreeHandler, Action onStoreOver)
            {
                m_queue = new Queue<object>();
                createHandler = onCreateHandler;
                freeHandler = onFreeHandler;
                this.m_type = type;
                for (int i = 0; i < count; i++)
                {
                    var cla = Activator.CreateInstance(type);
                    if (createHandler != null)
                        createHandler(cla);
                    m_queue.Enqueue(cla);
                }
                if (onStoreOver != null)
                {
                    onStoreOver();
                }
            }
            public void Store(int count, Action onStoreOver)
            {
                for (int i = 0; i < count; i++)
                {
                    var cla = Activator.CreateInstance(m_type);
                    if (createHandler != null)
                        createHandler(cla);
                    m_queue.Enqueue(cla);
                    WriteToLocal(System.DateTime.Now.ToString("mm月dd日hh时mm分ss秒  ") + m_type + " =>Store    m_queue count:" + m_queue.Count);
                }
                if (onStoreOver != null)
                {
                    onStoreOver();
                }
            }
            public object GetObject()
            {
                WriteToLocal(System.DateTime.Now.ToString("mm月dd日hh时mm分ss秒  ") + m_type + " =>GetObject    m_queue count:" + m_queue.Count);
                if (m_queue.Count <= 0)
                {
                    var cla = Activator.CreateInstance(m_type);
                    if (createHandler != null)
                        createHandler(cla);
                    return cla;
                }
                else
                    return m_queue.Dequeue();
            }
            public void FreeObject(object obj)
            {
                WriteToLocal(System.DateTime.Now.ToString("mm月dd日hh时mm分ss秒  ") + m_type + " =>FreeObject    m_queue count:" + m_queue.Count);
                if (m_queue.Contains(obj)) return;
                if (freeHandler != null)
                    freeHandler(obj);
                m_queue.Enqueue(obj);
            }
            public void Clear()
            {
                var arr = m_queue.ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = null;
                }
                m_queue.Clear();
            }
        }
        private Dictionary<Type, ClassPool> m_dic = new Dictionary<Type, ClassPool>();
        private static lhClassManager m_instance;
#if POOLPRINT
        private StringBuilder m_builder;
        private FileStream m_fileStream;
#endif
        public static lhClassManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhClassManager();
        }
        public lhClassManager()
        {
#if POOLPRINT
            m_builder = new StringBuilder();
            m_fileStream = new FileStream(Application.persistentDataPath + "/classManager.txt", FileMode.Append);
#endif
        }
        public void Dispose()
        {
#if POOLPRINT
            if (m_fileStream!=null)
            {
                m_fileStream.Close();
                m_builder = null;
            }
#endif
            Clear();
        }
        public void Update()
        {
#if POOLPRINT
            if (i_instance.m_fileStream != null)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(i_instance.m_builder.ToString());
                i_instance.m_fileStream.Write(bytes, 0, bytes.Length);
                i_instance.m_builder.Clear();
            }
#endif
        }
        public static void Store(ClassStoreData[] dataArr, Action onStoreOver)
        {
            int count = dataArr.Length;
            for (int i = 0; i < dataArr.Length; i++)
            {
                var data = dataArr[i];
                Store(data.type, data.count, data.onCreateHandler, data.onFreeHandler, () => {
                    count--;
                    if (count <= 0)
                    {
                        if (onStoreOver != null)
                        {
                            onStoreOver();
                        }
                    }
                });

            }
        }
        public static void Store(Type type, int count, Action<object> onCreateHandler = null, Action<object> onFreeHandler = null, Action onStoreOver = null)
        {
            if (m_instance.m_dic.ContainsKey(type))
            {
                m_instance.m_dic[type].freeHandler += onFreeHandler;
                m_instance.m_dic[type].createHandler += onCreateHandler;
                m_instance.m_dic[type].Store(count, onStoreOver);
            }
            else
            {
                m_instance.m_dic.Add(type, new ClassPool(type, count, onCreateHandler, onFreeHandler, onStoreOver));
            }
        }
        public static void Store<T>(int count, Action<object> onCreateHandler = null, Action<object> onFreeHandler = null, Action onStoreOver = null)
        {
            Type type = typeof(T);
            Store(type, count, onCreateHandler, onFreeHandler, onStoreOver);
        }
        public static T GetObject<T>()
        {
            return (T)GetObject(typeof(T));
        }
        public static T[] GetObject<T>(int count)
        {
            T[] arr = new T[count];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = GetObject<T>();
            }
            return arr;
        }
        public static object GetObject(Type type)
        {
            if (m_instance.m_dic.ContainsKey(type))
            {
                return m_instance.m_dic[type].GetObject();
            }
            else
            {
                lhDebug.LogWarning("LaoHan: this Class dont store =>" + type);
                m_instance.m_dic.Add(type, new ClassPool(type, 3, null, null, null));
                return m_instance.m_dic[type].GetObject();
            }
        }
        public static void FreeObject(object obj)
        {
            if (obj == null) return;
            Type type = obj.GetType();
            if (m_instance.m_dic.ContainsKey(type))
            {
                m_instance.m_dic[type].FreeObject(obj);
            }
            else
            {
                lhDebug.LogError("LaoHan: freeObject dont has this type =>" + type);
            }
        }
        public static void FreeObject<T>(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                FreeObject(arr[i]);
            }
        }
        public static void Clear()
        {
            foreach (var item in m_instance.m_dic)
            {
                item.Value.Clear();
            }
        }
        private static void WriteToLocal(string value)
        {
#if POOLPRINT
            i_instance.m_builder.Append("\n"+value);
#endif
        }
    }
}