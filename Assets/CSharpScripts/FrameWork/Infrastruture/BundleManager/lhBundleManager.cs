using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LaoHan.Infrastruture
{
    public class lhBundleManager
    {
        public class UsedObjectData
        {
            public string groupName;
            public string assetbundleName;
        }
        public class LoadData
        {
            public string rootPath;
            public string externalPath;
            public string assetBundleName;
            public string sourceName;
            public bool loading;
            public Action<UnityEngine.Object> loadOverHandler;
            
        }
        public class BundleReference
        {
            private AssetBundle assetBundle;
            private UnityEngine.Object mainAsset;

            private int m_count=1;
            public BundleReference(AssetBundle assetBundle,UnityEngine.Object asset)
            {
                this.mainAsset = asset;
                this.assetBundle = assetBundle;
            }
            public BundleReference(AssetBundle assetBundle)
            {
                this.assetBundle = assetBundle;
            }
            public void GetObject(string sourceName,Action<UnityEngine.Object> onLoadOver)
            {
                m_count++;
                if (mainAsset!=null && mainAsset.name.Equals(sourceName))
                {
                    onLoadOver(mainAsset);
                }
                else
                    lhCoroutine.StartCoroutine(ELoadAsset(assetBundle, sourceName, onLoadOver));
            }
            public void Add()
            {
                m_count++;
            }
            public bool Remove()
            {
                m_count--;
                if (m_count <= 0)
                {
                    assetBundle.Unload(false);
                    return true;
                }
                else
                    return false;
            }
            IEnumerator ELoadAsset(AssetBundle assetBundle,string sourceName,Action<UnityEngine.Object> onLoadOver)
            {
                var request = assetBundle.LoadAssetAsync(sourceName.ToLower(), typeof(UnityEngine.Object));
                yield return request;
                onLoadOver(request.asset);
            }
        }
        public UnityEngine.AssetBundleManifest manifest { get; set; }
        private List<LoadData> m_loadList = new List<LoadData>();
        private Dictionary<string, BundleReference> m_usedBundleDic = new Dictionary<string, BundleReference>();
        
        private Dictionary<UnityEngine.Object, UsedObjectData> m_usedObjectDic;
        private static lhBundleManager m_instance;
        public static lhBundleManager GetInstance(Action onReadyOver)
        {
            if (m_instance != null) return null;
            return m_instance = new lhBundleManager(onReadyOver);
        }
        lhBundleManager(Action onReadyOver)
        {
            if (lhDefine.projectType == ProjectType.develop)
            {
                onReadyOver();
                return;
            }
            m_usedObjectDic = new Dictionary<UnityEngine.Object, UsedObjectData>();
            if (Caching.ready)
            {
                Caching.CleanCache();
            }
            lhCoroutine.StartCoroutine(ELoadManifest(lhDefine.manifestUrl, (manifest) =>
            {
                this.manifest = manifest;
                onReadyOver();
            }));
        }
        public void Update()
        {
            for (int i = 0; i < m_loadList.Count; i++)
            {
                var loadData = m_loadList[i];
                if (!loadData.loading)
                {
                    loadData.loading = true;
                    var dependencies = manifest.GetAllDependencies(loadData.sourceName);
                    Action CanLoad = () =>
                    {
                        if (m_usedBundleDic.ContainsKey(loadData.assetBundleName))
                        {
                            m_usedBundleDic[loadData.assetBundleName].GetObject(loadData.sourceName, (o) =>
                            {
                                m_loadList.Remove(loadData);
                                loadData.loadOverHandler(o);
                            });
                        }
                        else
                        {
                            string url = PathCombine(loadData.rootPath, loadData.externalPath, loadData.assetBundleName + lhDefine.bundleStuff);
                            lhCoroutine.StartCoroutine(ELoadAssetBundle(url, loadData.sourceName, 0, 0, (assetbundle, obj) =>
                            {
                                m_usedBundleDic.Add(loadData.assetBundleName, new BundleReference(assetbundle, obj));
                                loadData.loadOverHandler(obj);
                                m_loadList.Remove(loadData);
                            }));
                        }
                    };
                    if (dependencies == null || dependencies.Length == 0)
                        CanLoad();
                    else
                    {
                        int count = dependencies.Length;
                        for (int j = 0; j < dependencies.Length; j++)
                        {
                            var dependency = dependencies[j];
                            if (!m_usedBundleDic.ContainsKey(dependency))
                            {
                                string url = PathCombine(loadData.rootPath, loadData.externalPath, dependency);
                                lhCoroutine.StartCoroutine(ELoadAssetBundle(url, 0, 0, (assetbbunde) =>
                                {
                                    m_usedBundleDic.Add(dependency, new BundleReference(assetbbunde));
                                    count--;
                                    if (count <= 0)
                                    {
                                        CanLoad();
                                    }
                                }));
                            }
                            else
                            {
                                m_usedBundleDic[dependencies[j]].Add();
                                count--;
                                if (count <= 0)
                                {
                                    CanLoad();
                                }
                            }
                        }
                    }
                }
            }
        }
        public void Dispose()
        {
            m_instance = null;
        }
        public static void Load(string rootPath,string externalPath,string assetBundleName,string sourceName,Action<UnityEngine.Object> onLoadOver,bool destroyReference=true)
        {
            externalPath = externalPath.ToLower();
            assetBundleName = assetBundleName.ToLower();
            m_instance.AddLoadCache(rootPath, externalPath, assetBundleName, sourceName,
                    (o) =>
                    {
                        if (o==null)
                        {
                            lhDebug.LogError("LaoHan: o =null  " + rootPath + "  "+externalPath + "  " + assetBundleName + "  " + sourceName);
                        }
                        //else
                        //    lhDebug.Log("LaoHan: "+o.GetType().Name+ "\nrootPath:  "+ rootPath + "\nexternanPath  " + externalPath + "  " + assetBundleName + "  " + sourceName);
                        if (!m_instance.m_usedObjectDic.ContainsKey(o))
                            m_instance.m_usedObjectDic.Add(o, new UsedObjectData() { groupName = externalPath, assetbundleName = assetBundleName });
                        onLoadOver(o);
                        if (destroyReference)
                            DestroyReference(o);
                    }
                );
        }
        public static void DestroyReference(UnityEngine.Object obj)
        {
            if(m_instance.m_usedObjectDic.ContainsKey(obj))
            {
                var data = m_instance.m_usedObjectDic[obj];
                if(m_instance.DestroyReference(data.assetbundleName))
                {
                    m_instance.m_usedObjectDic.Remove(obj);
                }
            }
            else
            {
                lhDebug.LogWarning((object)("LaoHan: this < " + obj + " > dont Load fron BundleManager"));
            }
        }
        private string PathCombine(params string[]  param)
        {
            string path="";
            foreach(var str in param)
            {
                path = System.IO.Path.Combine(path, str);
            }
            path=path.Replace("\\", "/");
            return path;
        }

        private void AddLoadCache(string rootPath, string externalPath, string assetBundleName, string sourceName, Action<UnityEngine.Object> onLoadOver)
        {
            bool has = false;
            for (int i = 0; i < m_loadList.Count; i++)
            {
                var data = m_loadList[i];
                if (data.rootPath.Equals(rootPath)
                    && data.externalPath.Equals(externalPath)
                    && data.assetBundleName.Equals(assetBundleName)
                    && data.sourceName.Equals(sourceName))
                {
                    has = true;
                    m_loadList[i].loadOverHandler += onLoadOver;
                }
            }
            if (!has)
            {
                m_loadList.Add(new LoadData()
                {
                    rootPath = rootPath,
                    externalPath = externalPath,
                    assetBundleName = assetBundleName,
                    sourceName = sourceName,
                    loadOverHandler = onLoadOver
                });
            }
        }

        private bool DestroyReference(string assetBundleName)
        {
            if (m_usedBundleDic.ContainsKey(assetBundleName))
            {
                if (m_usedBundleDic[assetBundleName].Remove())
                {
                    m_usedBundleDic.Remove(assetBundleName);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                lhDebug.LogWarning((object)("LaoHan: this < " + assetBundleName + " > dont exists in BundleReference"));
                return false;
            }
        }
        IEnumerator ELoadAssetBundle(string url, string sourceName, int version, uint crc, Action<AssetBundle, UnityEngine.Object> onCreateRequest)
        {
            using (WWW www = WWW.LoadFromCacheOrDownload("file:" + url, version, crc))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    lhDebug.LogError((object)("LaoHan:" + www.error));
                }
                var request = www.assetBundle.LoadAssetAsync(sourceName, typeof(UnityEngine.Object));
                yield return request;
                onCreateRequest(www.assetBundle, request.asset);
            }
        }
        IEnumerator ELoadAssetBundle(string url,uint crc,Action<AssetBundle> onCreateRequest)
        {
            var async=AssetBundle.LoadFromFileAsync("file:" + url, crc);
            yield return async;
            onCreateRequest(async.assetBundle);
        }
        IEnumerator ELoadAssetBundle(string url, int version, uint crc, Action<AssetBundle> onCreateRequest)
        {
            using (WWW www = WWW.LoadFromCacheOrDownload("file:" + url, version, crc))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    lhDebug.LogError((object)("LaoHan:" + www.error));
                    yield return new lhWaitForReturn();
                }
                onCreateRequest(www.assetBundle);
            }
        }
        IEnumerator ELoadAssetBundle(string url, int version, Action<AssetBundle> onCreateRequest)
        {
            using (WWW www = WWW.LoadFromCacheOrDownload("file:" + url, version))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError("LaoHan:" + www.error);
                    yield return new lhWaitForReturn();
                }
                onCreateRequest(www.assetBundle);
            }
        }
        IEnumerator ELoadManifest(string url,Action<UnityEngine.AssetBundleManifest> onLoadOver)
        {
            //string name =url+"/"+ Path.GetFileNameWithoutExtension(url);
            using (WWW www = WWW.LoadFromCacheOrDownload("file:" + url, 1))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError("LaoHan:" + www.error);
                    yield return new lhWaitForReturn();
                }
                onLoadOver((UnityEngine.AssetBundleManifest)www.assetBundle.LoadAsset("AssetBundleManifest"));
                www.assetBundle.Unload(false);
            }
        }
    }
}
