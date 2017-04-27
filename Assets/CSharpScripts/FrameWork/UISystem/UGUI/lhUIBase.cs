using UnityEngine;
using System.Collections.Generic;
using System;
using LaoHan.Infrastruture;
namespace LaoHan.UGUI
{
    public class lhUIBase : lhMonoBehaviour,IUIInterface
    {
        #region public member
        public RectTransform rectTransform;
        public EUIState uiState { get; private set; }
        public Action<EUIState> uiStateHandler;
        #endregion

        #region protected member
        #endregion

        #region private member
        #endregion

        #region unity Methods
        #endregion

        #region lhUIBase
        public virtual void Initialize(Action onInitialOver)
        {
            uiState = EUIState.Initialize;
            if(uiStateHandler!=null)
                uiStateHandler(uiState);
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            //onInitialOver();
        }
        public virtual void Open(Intent parameter, Action<Intent> onOpenOver)
        {
            uiState = EUIState.Open;
            if (uiStateHandler != null)
                uiStateHandler(uiState);
        }
        public virtual void Close(Action<Intent> onCloseOver)
        {
            uiState = EUIState.Close;
            if (uiStateHandler != null)
                uiStateHandler(uiState);
        }
        public virtual void Destroy(Action<Intent> onDestoryOver)
        {
            uiState = EUIState.Destroy;
            Resources.UnloadUnusedAssets();
            if (uiStateHandler != null)
                uiStateHandler(uiState);
        }
        public virtual void ReceiveMessage(object mark, object value)
        {
        }
        public virtual object Transmit(object mark, object value)
        {
            return null;
        }
        #endregion

        #region Server Drive Methods
        #endregion

        #region Button Event
        #endregion

        #region protected methods
        protected virtual void EnterUtility_Dialog(string title, string message, string ok, Action<lhUtilityBase,bool> clickHandler)
        {
            lhUIManager.EnterUtility("Utility_Dialog", (utilityBase) =>
            {
                Intent dic = new Intent();
                dic.PutExtras("Title", title);
                dic.PutExtras("Message", message);
                dic.PutExtras("Ok", ok);
                dic.PutExtras("lhUtilityBase", utilityBase);
                dic.PutExtras("ClickHandler", clickHandler);
                utilityBase.Open(dic, null);
            });
        }
        protected virtual void EnterUtility_Dialog(string title, string message, string ok, string cancel, Action<lhUtilityBase,bool> clickHandler)
        {
            lhUIManager.EnterUtility("Utility_Dialog", (utilityBase) =>
            {
                Intent dic = new Intent();
                dic.PutExtras("Title", title);
                dic.PutExtras("Message", message);
                dic.PutExtras("Ok", ok);
                dic.PutExtras("Cancel", cancel);
                dic.PutExtras("ClickHandler", clickHandler);
                utilityBase.Open(dic, null);
            });
        }
        protected virtual void EnterUtility_Dialog(string title, string message, string ok)
        {
            EnterUtility_Dialog(title, message, ok, (b,a) => {
                lhUIManager.CloseUtility("Utility_Dialog", b); 
            });
        }
        protected virtual void CloseUtility_Dialog(lhUtilityBase utilityBase,Action<Intent> onCloseOver=null)
        {
            lhUIManager.CloseUtility("Utility_Dialog", utilityBase, onCloseOver);
        }
        protected virtual void EnterUtility_Loading(Action<lhUtilityBase> onOpenOver)
        {
            lhUIManager.EnterUtility("Utility_Loading", (utilityBase) =>
            {
                utilityBase.Open(null, (o) =>
                {
                    onOpenOver(utilityBase);
                });
            });
        }
        protected virtual void CloseUtility_Loading(lhUtilityBase utilityBase,Action<Intent> onCloseOver=null)
        {
            lhUIManager.CloseUtility("Utility_Loading", utilityBase, onCloseOver);
        }

        protected void SetRectTransform(RectTransform target, Transform parent)
        {
            SetRectTransform(target, parent, Vector2.one, Vector2.zero, Vector2.one);
        }
        protected void SetRectTransform(RectTransform target, Transform parent, Vector2 localScale, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            target.SetParent(parent);
            target.localScale = localScale;
            target.anchoredPosition = anchoredPosition;
            target.sizeDelta = sizeDelta;
        }
        protected void UIScreenAdaptation(Transform trans)
        {
            RectTransform tr = trans.gameObject.GetComponent<RectTransform>();

            Canvas canvas = transform.root.GetComponent<Canvas>();
            float standardAspect = lhDefine.baseUIRatio;
            
            float scaleFactor = canvas.scaleFactor;
            float deviceAspect = (Screen.width / scaleFactor) / (Screen.height / scaleFactor);

            //float width = tr.sizeDelta.x / (Screen.height / scaleFactor / lhMacro.baseUIHeight);
            //float height = tr.sizeDelta.y / (Screen.width / scaleFactor / lhMacro.baseUIWidth);
            //tr.sizeDelta = new Vector2(width, height);

            if (deviceAspect < standardAspect) //按高度适配
            {
                float width = tr.localScale.x / (Screen.height / scaleFactor / lhDefine.baseUIHeight);
                tr.localScale = new Vector2(width, tr.localScale.y);
            }
            else if (deviceAspect > standardAspect) //按宽度适配
            {
                float height = tr.localScale.y / (Screen.width / scaleFactor / lhDefine.baseUIWidth);
                tr.localScale = new Vector2(tr.localScale.x, height);
            }
        }
        protected float GetTextWidth(Font font, int fontSize, string content, FontStyle style = FontStyle.Normal)
        {
            font.RequestCharactersInTexture(content, fontSize, style);
            CharacterInfo characterInfo;
            float width = 0f;
            for (int i = 0; i < content.Length; i++)
            {

                font.GetCharacterInfo(content[i], out characterInfo, fontSize);
                width += characterInfo.width;
                //width += characterInfo.advance;unity5.x提示此方法替代width
            }
            return width;
        }
        #endregion

        #region private methods
        #endregion


    }
}