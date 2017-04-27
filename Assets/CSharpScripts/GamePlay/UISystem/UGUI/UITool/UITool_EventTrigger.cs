using UnityEngine;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using LaoHan.Infrastruture;
namespace LaoHan.UGUI
{
    public class UITool_EventTrigger : lhMonoBehaviour,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerUpHandler,
        IDragHandler,
        IDropHandler
    {
        public UnityAction<PointerEventData> pointerClickHandler;
        public UnityAction<PointerEventData> pointerDownHandler;
        public UnityAction<PointerEventData> pointerEnterHandler;
        public UnityAction<PointerEventData> pointerExitHandler;
        public UnityAction<PointerEventData> pointerUpHandler;
        public UnityAction<PointerEventData> dragHandler;
        public UnityAction<PointerEventData> dropHandler;

        void OnDestroy()
        {
            pointerClickHandler = null;
            pointerDownHandler = null;
            pointerEnterHandler = null;
            pointerExitHandler = null;
            pointerUpHandler = null;
            dragHandler = null;
            dropHandler = null;
        }
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (pointerClickHandler != null)
                pointerClickHandler(eventData);
        }
        public virtual void OnPointerDown(PointerEventData eventData)
        {

            if (pointerDownHandler != null)
                pointerDownHandler(eventData);
        }
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (pointerEnterHandler != null)
                pointerEnterHandler(eventData);

        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (pointerExitHandler != null)
                pointerExitHandler(eventData);
        }
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (pointerUpHandler != null)
                pointerUpHandler(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragHandler != null)
                dragHandler(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (dropHandler != null)
                dropHandler(eventData);
        }
    }
}
