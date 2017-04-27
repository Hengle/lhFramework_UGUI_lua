//#define POOLPOINT

using UnityEngine;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;
#if POOLPOINT
using System.IO;
using System.Text;
#endif

namespace LaoHan.Infrastruture
{
    public enum EPoolType
    {
        /// <summary>
        /// 默认
        /// </summary>
        None,
        /// <summary>
        /// ui池
        /// </summary>
        UI,
        /// <summary>
        /// 模型池
        /// </summary>
        Model,
        /// <summary>
        /// 特效池
        /// </summary>
        Effect,
        /// <summary>
        /// 声音池
        /// </summary>
        Sound3D,
        /// <summary>
        /// unity资源池只要指ScriptObject
        /// </summary>
        ScriptObject,
    }
    public class ObjectStoreData
    {
        public object index;
        public string path;
        public Object obj;
        public int count = 1;
        public Action<Object> onCreateHandler;
        public Action onStoreOver;
        public EPoolType poolType;
    }
    public class lhObjectManager
    {
        public static Transform poolParent { get; private set; }
        private static lhObjectManager m_instance;
        private class ObjectPool
        {
            public EPoolType poolType;
            public object index;
            public UnityEngine.Object obj;
            public string path;
            public int capacity = 10;
            public Action<Object> createHandler;
            public Action storeOverHandler;

            private Queue<UnityEngine.Object> m_queue = new Queue<UnityEngine.Object>();
            public ObjectPool()
            {

            }
            public void Store(int count, Action<Object> onCreateHandler, Action onStoreOver)
            {
                createHandler = onCreateHandler;
                storeOverHandler = onStoreOver;
                Action ForStore = () =>
                {
                    for (int i = 0; i < count; i++)
                    {
                        var clone = CreateObj();
                        m_queue.Enqueue(clone);
                        WriteToLocal(System.DateTime.Now.ToString("MM月dd日hh时mm分ss秒  ") + obj + " =>Store    m_queue count:" + m_queue.Count + "                           " + poolType + "   " + index);
                    }
                };
                if (obj == null)
                {
                    lhResources.Load(path, (o) =>
                    {
                        obj = o;
                        ForStore();
                        if (storeOverHandler != null)
                        {
                            storeOverHandler();
                            storeOverHandler = null;
                        }
                    });
                }
                else
                {
                    //if (m_queue.Count> capacity)
                    //{
                    //    if (storeOverHandler != null)
                    //        storeOverHandler();
                    //    return;
                    //}
                    ForStore();
                    if (storeOverHandler != null)
                        storeOverHandler();
                }
            }
            public UnityEngine.Object GetObject()
            {
                WriteToLocal(System.DateTime.Now.ToString("MM月dd日hh时mm分ss秒  ") + obj + " =>GetObject    m_queue count:" + m_queue.Count + "                           " + poolType + "   " + index);
                if (m_queue.Count != 0)
                    return m_queue.Dequeue();
                else
                    return CreateObj();
            }
            public void FreeObject(UnityEngine.Object obj, bool backParent)
            {
                if (m_queue.Contains(obj)) return;
                WriteToLocal(System.DateTime.Now.ToString("MM月dd日hh时mm分ss秒  ") + obj + " =>FreeObject    m_queue count:" + m_queue.Count + "                           " + poolType + "   " + index);
                if (backParent)
                {
                    if (obj is GameObject)
                    {
                        ((GameObject)obj).transform.SetParent(poolParent);
                    }
                    else if (obj is ScriptableObject)
                    {

                    }
                    else
                        lhDebug.LogWarning((object)"obj is dont GameObject ");
                }
                else
                {

                    if (obj is GameObject)
                    {
                        ((GameObject)obj).SetActive(false);
                    }
                    else if (obj is ScriptableObject)
                    {

                    }
                    else
                        lhDebug.LogWarning((object)"obj is dont GameObject");
                }
                m_queue.Enqueue(obj);
            }
            public bool Contains(Object obj)
            {
                return m_queue.Contains(obj);
            }
            public void Clear()
            {
                var arr = m_queue.ToArray();
                foreach (var item in arr)
                {
                    UnityEngine.Object.Destroy(item);
                }
                m_queue.Clear();
            }
            private Object CreateObj()
            {
                if (obj is GameObject)
                {
                    GameObject o = (GameObject)UnityEngine.Object.Instantiate((GameObject)obj);
                    o.transform.SetParent(poolParent);
                    if (createHandler != null)
                        createHandler(o);
                    return o;
                }
                else if (obj is ScriptableObject)
                {
                    ScriptableObject o = ScriptableObject.Instantiate(obj) as ScriptableObject;
                    if (createHandler != null)
                        createHandler(o);
                    return o;
                }
                else
                {
                    lhDebug.LogWarning((object)"obj is dont GameObject ");
                    return null;
                }
            }
        }
        private Dictionary<EPoolType, Dictionary<object, ObjectPool>> m_idPool = new Dictionary<EPoolType, Dictionary<object, ObjectPool>>();
        public static lhObjectManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhObjectManager();
        }
#if POOLPOINT
        private StringBuilder m_builder;
        private FileStream m_fileStream;
#endif
        public lhObjectManager()
        {
#if POOLPOINT
            m_builder = new StringBuilder();
            m_fileStream = new FileStream(Application.persistentDataPath+"/objectManager.txt", FileMode.Append);
            m_builder.Length = 0;
			WriteToLocal(System.DateTime.Now.ToString("MM月dd日hh时mm分ss秒")+"  ------------------start");
#endif
            GameObject poolObject = new GameObject("[ObjectManager]");
            //UnityEngine.Object.DontDestroyOnLoad(poolObject);
            poolParent = poolObject.transform;
            poolObject.SetActive(false);
        }
        public void Dispose()
        {
#if POOLPOINT
            if (m_fileStream != null)
            {
                m_fileStream.Close();
                m_builder.Length = 0;
                m_builder = null;
            }
#endif
            Clear();
            UnityEngine.Object.Destroy(poolParent.gameObject);
            m_idPool = null;
        }
        public void Update()
        {
#if POOLPOINT
            if (m_instance.m_fileStream != null)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(m_instance.m_builder.ToString());
                m_instance.m_fileStream.Write(bytes, 0, bytes.Length);
                m_instance.m_builder.Length = 0;
            }
#endif
        }
        public static bool Contains(object index, EPoolType type = EPoolType.None)
        {
            if (m_instance.m_idPool.ContainsKey(type))
            {
                if (m_instance.m_idPool[type].ContainsKey(index))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }
        public static bool Contains(object[] indexArr)
        {
            bool has = false;
            for (int i = 0; i < indexArr.Length; i++)
            {
                has = Contains(indexArr[i]);
                if (has)
                {
                    return has;
                }
            }
            return has;
        }
        public static void Store(ObjectStoreData[] dataArr, Action onStoreOver)
        {
            if (dataArr == null) return;
            int count = dataArr.Length;
            for (int i = 0; i < dataArr.Length; i++)
            {
                var data = dataArr[i];
                if (data.obj == null)
                    Store(data.index, data.path, data.count, data.onCreateHandler, () => {
                        count--;
                        if (count <= 0)
                        {
                            if (onStoreOver != null)
                                onStoreOver();
                        }
                        if (data.onStoreOver != null)
                            data.onStoreOver();
                    }, data.poolType);
                else
                    Store(data.index, data.obj, data.count, data.onCreateHandler, () => {
                        count--;
                        if (count <= 0)
                        {
                            if (onStoreOver != null)
                                onStoreOver();
                        }
                        if (data.onStoreOver != null)
                            data.onStoreOver();
                    }, data.poolType);
            }
        }
        public static void Store(object index, string path, int count = 1, Action<Object> OnCreateHandler = null, Action onStoreOver = null, EPoolType type = EPoolType.None)
        {
            if (m_instance.m_idPool.ContainsKey(type))
            {
                if (m_instance.m_idPool[type].ContainsKey(index))
                {
                    if (m_instance.m_idPool[type][index].obj == null)
                    {
                        m_instance.m_idPool[type][index].storeOverHandler += onStoreOver;
                    }
                    else
                    {
                        if (onStoreOver != null)
                        {
                            onStoreOver();
                        }
                    }
                    //m_instance.m_idPool[type][index].Store(count, OnCreateHandler, onStoreOver);
                }
                else
                {
                    var pool = new ObjectPool();
                    pool.index = index;
                    pool.path = path;
                    pool.poolType = type;
                    m_instance.m_idPool[type].Add(index, pool);
                    pool.Store(count, OnCreateHandler, onStoreOver);
                }
            }
            else
            {
                var dic = new Dictionary<object, ObjectPool>();
                var pool = new ObjectPool();
                pool.index = index;
                pool.path = path;
                pool.poolType = type;
                dic.Add(index, pool);
                m_instance.m_idPool.Add(type, dic);
                pool.Store(count, OnCreateHandler, onStoreOver);
            }
        }
        public static void Store(object index, UnityEngine.Object obj, int count, Action<Object> onCreateHandler = null, Action onStoreOver = null, EPoolType type = EPoolType.None)
        {
            if (m_instance.m_idPool.ContainsKey(type))
            {
                if (m_instance.m_idPool[type].ContainsKey(index))
                {
                    if (onStoreOver != null)
                    {
                        onStoreOver();
                    }
                    //m_instance.m_idPool[type][index].Store(count, onCreateHandler, onStoreOver);
                }
                else
                {
                    var pool = new ObjectPool();
                    pool.index = index;
                    pool.obj = obj;
                    obj.hideFlags = HideFlags.HideInHierarchy;
                    pool.poolType = type;
                    m_instance.m_idPool[type].Add(index, pool);
                    pool.Store(count, onCreateHandler, onStoreOver);
                }
            }
            else
            {
                var dic = new Dictionary<object, ObjectPool>();
                var pool = new ObjectPool();
                pool.index = index;
                pool.obj = obj;
                obj.hideFlags = HideFlags.HideInHierarchy;
                pool.poolType = type;
                dic.Add(index, pool);
                m_instance.m_idPool.Add(type, dic);
                pool.Store(count, onCreateHandler, onStoreOver);
            }
        }
        public static UnityEngine.Object GetObject(object index, EPoolType type = EPoolType.None)
        {
            if (m_instance.m_idPool.ContainsKey(type))
            {
                var dic = m_instance.m_idPool[type];
                if (dic.ContainsKey(index))
                {
                    var o = dic[index].GetObject();
                    return o;
                }
                else
                {
                    if (index is GameObject)
                    {
                        GameObject obj = index as GameObject;
                        var pool = new ObjectPool();
                        pool.index = index;
                        pool.obj = obj;
                        pool.poolType = type;
                        m_instance.m_idPool[type].Add(index, pool);
                        var o = pool.GetObject();
                        return o;
                    }
                    else
                    {
                        lhDebug.LogError((object)("LaoHan: pool dont has this id:  " + type + "     " + index));
                        return null;
                    }
                }
            }
            else
            {
                lhDebug.LogError((object)("LaoHan: pool dont has this type:  " + type));
                return null;
            }
        }
        public static UnityEngine.Object GetObject(object index, Vector3 position, Quaternion rotation, Transform parent = null, bool activate = true, EPoolType type = EPoolType.None)
        {
            UnityEngine.Object obje = GetObject(index, type);
            if (obje is GameObject)
            {
                ((GameObject)obje).transform.SetParent(parent);
                ((GameObject)obje).transform.localPosition = position;
                ((GameObject)obje).transform.localRotation = rotation;
                ((GameObject)obje).SetActive(activate);
            }
            else
                lhDebug.LogError((object)"obj is dont GameObject and ScriptObject");
            return obje;
        }
        public static void FreeObject(object index, UnityEngine.Object waitFree, float freeTime = -1, EPoolType type = EPoolType.None, bool backParent = true, Action onFreeOver = null)
        {
            if (m_instance.m_idPool.ContainsKey(type))
            {
                var dic = m_instance.m_idPool[type];

                if (dic.ContainsKey(index))
                {
                    if (waitFree != null && !dic[index].Contains(waitFree))
                    {
                        Action FreeObj = () =>
                        {
                            dic[index].FreeObject(waitFree, backParent);
                            if (onFreeOver != null)
                                onFreeOver();
                        };
                        if (freeTime > 0)
                        {
                            lhInvoke.Invoke(FreeObj, freeTime);
                        }
                        else
                        {
                            FreeObj();
                        }
                    }
                }
                else
                {
                    lhDebug.LogWarning("LaoHan: pool is nont has this id:     " + type + "    " + index);
                    return;
                }
            }
            else
            {
                lhDebug.LogWarning("LaoHan: pool is nont has this type:     " + type);
                return;
            }
        }
        public static void FreeObject(UnityEngine.Object waitFree, float freeTime = -1, EPoolType type = EPoolType.None, bool backParent = true, Action onFreeOver = null)
        {
            if (waitFree == null) return;
            var index = GetIndex(waitFree, type);
            if (index == null)
            {
                lhDebug.LogWarning("LaoHan: waitFree dont store:    " + waitFree + "     " + type);
                return;
            }
            FreeObject(index, waitFree, freeTime, type, backParent, onFreeOver);
        }
        public static void Clear(EPoolType type)
        {
            if (m_instance.m_idPool.ContainsKey(type))
            {
                foreach (var item in m_instance.m_idPool[type])
                {
                    item.Value.Clear();
                }
            }
            else
            {
                lhDebug.LogWarning("LaoHan: pool is nont has this type:     " + type);
                return;
            }
        }
        public static void Clear(object index, EPoolType type)
        {
            if (m_instance.m_idPool.ContainsKey(type))
            {
                var dic = m_instance.m_idPool[type];
                if (dic.ContainsKey(index))
                {
                    dic[index].Clear();
                    dic.Remove(index);
                }
                else
                {
                    lhDebug.LogWarning("LaoHan: pool is nont has this id:     " + type + "    " + index);
                    return;
                }
            }
            else
            {
                lhDebug.LogWarning("LaoHan: pool is nont has this type:     " + type);
                return;
            }
        }
        public static void Clear()
        {
            foreach (var item in m_instance.m_idPool)
            {
                Clear(item.Key);
            }
        }
        public static object GetIndex(UnityEngine.Object waitFree, EPoolType type = EPoolType.None)
        {
            if (m_instance.m_idPool.ContainsKey(type))
            {
                foreach (var item in m_instance.m_idPool[type])
                {
                    if (waitFree.name.Contains(item.Value.obj.name))
                    {
                        return item.Key;
                    }
                }
                lhDebug.LogWarning("LaoHan: pool is nont has waitFree:     " + type + "     " + waitFree);
                return null;
            }
            else
            {
                lhDebug.LogWarning("LaoHan: pool is nont has this type:     " + type);
                return null;
            }
        }
        public static object GetSource(object index, EPoolType type)
        {
            if (m_instance.m_idPool.ContainsKey(type))
            {
                var dic = m_instance.m_idPool[type];
                if (dic.ContainsKey(index))
                {
                    return dic[index].obj;
                }
                else
                {
                    lhDebug.LogWarning("LaoHan: pool is nont has index:     " + index);
                    return null;
                }
            }
            else
            {
                lhDebug.LogWarning("LaoHan: pool is nont has this type:     " + type);
                return null;
            }
        }
        private static void WriteToLocal(string value)
        {
#if POOLPOINT
            m_instance.m_builder.Append("\n" + value);
#endif
        }
    }
}