using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace LaoHan.Infrastruture
{
    public class lhAssetManager
    {
        public static Transform poolParent { get; private set; }
        private class ObjectPool
        {
            public object id;
            public string sourcePath;

            private Queue<UnityEngine.Object> m_objQueue = new Queue<UnityEngine.Object>();
            public void Reverse(int count, Action<UnityEngine.Object> callback)
            {
                for (int i = 0; i < count; i++)
                {
                    CreateAsset((clone) =>
                    {
                        m_objQueue.Enqueue(clone);
                        if (callback != null)
                            callback(clone);
                    });
                }
            }
            public void GetAsset(Action<UnityEngine.Object> onGetOver)
            {
                if (m_objQueue.Count != 0)
                    onGetOver(m_objQueue.Dequeue());
                else
                    CreateAsset(onGetOver);
            }
            public void FreeAsset(UnityEngine.Object asset)
            {
                m_objQueue.Enqueue(asset);
            }
            private void CreateAsset(Action<UnityEngine.Object> onCreateOver)
            {
                lhResources.Load(sourcePath, onCreateOver);
            }
        }
        private Dictionary<object, ObjectPool> m_assetPoolDic = new Dictionary<object, ObjectPool>();
        private static lhAssetManager m_instance;
        public static lhAssetManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhAssetManager();
        }
        public void Dispose()
        {
            m_instance = null;
        }
        lhAssetManager()
        {

        }
        public static void Reverse(int id,string sourcePath,int count, Action<UnityEngine.Object> callback = null)
        {
            if (m_instance.m_assetPoolDic.ContainsKey(id))
            {
                m_instance.m_assetPoolDic[id].Reverse(count, callback);
            }
            else
            {
                var pool = new ObjectPool();
                pool.id = id;
                pool.sourcePath = sourcePath;
                pool.Reverse(count, callback);
                m_instance.m_assetPoolDic.Add(id, pool);
            }
        }
        public static void GetAsset(object id,Action<UnityEngine.Object> onGetOver)
        {
            if (m_instance.m_assetPoolDic.ContainsKey(id))
            {
                m_instance.m_assetPoolDic[id].GetAsset(onGetOver);
            }
            else
            {
                lhDebug.LogError((object)("LaoHan: this id dont Reverse to Pool: " + id));
            }
        }
        public static void FreeAsset(object id, UnityEngine.Object asset, float freeTime = -1, Action onFreeOver = null)
        {
            if (m_instance.m_assetPoolDic.ContainsKey(id))
            {
                if (asset != null)
                {
                    Action FreeObj = () =>
                    {
                        m_instance.m_assetPoolDic[id].FreeAsset(asset);
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
                Debug.LogWarning("LaoHan: pool is nont has this id:     " + id);
                return;
            }
        }
    }
}