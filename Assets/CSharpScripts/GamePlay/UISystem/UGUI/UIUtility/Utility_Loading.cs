using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

using LaoHan.Infrastruture;
namespace LaoHan.UGUI
{
    public class Utility_Loading : lhUtilityBase
    {

        public RectTransform loadingImage;

        private IEnumerator m_enumRotate;

        private object m_value;

        #region lhUtilityBase
        public override void Initialize(Action onInitialOver)
        {
            onInitialOver();
            rectTransform.SetAsLastSibling();
        }
        public override void Open(Intent parameter,  Action<Intent> onOpenOver)
        {
            base.Open(parameter, onOpenOver);
            gameObject.SetActive(true);
            rectTransform.SetAsLastSibling();
            if (parameter!=null)
                m_value = parameter.GetExtras("value");
            if (onOpenOver!=null)
                onOpenOver(null);
            if (m_enumRotate == null)
            {
                m_enumRotate = ERotate();
                lhCoroutine.StartCoroutine(m_enumRotate);
            }
        }
        public override void Close(Action<Intent> onCloseOver)
        {
            gameObject.SetActive(false);
            lhCoroutine.StopCoroutine(m_enumRotate);
            m_enumRotate = null;
            if (onCloseOver != null)
                onCloseOver(null);
            base.Close(onCloseOver);
        }
        public override void Destroy(Action<Intent> onDestoryOver)
        {
            base.Destroy(onDestoryOver);
        }
        public override void ReceiveMessage(object mark, object value)
        {
        }
        public override object Transmit(object mark, object value)
        {
            if (value == null && m_value == null) return true;
            if (value == null && m_value != null) return false;
            if(value.Equals(-1))//force close
            {
                return true;
            }
            return value.Equals(m_value);
        }
        #endregion

        #region private methods

        #endregion

        #region public methods
        #endregion

        #region IEnumerator

        IEnumerator ERotate()
        {
            while (true)
            {
                loadingImage.localEulerAngles -= new Vector3(0, 0, 11f);
                yield return 1;
            }
        }
        #endregion
    }
}