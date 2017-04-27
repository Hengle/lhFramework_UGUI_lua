using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using LaoHan.Infrastruture;
namespace LaoHan.UGUI
{
    public class lhUtilityBase : lhMonoBehaviour,IUtilityInterface
    {
        #region public Member
        public RectTransform rectTransform;
        public EUIState uiState;
        #endregion

        #region lhUtilityBase
        public virtual void Initialize(Action onInitialOver)
        {
            uiState = EUIState.Initialize;
            rectTransform.SetAsLastSibling();
            //onInitialOver();
        }
        public virtual void Open(Intent parameter, Action<Intent> onOpenOver)
        {
            uiState = EUIState.Open;
        }
        public virtual void Close(Action<Intent> onCloseOver)
        {
            uiState = EUIState.Close;
        }
        public virtual void Destroy(Action<Intent> onDestoryOver)
        {
            uiState = EUIState.Destroy;
            Resources.UnloadUnusedAssets();
        }
        public virtual void ReceiveMessage(object mark, object value)
        {
        }
        public virtual object Transmit(object mark,object value)
        {
            return null;
        }
        #endregion

    }
}