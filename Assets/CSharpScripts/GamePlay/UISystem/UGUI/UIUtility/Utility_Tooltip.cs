using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

using LaoHan.Infrastruture;
namespace LaoHan.UGUI
{
    public class Utility_Tooltip : lhUtilityBase
    {
        #region public member
        public Text title;
        public Text message;
        #endregion

        #region lhUtilityBase
        public override void Initialize(Action onInitialOver)
        {
            onInitialOver();
            rectTransform.SetAsLastSibling();
        }
        public override void Open(Intent parameter, Action<Intent> onOpenOver)
        {
            string title = (string)parameter.GetExtras("Title");
            string message = (string)parameter.GetExtras("Message");
            if (parameter.HasExtras("Delay"))
                DisplayTooltip(title, message, (float)parameter.GetExtras("Delay"));
            else
                DisplayTooltip(title, message);
            gameObject.SetActive(true);
            base.Open(parameter, onOpenOver);
            onOpenOver(null);
        }
        public override void Close(Action<Intent> onCloseOver)
        {
            gameObject.SetActive(false);
            if (onCloseOver != null)
                onCloseOver(null);
        }
        public override void Destroy(Action<Intent> onDestoryOver)
        {
            base.Destroy(onDestoryOver);
        }
        #endregion
        public void DisplayTooltip(string title, string message, float delay)
        {
            this.title.text = title;
            this.message.text = message;
            rectTransform.SetAsLastSibling();
            lhCoroutine.StartCoroutine(EWaitForSeconds(delay, () => { gameObject.SetActive(false); }));
        }
        public void DisplayTooltip(string title, string message)
        {
            this.title.text = title;
            this.message.text = message;
        }
        public void PointClick()
        {
            gameObject.SetActive(false);
        }

    }
}