using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using LaoHan.UGUI;
using LaoHan.Infrastruture;

public class lhGuide : lhMonoBehaviour{

    public GuideTrigger guidTrigger;
    public string guide;
    public lhUIBase uiBase;
    public Button buttonObj;
    void Awake()
    {
        switch(guidTrigger)
        {
            case GuideTrigger.None:
                break;
            case GuideTrigger.OnClick:
                buttonObj.onClick.AddListener(() => lhUIManager.NextGuide(guide));
                break;
            case GuideTrigger.UIClose:
                uiBase.uiStateHandler += (s) => { if (s == EUIState.Close)lhUIManager.NextGuide(guide); };
                break;
            case GuideTrigger.UIDestroy:
                uiBase.uiStateHandler += (s) => { if (s == EUIState.Destroy)lhUIManager.NextGuide(guide); };
                break;
            case GuideTrigger.UIOpen:
                uiBase.uiStateHandler += (s) => { if (s == EUIState.Open)lhUIManager.NextGuide(guide); };
                break;
            case GuideTrigger.UIInitialize:
                uiBase.uiStateHandler += (s) => { if (s == EUIState.Initialize)lhUIManager.NextGuide(guide); };
                break;
        }
    }
    
}