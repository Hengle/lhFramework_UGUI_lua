using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LaoHan.Infrastruture;
namespace LaoHan.UGUI
{
    public class UITool_LayoutGroup : lhMonoBehaviour
    {
        public RectTransform ownRectTransform;
        public RectTransform LayoutParent;
        public GameObject layoutItem;
        public Vector2 margin;//Range(0,1)          x: horizontal,y:Vertical
        public Vector2 spacing;//Range(0,1)         x:horizontal,y:Vertical
        public Vector2 itemSize;//Range(0,1)        x:width,y:height
        public Vector2 layoutCount;//                x:columnsCount,y:rowsCount
        public List<GameObject> childList
        {
            get { return m_childList; }
        }

        private List<GameObject> m_childList;
        private bool m_pool;
        void OnDestroy()
        {
            Clear();
        }
        public void Create(bool pool = false)
        {
            m_pool = pool;
            if (layoutItem == null)
            {
                Infrastruture.lhDebug.LogError((object)"LaoHan: layoutItem is null");
                return;
            }
            if (LayoutParent == null)
            {
                Infrastruture.lhDebug.LogError((object)"LaoHan: layoutParent is null");
                return;
            }
            if (m_childList == null)
                m_childList = new List<GameObject>();
            float parentWidth = LayoutParent.rect.width;
            float parentHeight = LayoutParent.rect.height;
            ownRectTransform.anchoredPosition = Vector2.zero;
            int columns = (int)layoutCount.x;
            int rows = (int)layoutCount.y;
            float ownWidth = parentWidth * (margin.x * 2 + spacing.x * (columns - 1) + (itemSize.x * columns));
            float ownHeight = parentHeight * (margin.y * 2 + spacing.y * (rows - 1) + (itemSize.y * rows));
            ownRectTransform.sizeDelta = new Vector2(ownWidth, ownHeight);
            for (int i = 0; i < rows; i++)
            {
                float anchoredPositionY = -parentHeight * (margin.y + i * itemSize.y + spacing.y * i);
                for (int j = 0; j < columns; j++)
                {
                    RectTransform rectTransform = lhInstantiate(layoutItem, pool).GetComponent<RectTransform>();
                    m_childList.Add(rectTransform.gameObject);
                    rectTransform.SetParent(ownRectTransform);
                    float anchoredPositionX = parentWidth * (margin.x + j * itemSize.x + spacing.x * j);
                    rectTransform.anchoredPosition = new Vector2(anchoredPositionX, anchoredPositionY);
                    rectTransform.localScale = Vector3.one;
                    rectTransform.sizeDelta = new Vector2(parentWidth * itemSize.x, parentHeight * itemSize.y);
                }
            }
        }
        public T[] GetItem<T>() where T : Component
        {
            return ownRectTransform.GetComponentsInChildren<T>(true);
        }
        public void Clear(System.Action onClearOver = null, bool clearAll = false)
        {
            lhCoroutine.StartCoroutine(ClearChilds(onClearOver, clearAll));
        }
        IEnumerator ClearChilds(System.Action onClearOver, bool clearAll)
        {
            if (m_pool)
            {
                foreach (GameObject item in m_childList)
                {
                    lhFreeInstantiate(layoutItem, item);
                    Destroy(item);
                }
                m_childList = null;
            }
            if (ownRectTransform == null)
            {
                if (onClearOver != null)
                    onClearOver();
                yield return new lhWaitForReturn();
            }
            if (clearAll)
            {
                Transform[] objArr = ownRectTransform.GetComponentsInChildren<Transform>(true);
                for (int i = 0; i < objArr.Length; i++)
                {
                    if (objArr[i] == ownRectTransform) continue;
                    Destroy(objArr[i].gameObject);
                }
                Resources.UnloadUnusedAssets();
                yield return new lhWaitForSeconds(1);
                if (onClearOver != null)
                    onClearOver();
            }
            else
            {
                if (onClearOver != null)
                    onClearOver();
            }
        }
    }
}