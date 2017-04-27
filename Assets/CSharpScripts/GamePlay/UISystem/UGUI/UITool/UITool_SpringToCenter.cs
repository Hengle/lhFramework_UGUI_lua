using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

using LaoHan.Infrastruture;
namespace LaoHan.UGUI
{
    public class UITool_SpringToCenter : lhMonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        public ScrollRect scrollRect;
        public bool canDrag;
        public bool autoCenter;
        public bool horizontal;
        public bool vertical;
        public int divide;
        public Vector2 spacing;
        public float elasticity = 0.135f;

        public UnityAction<int> pointerChangedHandler;

        private Vector2[] m_dividePoint;
        private RectTransform m_content;
        private IEnumerator EMove;
        private int m_currentPoint;
        void Start()
        {
            Apply();
        }
        public void Apply()
        {
            m_content = scrollRect.content;
            if (autoCenter)
            {
                scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
                scrollRect.inertia = false;
            }
            m_dividePoint = new Vector2[divide];
            Vector2 interval = Vector2.zero;
            if (horizontal)
                interval = new Vector2(-m_content.rect.width / divide, 0);
            if (vertical)
                interval = new Vector2(interval.x, m_content.rect.height / divide);
            for (int i = 0; i < m_dividePoint.Length; i++)
            {
                if (i == 0)
                    m_dividePoint[i] = interval * i;
                else
                    m_dividePoint[i] = new Vector2(-spacing.x, spacing.y) + interval * i;
            }
        }
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            Vector2 current = m_content.anchoredPosition;
            Vector2 target = current;
            float min = Mathf.Infinity;
            for (int i = 0; i < m_dividePoint.Length; i++)
            {
                float distance = Vector2.Distance(current, m_dividePoint[i]);
                if (distance < min)
                {
                    min = distance;
                    target = m_dividePoint[i];
                    m_currentPoint = i;
                }
            }
            if (pointerChangedHandler != null)
                pointerChangedHandler(m_currentPoint);
            if (autoCenter)
            {
                if (EMove == null)
                {
                    EMove = EMoveToTarget(target);
                    lhCoroutine.StartCoroutine(EMove);
                }
            }
        }
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (EMove != null)
                lhCoroutine.StopCoroutine(EMove);
            EMove = null;
        }
        IEnumerator EMoveToTarget(Vector2 targetPosition)
        {
            Vector2 velocity = scrollRect.velocity;
            while (true)
            {
                m_content.anchoredPosition = Vector2.SmoothDamp(m_content.anchoredPosition, targetPosition, ref velocity, elasticity, Mathf.Infinity, Time.deltaTime);
                scrollRect.velocity = velocity;
                if (Vector2.SqrMagnitude(velocity) < 0.05f)
                {
                    EMove = null;
                    m_content.anchoredPosition = targetPosition;
                    scrollRect.velocity = Vector2.zero;
                    break;
                }
                yield return 1;
            }
        }

    }
}