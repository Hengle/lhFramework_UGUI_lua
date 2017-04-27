using UnityEngine;
using System;
using System.Collections.Generic;
using LaoHan.Infrastruture;
using LaoHan.Data;

using UnityEngine.EventSystems;
namespace LaoHan.UGUI
{
    public class lhUIManager : lhMonoBehaviour
    {
        public Transform defaultParent;
        private static lhUIManager m_instance;
        private Dictionary<string, lhUIBase> m_uiLibrary;
        private Dictionary<string, GameObject> m_utilityLibrary;

        private Dictionary<string, lhConfigData.UIConfig.Library> m_uiPath;
        private Dictionary<string, lhConfigData.UIConfig.Library> m_utilityPath;
        
        private Dictionary<string, lhConfigData.UIConfig.GuideData> m_guidePath;

        private lhUtilityBase m_networkLoading;
        private lhUIBase m_currentUI;
        private string m_currentGuide;
        private Action<object, object> m_uiMessageHandler;
        public void Initialize(Action onInitialOver)
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(GameObject.FindObjectOfType(typeof(EventSystem)));
            m_uiPath = new Dictionary<string, lhConfigData.UIConfig.Library>();
            m_utilityPath = new Dictionary<string, lhConfigData.UIConfig.Library>();
            m_uiLibrary = new Dictionary<string, lhUIBase>();
            m_utilityLibrary = new Dictionary<string, GameObject>();
            m_guidePath = new Dictionary<string, lhConfigData.UIConfig.GuideData>();
            var uiType = lhConfigData.uiConfig;
            Action OnInitial = () =>
            {
                for (int i = 0; i < uiType.uiLibrary.Count; i++)
                {
                    string uiClass = uiType.uiLibrary[i].uiClass;
                    m_uiPath.Add(uiClass, uiType.uiLibrary[i]);
                }
                for (int i = 0; i < uiType.utilityLibrary.Count; i++)
                {
                    string uiClass = uiType.utilityLibrary[i].uiClass;
                    m_utilityPath.Add(uiClass, uiType.utilityLibrary[i]);
                }
                if (uiType.guide != null)
                {
                    for (int i = 0; i < uiType.guide.Count; i++)
                    {
                        string guideId = uiType.guide[i].guidId;
                        m_guidePath.Add(guideId, uiType.guide[i]);
                    }
                }
                lhAudioManager.Play2D(ESound2DType.BackMusic, "back1");
                if (uiType.defaultData != null)
                {
                    if (!string.IsNullOrEmpty(uiType.defaultData.uiClass))
                        EnterUI(uiType.defaultData.uiClass, (uiBase) => { uiBase.Open(null, null); onInitialOver(); });
                    else
                        onInitialOver();
                }
                else
                    onInitialOver();
            };
            if (!string.IsNullOrEmpty(uiType.defaultData.uiRoot))
            {
                lhResources.Load(uiType.defaultData.uiRoot, o =>
                {
                    GameObject obj = o as GameObject;
                    defaultParent = lhInstantiate(obj).transform;
                    OnInitial();
                });
            }
            else
            {
                if (defaultParent == null)
                    defaultParent = transform;
                OnInitial();
            }
        }
        
        public void Dispose()
        {
            m_uiLibrary = null;
            m_utilityLibrary = null;
            m_uiPath = null;
            m_utilityPath = null;
            m_instance = null;
        }
        public static lhUIManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = (lhUIManager)GameObject.FindObjectOfType(typeof(lhUIManager));
        }
        public static Transform GetCanvasTransform()
        {
            return m_instance.defaultParent;
        }
        public static void EnterUI(string uiClass, Transform parent, Action<lhUIBase> onEnterOver = null,Action<Intent> onCloseOver=null, bool closeCurrentUI = true)
        {
            if (!m_instance.m_uiLibrary.ContainsKey(uiClass))
            {
                if (!m_instance.m_uiPath.ContainsKey(uiClass))
                {
                    lhDebug.LogError((object)("LaoHan: uiPath dont has this Class: < " + uiClass + " >"));
                    return;
                }
                lhResources.Load(m_instance.m_uiPath[uiClass].uiPath, (o) =>
                {
                    GameObject obj = o as GameObject;

                    lhUIBase uiBase = (m_instance.lhInstantiate((GameObject)obj)).GetComponent<lhUIBase>();
                    m_instance.SetRectTransform(uiBase.rectTransform, parent);
                    uiBase.gameObject.SetActive(false);
                    uiBase.Initialize(() =>
                    {
                        if (closeCurrentUI)
                        {
                            if (m_instance.m_currentUI != null)
                                m_instance.m_currentUI.Close(onCloseOver);
                            m_instance.m_currentUI = uiBase;
                        }
                        m_instance.m_uiLibrary.Add(uiClass, uiBase);
                        if (onEnterOver != null)
                            onEnterOver(uiBase);
                        m_instance.m_uiMessageHandler += uiBase.ReceiveMessage;
                    });
                });
            }
            else
            {
                lhUIBase uiBase = m_instance.m_uiLibrary[uiClass];
                //uiBase.gameObject.SetActive(true);
                if (closeCurrentUI && m_instance.m_currentUI != null)
                {
                    m_instance.m_currentUI.Close(onCloseOver);
                    m_instance.m_currentUI = uiBase;
                }
                if (onEnterOver != null)
                    onEnterOver(uiBase);
            }
        }
        public static void EnterUI(string uiClass, Action<lhUIBase> onEnterOver = null, Action<Intent> onCloseOver = null, bool closeCurrentUI = true)
        {
            if (m_instance.defaultParent == null)
            {
                lhDebug.LogError((object)"LaoHan: defaultParent is null");
                return;
            }
            EnterUI(uiClass, m_instance.defaultParent, onEnterOver,onCloseOver, closeCurrentUI);
        }
        public static void CloseUI(string uiClass, Action<Intent> onCloseOver = null)
        {
            if (m_instance.defaultParent == null)
            {
                lhDebug.LogError((object)"LaoHan: defaultParent is null");
                return;
            }
            if (!m_instance.m_uiLibrary.ContainsKey(uiClass))
            {
                lhDebug.LogWarning((object)("LaoHan: m_uiLibrary dont has this Class : < " + uiClass + " > "));
            }
            else
            {
                if (!m_instance.m_uiPath.ContainsKey(uiClass))
                {
                    lhDebug.LogError((object)("LaoHan: uiPath dont has this Class: < " + uiClass + " >"));
                }
                else
                {
                    lhUIBase uiBase = m_instance.m_uiLibrary[uiClass];
                    uiBase.gameObject.SetActive(false);
                    if(uiBase.uiState!=EUIState.Close)
                        uiBase.Close(onCloseOver);
                    else
                    {
                        lhDebug.LogWarning((object)("LaoHan: Class < " + uiClass + " > UIState < " + uiBase.uiState + " >"));
                    }
                }
            }
        }
        public static void DestroyUI(string uiClass, Action<Intent> onDestroyOver = null)
        {
            if (m_instance.defaultParent == null)
            {
                lhDebug.LogError((object)"LaoHan: defaultParent is null");
                return;
            }
            if (!m_instance.m_uiLibrary.ContainsKey(uiClass))
            {
                lhDebug.LogWarning((object)("LaoHan: m_uiLibrary dont has this Class : < " + uiClass + " > "));
            }
            else
            {
                if (!m_instance.m_uiPath.ContainsKey(uiClass))
                {
                    lhDebug.LogError((object)("LaoHan: uiPath dont has this Class: < " + uiClass + " >"));
                }
                else
                {
                    lhUIBase uiBase = m_instance.m_uiLibrary[uiClass];
                    uiBase.gameObject.SetActive(false);
                    if (uiBase.uiState != EUIState.Destroy)
                    {
                        m_instance.m_uiMessageHandler -= uiBase.ReceiveMessage;
                        m_instance.m_uiLibrary.Remove(uiClass);
                        uiBase.Destroy(onDestroyOver);
                        UnityEngine.Object.Destroy(uiBase.gameObject);
                    }
                    else
                    {
                        lhDebug.LogWarning((object)("LaoHan: Class < " + uiClass + " > UIState < " + uiBase.uiState + " >"));
                    }
                }
            }
        }
        public static void EnterUtility(string uiClass, Transform parent, Action<lhUtilityBase> onEnterOver = null)
        {
            Action<GameObject> GO = (obj) =>
            {
                GameObject go = m_instance.lhInstantiate((GameObject)obj, true);
                
                lhUtilityBase utilityBase = go.GetComponent<lhUtilityBase>();
                m_instance.SetRectTransform(utilityBase.rectTransform, parent);
                if (utilityBase.uiState == EUIState.None)
                {
                    utilityBase.Initialize(() =>
                    {
                        if (onEnterOver != null)
                            onEnterOver(utilityBase);
                    });
                }
                else
                    if (onEnterOver != null)
                        onEnterOver(utilityBase);
                m_instance.m_uiMessageHandler += utilityBase.ReceiveMessage;
            };
            if (!m_instance.m_utilityLibrary.ContainsKey(uiClass))
            {
                lhResources.Load(m_instance.m_utilityPath[uiClass].uiPath, (o) =>
                {
                    GameObject obj = o as GameObject;
                    m_instance.m_utilityLibrary.Add(uiClass, obj);
                    GO(obj);
                });
            }
            else
            {
                GameObject obj = m_instance.m_utilityLibrary[uiClass];
                GO(obj);
            }
        }
        public static void EnterUtility(string uiClass, Action<lhUtilityBase> onEnterOver = null)
        {
            if (m_instance.defaultParent == null)
            {
                lhDebug.LogError((object)"LaoHan: defaultParent is null");
                return;
            }
            EnterUtility(uiClass, m_instance.defaultParent, onEnterOver);
        }
        public static void CloseUtility(string uiClass, lhUtilityBase utilityBase, Action<Intent> onCloseOver = null)
        {
            if (!m_instance.m_utilityLibrary.ContainsKey(uiClass))
            {
                lhDebug.LogError((object)("LaoHan: uiClass < " + uiClass + " > dont in utilityLibrary"));
                return;
            }
            utilityBase.Close(onCloseOver);
            m_instance.lhFreeInstantiate(m_instance.m_utilityLibrary[uiClass], utilityBase.gameObject);
        }
        public static void DestoryUtility(string uiClass,lhUIBase utilityBase,Action<Intent> onDestroyOver=null)
        {
            if (!m_instance.m_utilityLibrary.ContainsKey(uiClass))
            {
                lhDebug.LogError((object)("LaoHan: uiClass < " + uiClass + " > dont in utilityLibrary"));
                return;
            }
            m_instance.m_utilityLibrary.Remove(uiClass);
            utilityBase.Destroy(onDestroyOver);
            UnityEngine.Object.Destroy(utilityBase.gameObject);
        }
        public static lhUIBase GetUIBase(string uiClass)
        {
            if (m_instance.m_uiLibrary.ContainsKey(uiClass))
                return m_instance.m_uiLibrary[uiClass];
            else
                return null;
        }
        public static void NextGuide(string guide)
        {
            var guideData = m_instance.m_guidePath[guide];
            lhResources.Load(guideData.uiPath, (o) =>
            {
                GameObject obj = o as GameObject;
                lhGuideBase guideBase = obj.GetComponent<lhGuideBase>();
                List<string> processObjList = new List<string>();
                List<string> maskObjList = new List<string>();
                for (int j = 0; j < guideData.process.Count; j++)
                {
                    var dic = guideData.process[j];
                    processObjList.Add(dic.processObj);
                    maskObjList.Add(dic.maskObj);
                }
                guideBase.Initialize(processObjList, maskObjList);
                guideBase.EnterNext();
            });
        }
        public static void SetActiveUIRoot(bool show)
        {
            m_instance.defaultParent.gameObject.SetActive(show);
        }
        public static void DispatchMessage(object mark,object value)
        {
            if(m_instance.m_uiMessageHandler==null)
            {
                lhDebug.LogError((object)"LaoHan: UILibrary is null");
                return;
            }
            m_instance.m_uiMessageHandler(mark, value);
        }
        public static void ShowNetworkLoading(Action onShowOver=null)
        {
            if(m_instance.m_networkLoading==null)
            {
                EnterUtility("Utility_Loading", (utilityBase) =>
                {
                    m_instance.m_networkLoading=utilityBase ;
                    utilityBase.Open(null, null);
                    if (onShowOver!=null)
                        onShowOver();
                });
            }
            else
            {
                m_instance.m_networkLoading.Open(null, null);
                if (onShowOver != null)
                    onShowOver();
            }
        }
        public static void HideNetworkLoading()
        {
            m_instance.m_networkLoading.Close(null);
        }
        void SetRectTransform(RectTransform target, Transform parent)
        {
            SetRectTransform(target, parent, Vector2.one, Vector2.zero, Vector2.one);
        }
        void SetRectTransform(RectTransform target, Transform parent, Vector2 localScale, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            target.SetParent(parent);
            target.localScale = localScale;
            target.anchoredPosition = anchoredPosition;
            target.sizeDelta = sizeDelta;
        }
    }
}
