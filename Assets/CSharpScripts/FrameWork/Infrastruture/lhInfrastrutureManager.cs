using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using LaoHan.Data;
using LaoHan.Control;
using LaoHan.Network;
using LaoHan.Infrastruture.ulua;
namespace LaoHan.Infrastruture
{
    public class lhInfrastrutureManager : lhMonoBehaviour
    {
        public bool open_network=true;

        private static lhInfrastrutureManager m_instance;

        private lhCoroutine m_coroutine;
        private lhInvoke m_invoke;
        private lhResources m_resources;
        private lhComponent m_component;
        private lhRandom m_random;
        private lhDebug m_debug;
        private lhFrame m_frame;
        private lhHttp m_http; 
        private lhLoom m_loom;
        private lhLoop m_loop;
#if LUA
        private lhLuaManager m_luaManager;
#endif
#if UGUI
        private LaoHan.UGUI.lhUIManager m_uiManager;
#endif
#if FAIRYGUI
        private LaoHan.Fairy.lhUIManager m_uiManager;
#endif
        private lhNetwork m_network;
        private lhLanguage m_language;
        private lhCacheData m_cacheData;
        private lhConfigData m_configData;
        private lhMemoryData m_memoryData;
        private lhControlNetwork m_controlNetwork;
        private lhControlData m_controlData;
        private lhBundleManager m_bundleManager;
        private lhObjectManager m_objectManager;
        private lhAssetManager m_assetManager;
        private lhAudioManager m_audioManager;
        private lhSourceUpdate m_sourceUpdate;
        private lhSceneManager m_sceneManager;

        // Use this for initialization
        void Awake()
        {
            m_instance = this;
            DisposeAll();
            DontDestroyOnLoad(gameObject);
            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
            InitializeShader();
            InitializeBase();
        }
        void Update()
        {
            m_bundleManager.Update();
            m_invoke.Update();
            m_frame.Update();
            m_loom.Update();
            if(m_network != null)
                m_network.Update();
        }
        void LateUpdate()
        {
            m_coroutine.LateUpdate();
        }
        void OnGUI()
        {
            m_debug.OnGUI();
        }
        void OnDestroy()
        {
            DisposeAll();
        }
        void ApplicationQuit()
        {
            DisposeAll();
        }

        public void SourceUpdate(Action onUpdateOver)
        {
            m_instance.m_sourceUpdate.Updating(onUpdateOver);
        }
        public void InitializeGame(Action onInitialOver)
        {
            m_instance.m_bundleManager = lhBundleManager.GetInstance(() => {
                m_instance.m_configData = lhConfigData.GetInstance(
                    () => {
                        if (m_instance.open_network)
                        {
                            m_instance.m_network = lhNetwork.GetInstance();
                            m_instance.m_controlNetwork = lhControlNetwork.GetInstance(m_instance.m_network);
                        }
                        m_instance.m_controlData = lhControlData.GetInstance(m_instance.m_memoryData);
                        m_instance.m_memoryData = lhMemoryData.GetInstance();
                        m_instance.m_http = lhHttp.GetInstance();
                        m_instance.m_language = lhLanguage.GetInstance();
                        m_instance.m_audioManager = lhAudioManager.GetInstance();
                        m_instance.m_sceneManager = lhSceneManager.GetInstance();
                        m_instance.m_audioManager.Initialize(onInitialOver);
                    });
            });
        }
        public void StartGame(Action OnStartOver)
        {
#if LUA
                                 m_instance.m_luaManager = lhLuaManager.GetInstance();
#endif
#if UGUI
                                m_instance.m_uiManager = lhUIManager.GetInstance();
#endif

#if LUA
                                 m_instance.m_luaManager.Initialize(OnStartOver);
#endif
#if UGUI
                                m_instance.m_uiManager.Initialize(OnStartOver);
#endif

        }

        private void InitializeBase() { 
            m_debug = lhDebug.GetInstance();
            m_coroutine = lhCoroutine.GetInstance();
            m_invoke = lhInvoke.GetInstance();
            m_loom = lhLoom.GetInstance();
            m_loop = lhLoop.GetInstance();
            m_resources = lhResources.GetInstance();
            m_component = lhComponent.GetInstance();
            m_cacheData = lhCacheData.GetInstance();
            m_random = lhRandom.GetInstance(UnityEngine.Random.Range(0, int.MaxValue));
            m_frame = lhFrame.GetInstance(5);
            m_frame.AutoFPS(25, 60);
            m_objectManager = lhObjectManager.GetInstance();
            m_assetManager = lhAssetManager.GetInstance();
            m_sourceUpdate = lhSourceUpdate.GetInstance();
        }
        private void InitializeShader()
        {
            //Shader.WarmupAllShaders();
        }
        private void DisposeAll()
        {
            if (m_coroutine != null)
                m_coroutine.Dispose();
            if (m_invoke != null)
                m_invoke.Dispose();
            if (m_debug != null)
                m_debug.Dispose();
            if (m_component != null)
                m_component.Dispose();
            if (m_sourceUpdate!=null)
            {
                m_sourceUpdate.Dispose();
            }
#if LUA
            if (m_luaManager != null)
                m_luaManager.Dispose();
#endif

#if UGUI
            if (m_uiManager != null)
                m_uiManager.Dispose();
#endif
#if FAIRYGUI
            if (m_uiManager != null)
                m_uiManager.Dispose();
#endif
            if (m_audioManager != null)
                m_audioManager.Dispose();
            if (m_controlData != null)
                m_controlData.Dispose();
            if (m_cacheData != null)
                m_cacheData.Dispose();
            if (m_configData != null)
                m_configData.Dispose();
            if (m_loom != null)
                m_loom.Dispose();
            if (m_loop!=null)
                m_loop.Dispose();
            if (m_http != null)
                m_http.Dispose();
            if (m_network != null)
                m_network.Dispose();
            if (m_resources != null)
                m_resources.Dispose();
            if (m_bundleManager != null)
                m_bundleManager.Dispose();
            if (m_objectManager != null)
                m_objectManager.Dispose();
        }
    }
}

