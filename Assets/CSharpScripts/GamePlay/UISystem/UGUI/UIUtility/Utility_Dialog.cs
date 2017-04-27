using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

using LaoHan.Infrastruture;
namespace LaoHan.UGUI
{
    public class Utility_Dialog : lhUtilityBase
    {
        public Text title;
        public Text message;
        public Text ok;
        public Text okCenter;
        public Text cancel;
        public GameObject okButton;
        public GameObject okCenterButton;
        public GameObject camcelButton;

        private Action<lhUtilityBase,bool> clickHandler;
        #region Unity Methods
        void OnDisable()
        {
            clickHandler = null;
        }
        void OnDestory()
        {
            clickHandler = null;
        }
        #endregion

        #region lhUtilityBase
        public override void Initialize(Action onInitialOver)
        {
            onInitialOver();
        }
        public override void Open(Intent parameter, Action<Intent> onOpenOver)
        {

            string title = (string)parameter.GetExtras("Title");
            string message = (string)parameter.GetExtras("Message");
            string ok = (string)parameter.GetExtras("Ok");
            clickHandler = (Action<lhUtilityBase,bool>)parameter.GetExtras("ClickHandler");
            if (parameter.HasExtras("Cancel"))
                DisplayDialog(title, message, ok, (string)parameter.GetExtras("Cancel"));
            else
                DisplayDialog(title, message, ok);
            base.Open(parameter, onOpenOver);
            if (onOpenOver!=null)
                onOpenOver(null);
        }
        public override void Close(Action<Intent> onCloseOver)
        {
            gameObject.SetActive(false);
            if (onCloseOver != null)
                onCloseOver(null);
        }
        public override void Destroy(Action<Intent> onDestoryOver) { }
        #endregion

        private void DisplayDialog(string title, string message, string ok)
        {
            gameObject.SetActive(true);
            this.okCenterButton.SetActive(true);
            this.okButton.gameObject.SetActive(false);
            this.camcelButton.gameObject.SetActive(false);
            this.title.text = title;
            this.message.text = message;
            this.okCenter.text = ok;
            rectTransform.SetAsLastSibling();
        }
        private void DisplayDialog(string title, string message, string ok, string cancel)
        {
            gameObject.SetActive(true);
            this.okCenterButton.SetActive(false);
            this.ok.gameObject.SetActive(true);
            this.cancel.gameObject.SetActive(true);
            this.title.text = title;
            this.message.text = message;
            this.ok.text = ok;
            this.cancel.text = cancel;
            rectTransform.SetAsLastSibling();
        }
        public void PointClick(bool certain)
        {
            clickHandler(this,certain);
        }
    }
}