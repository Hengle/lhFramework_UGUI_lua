using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using LaoHan.Data;

namespace LaoHan.Infrastruture
{
    public class lhSceneManager
    {

        public static string currentLevel { get; private set; }
        public static string currentSceneName { get; private set; }
        private static lhSceneManager m_instance;

        //private AssetBundle bundle;
        public static lhSceneManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhSceneManager();
        }
        lhSceneManager()
        {
            currentLevel = "-1";
            currentSceneName = "Scene/Entry";
            if (Caching.ready)
                Caching.CleanCache();
        }
        public static void Load(string sceneId, Action<float> progressHandler, Action onLoadOver, LoadSceneMode mode)
        {
            if (!lhConfigData.sceneConfig.ContainsKey(sceneId))
            {
                lhDebug.LogError("LaoHan: sceneConfig dont has this scene, sceneId:" + sceneId);
                return;
            }
            var sceneName = lhConfigData.sceneConfig[sceneId];
            if (lhDefine.projectType == ProjectType.develop)
            {
                lhCoroutine.StartCoroutine(m_instance.EAsyncLoad("Scene/" + sceneName, progressHandler, () => { onLoadOver(); }, mode));
            }
            else
            {
                string url = lhDefine.sourceUrl + lhDefine.sceneFolder + "/" + lhDefine.platform + "/" + sceneName + lhDefine.sceneStuff;
                lhCoroutine.StartCoroutine(m_instance.ELoadSceneAsset(url, 0, 0, (p) =>
                {
                    lhCoroutine.StartCoroutine(m_instance.EAsyncLoad(sceneName, progressHandler, () => { p(); onLoadOver(); }, mode));
                }));
            }
            currentLevel = sceneId;
            currentSceneName = sceneName;
        }
        public static bool Unload()
        {
            return SceneManager.UnloadScene(currentLevel);
        }
        IEnumerator EAsyncLoad(string levelName, Action<float> progressHandler, Action onLoadOver, LoadSceneMode mode)
        {
            var async = SceneManager.LoadSceneAsync(levelName, mode);
            while (true)
            {
                if (progressHandler != null)
                {
                    progressHandler(async.progress);
                }
                if (async.isDone)
                {
                    if (onLoadOver != null)
                    {
                        onLoadOver();
                    }
                    break;
                }
                yield return 1;
            }
        }
        IEnumerator ELoadSceneAsset(string url, int version, uint crc, Action<Action> onLoadOver)
        {
            using (WWW www = WWW.LoadFromCacheOrDownload("file:" + url, version, crc))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    lhDebug.LogError((object)("LaoHan:" + www.error));
                }
                AssetBundle bundle = www.assetBundle;
                onLoadOver(() =>
                {
                    bundle.Unload(false);
                });
            }
        }
    }
}