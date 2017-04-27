using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

namespace LaoHan.UGUI
{
    public class UITool_ListView : UITool_View
    {
        public GameObject itemPrefab;

        protected List<UITool_ListItem> listItems = new List<UITool_ListItem>();
        //protected List<object> dataSource;
        protected ScrollRect scrollRect;
        protected Mask mask;
        protected LayoutGroupInner layoutGroupInner;
        /// <summary>
        /// 根据UIPanel区域计算出来要创建的子项的个数
        /// </summary>
        protected int createItemCount;
        /// <summary>
        /// 被选中的子项的索引
        /// </summary>
        protected int selectIndex;

        //private IBaseAdapter adapter;
        private int maxCount = -1;
        private int defaultSelectdItemIndex = -1;
        //记录content的初始位置，当创建的时候，用来对content的初始化
        private Vector2 contentOriginPos;

        private Action<UITool_ListItem> onClickItem;
        private Action<UITool_ListItem> onCreateItem;
        private Action<UITool_ListItem> onUpdateItem;

        public override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// 获得子项的个数
        /// </summary>
        public int ItemCount
        {
            get { return listItems.Count; }
        }

        /// <summary>
        /// 获得数据源对象
        /// </summary>
        //public List<object> DataSource
        //{
        //    get { return dataSource; }
        //}

        /// <summary>
        /// 获得子项父节点的坐标（根据排布方向不同，返回坐标点的x或y）
        /// </summary>
        protected float ContentPosSingle
        {
            get
            {
                float pos = 0;
                if (Direction == Arrangement.Horizontal)
                    pos = layoutGroupInner.rectTransform.position.x;
                else if (Direction == Arrangement.Vertical)
                    pos = layoutGroupInner.rectTransform.position.y;

                return pos;
            }
        }

        /// <summary>
        /// 获得ListView的排布方向 --只读
        /// </summary>
        public Arrangement Direction
        {
            get
            {
                if (scrollRect.vertical)
                    return Arrangement.Vertical;
                else if (scrollRect.horizontal)
                    return Arrangement.Horizontal;
                else
                    return Arrangement.Vertical;
            }
        }

        /// <summary>
        /// 获得ScrollRect的大小（根据排布方向不同，返回宽度或高度）
        /// </summary>
        protected float ScrollRectSizeSingle
        {
            get
            {
                float size = 0;
                if (Direction == Arrangement.Horizontal)
                    size = scrollRect.GetComponent<RectTransform>().sizeDelta.x;
                else if (Direction == Arrangement.Vertical)
                    size = scrollRect.GetComponent<RectTransform>().sizeDelta.y;

                return size;
            }
        }

        /// <summary>
        /// 获得子项的大小（根据排布方向不同，返回子项宽度或高度）
        /// </summary>
        protected float CellSingle
        {
            get
            {
                float cell = 0;
                if (Direction == Arrangement.Horizontal)
                    cell = layoutGroupInner.CellWidth;
                else if (Direction == Arrangement.Vertical)
                    cell = layoutGroupInner.CellHeight;

                return cell;
            }
        }

        /// <summary>
        /// 设置子项的点击事件
        /// </summary>
        public Action<UITool_ListItem> OnClickItem
        {
            set { onClickItem = value; }
        }

        /// <summary>
        /// 最大可以创建多少个子项，如果为-1(是默认值)，子项的个数就等于数据源的个数;如果大于0，子项的个数就等于maxCount的个数，并且大于数据源个数的子项
        /// 上的脚本UITool_ListItem中的itemData为null
        /// </summary>
        public int MaxCount
        {
            get { return maxCount; }
            set { maxCount = value; }
        }

        /// <summary>
        /// 默认选中的子项的索引，即创建完成后会自动选中的子项，-1(默认值)表示没有默认选中的项，大于等于0表示有默认选中的项
        /// </summary>
        public int DefaultSelectedItemIndex
        {
            set { defaultSelectdItemIndex = value; }
        }

        /// <summary>
        /// 设置选中的子项的索引，内部会自动更新选中子项的界面表现
        /// </summary>
        public int SelectedItemIndex
        {
            set
            {
                if (ItemCount > 0)
                {
                    selectIndex = value;
                    FindItem(selectIndex).OnPointerClick(new PointerEventData(EventSystem.current));
                }
            }
        }

        /// <summary>
        /// 获得在ScrollRect的范围内可以显示子项的个数，如果ScrollRect的范围内的子项个数大于数据源的个数，那就返回数据源的个数，否则就返回ScrollRect的范围内的子项个数
        /// </summary>
        public int ItemCountInRect
        {
            get
            {
                int originCount = Mathf.CeilToInt(ScrollRectSizeSingle / CellSingle) * layoutGroupInner.Column;
                int actualCount = 0;
                if (ItemCount < originCount)
                    actualCount = ItemCount;
                else
                    actualCount = originCount;
                return actualCount;
            }
        }

        public virtual void AddItem(int index, bool isAddFore) { }

        public virtual void DeleteItem(int index) { }

        /// <summary>
        /// 重新计算子项的位置
        /// </summary>
        public void ResetPosition()
        {
            layoutGroupInner.UpdatePosition();
        }

        /// <summary>
        /// 更新所有的子项界面显示(但是不更新数据源)
        /// </summary>
        public void UpdateItems()
        {
            UITool_ListItem maxItem = GetMaxIndexItem(false);
            //当，没有子项的时候，就找不到了
            if (!maxItem) return;

            for (int i = 0; i <= maxItem.itemIndex; i++)
            {
                UpdateItem(i);
            }
        }

        /// <summary>
        /// 更新指定索引子项的界面显示(但是不更新数据源)
        /// </summary>
        /// <param name="index">子项的索引</param>
        private void UpdateItem(int index)
        {
            UITool_ListItem item = FindItem(index);
            //adapter.OnUpdateListViewItem(item);
            if (onUpdateItem != null)
                onUpdateItem(item);
        }

        /// <summary>
        /// 更新所有子项的索引，使得子项的索引重新连续排列(例如，如果删除子项时，使得索引不连续，可以调用此方法更新下)
        /// </summary>
        public void UpdateItemsIndex()
        {
            UITool_ListItem maxItem = GetMaxIndexItem(false);
            if (!maxItem) return;

            for (int i = 0; i < maxItem.itemIndex; i++)
            {
                UITool_ListItem item = listItems[i];
                item.itemIndex = i;
                item.name = i.ToString();
            }
        }

        /// <summary>
        /// 1.更新子项的索引，从0开始一直到listPanelItems.Count-1
        /// 2.调整子项，使listItems结合中的顺序和Inspector中的顺序一致
        /// </summary>
        protected void AjustItems()
        {
            for (int i = 0; i < listItems.Count; i++)
            {
                listItems[i].itemIndex = i;
                //使其按照数据集合中的顺序来放
                listItems[i].transform.SetSiblingIndex(i);
            }
        }

        /// <summary>
        /// 通过索引来找子项
        /// </summary>
        /// <param name="index">子项的索引</param>
        public UITool_ListItem FindItem(int index)
        {
            UITool_ListItem item = null;
            for (int i = 0; i < GetListItemCount(true); i++)
            {
                if (listItems[i].itemIndex == index)
                {
                    item = listItems[i];
                    break;
                }
            }
            return item;
        }

        /// <summary>
        /// 设置被选中子项的状态，即ListPanelItem中的isSelected字段
        /// </summary>
        /// <param name="item">当前被选中的子项</param>
        private void UpdateSelectItemState(UITool_ListItem item)
        {
            if (item)
            {
                for (int i = 0; i < GetListItemCount(true); i++)
                {
                    listItems[i].isSelected = false;
                }

                item.isSelected = true;
                selectIndex = item.itemIndex;
                UpdateItems();
            }
        }

        /// <summary>
        /// 获得被选中的子项，如果没有选中的，就返回null
        /// </summary>
        public UITool_ListItem GetSelectedItem()
        {
            UITool_ListItem item = null;

            for (int i = 0; i < GetListItemCount(true); i++)
            {
                if (listItems[i].isSelected)
                {
                    item = listItems[i];
                    break;
                }
            }

            return item;
        }

        /// <summary>
        /// 清除所有的子项
        /// </summary>
        protected void ClearItems()
        {
            for (int i = 0; i < GetListItemCount(true); i++)
            {
                /*
                 * 要在这帧立即销毁，否则第二次在打开面板时会错位
                 */
                DestroyImmediate(listItems[i].gameObject);
            }

            listItems.Clear();
        }

        public void HandleClickItem(UITool_ListItem listItem)
        {
            UpdateSelectItemState(listItem);

            if (onClickItem != null)
                onClickItem(listItem);
        }

        /// <summary>
        /// 获得子项的个数
        /// </summary>
        /// <param name="isIncludeNotActive">是否要包含不激活的子项</param>
        protected int GetListItemCount(bool isIncludeNotActive)
        {
            int count = 0;

            if (isIncludeNotActive)
                count = listItems.Count;
            else
                for (int i = 0; i < listItems.Count; i++)
                {
                    if (listItems[i].gameObject.activeSelf)
                        count++;
                }
            return count;
        }

        /// <summary>
        /// 清楚数据源
        /// </summary>
        //protected void ClearDataSource()
        //{
        //    if (this.dataSource.Count > 0)
        //        this.dataSource.Clear();
        //}

        /// <summary>
        /// 清除所有的子项
        /// </summary>
        public void Clear()
        {
            //ClearDataSource();
            ClearItems();
        }

        /// <summary>
        /// 计算包裹子项(即子项父类)的大小和位置
        /// </summary>
        public void CalculateContent()
        {
            //RectTransform tran = scrollRect.GetComponent<RectTransform>();
            RectTransform gridTrans = layoutGroupInner.rectTransform;

            //当子项都创建完成后，调整布局的大小，使其包含全部的子项
            switch (Direction)
            {
                case Arrangement.Horizontal:
                    gridTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, layoutGroupInner.CellWidth * layoutGroupInner.Row);
                    gridTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, gridTrans.rect.height);
                    //gridTrans.sizeDelta = new Vector2(layoutGroupInner.CellWidth * layoutGroupInner.Row, layoutGroupInner.CellHeight * layoutGroupInner.Column);
                    //tran.sizeDelta = new Vector2(gridTrans.sizeDelta.x, tran.sizeDelta.y);
                    break;
                case Arrangement.Vertical:
                    gridTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, gridTrans.rect.width);
                    gridTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, layoutGroupInner.CellHeight * layoutGroupInner.Row);
                    //gridTrans.sizeDelta = new Vector2(layoutGroupInner.CellWidth * layoutGroupInner.Column, layoutGroupInner.CellHeight * layoutGroupInner.Row);
                    //tran.sizeDelta = new Vector2(tran.sizeDelta.x, gridTrans.sizeDelta.y);
                    break;
            }


        }

        /// <summary>
        /// 设置所有子项的激活状态
        /// </summary>
        /// <param name="isActive">是否激活， true--激活  false--不激活</param>
        public void SetAllActive(bool isActive)
        {
            for (int i = 0; i < listItems.Count; i++)
            {
                listItems[i].gameObject.SetActive(isActive);
            }
        }

        /// <summary>
        /// 添加数据源并创建子项
        /// </summary>
        /// <param name="dataSource">数据源，根据数据源的个数创建子项的个数</param>
        /// <param name="onCreateItem">每创建一个子项都会调用此回调</param>
        /// <param name="onUpdateItem">每个子项更新界面数据时，会调用此回调</param>
        /// <param name="onClickItem">子项的点击事件</param>
        public void AddDSWithCreateItems(int itemCount, Action<UITool_ListItem> onCreateItem, Action<UITool_ListItem> onUpdateItem, Action<UITool_ListItem> onClickItem = null)
        {
            //if (this.dataSource == null)
            //    this.dataSource = new List<object>();

            if (!scrollRect)
                scrollRect = GetComponent<ScrollRect>();

            if (!mask)
                mask = GetComponent<Mask>();

            if (layoutGroupInner == null)
            {
                layoutGroupInner = new LayoutGroupInner(gameObject);
                contentOriginPos = layoutGroupInner.rectTransform.anchoredPosition;
            }

            this.onCreateItem = onCreateItem;
            this.onUpdateItem = onUpdateItem;

            //ClearDataSource();
            ClearItems();

            //填充数据源
            //if (maxCount == -1 && dataSource != null)
            //{
            //    for (int i = 0; i < dataSource.Count; i++)
            //    {
            //        this.dataSource.Add(dataSource[i]);
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < maxCount; i++)
            //    {
            //        if (dataSource != null && i < dataSource.Count)
            //            this.dataSource.Add(dataSource[i]);
            //        else
            //            this.dataSource.Add(default(T));
            //    }
            //}

            //创建子项
            OnCreateItems(itemCount);

            //添加点击事件
            //SetItemClickEvent(onClickItem);
            OnClickItem = onClickItem;

            //默认选中第一个子项
            if (defaultSelectdItemIndex > -1)
                SelectedItemIndex = defaultSelectdItemIndex;
        }

        /// <summary>
        /// 创建子项
        /// </summary>
        protected virtual void OnCreateItems(int itemCount) { }

        /// <summary>
        /// 创建指定个数的子项
        /// </summary>
        /// <param name="count">要创建的子项的个数</param>
        protected void CreateItems(int count)
        {
            //每次重新开始创建所有子项的时候，就初始化content的位置，否则位置会错乱
            layoutGroupInner.rectTransform.anchoredPosition = contentOriginPos;

            Transform trParent = itemPrefab.transform.parent;        
            //获得itemPrefab在Content下的位置
            itemPrefab.transform.SetParent(layoutGroupInner.rectTransform);
            //itemPrefab.transform.localScale = Vector3.one;

            for (int i = 0; i < count; i++)
            {
                CreateItemOne(i);
            }

            itemPrefab.transform.SetParent(trParent);

            CalculateContent();
            //for (int i = 0; i < listItems.Count; i++)
            //{
            //    listItems[i].transform.SetParent(layoutGroupInner.rectTransform);
            //}

            layoutGroupInner.UpdatePosition();
        }

        /// <summary>
        /// 创建一个子项
        /// </summary>
        ///<param name="index">子项索引</param>
        protected UITool_ListItem CreateItemOne(int index)
        {
            GameObject itemCopy = lhInstantiate(itemPrefab) as GameObject;
            itemCopy.SetActive(true);
            UITool_ListItem listItem = itemCopy.AddComponent<UITool_ListItem>();

            itemCopy.transform.SetParent(layoutGroupInner.rectTransform);
            RectTransform rtItemCopy = itemCopy.GetComponent<RectTransform>();
            RectTransform rtItemPrefab = itemPrefab.GetComponent<RectTransform>();
            if (!layoutGroupInner.IsIgnoreAdapter)
            {
                //将锚点设置成左上角对齐，因为这样可以当设置content大小时，不会是子项跟着拉伸，然后会在UITool_Grid中会重置锚点，重新排列
                rtItemCopy.anchorMin = new Vector2(0, 1);
                rtItemCopy.anchorMax = new Vector2(0, 1);
            }
            rtItemCopy.anchoredPosition = rtItemPrefab.anchoredPosition;
            rtItemCopy.sizeDelta = new Vector2(rtItemPrefab.rect.width, rtItemPrefab.rect.height); //rtItemPrefab.sizeDelta;
            itemCopy.transform.localScale = Vector3.one;

            listItem.width = layoutGroupInner.CellWidth;
            listItem.height = layoutGroupInner.CellHeight;
            listItem.listView = this;

            listItem.itemIndex = index;
            listItem.gameObject.name = listItem.itemIndex.ToString();
            //listItem.itemData = dataSource[listItem.itemIndex];
            listItems.Insert(listItem.itemIndex, listItem);

            //adapter.OnCreateListViewItem(listItem);
            //adapter.OnUpdateListViewItem(listItem);
            if (onCreateItem != null)
                onCreateItem(listItem);
            if (onUpdateItem != null)
                onUpdateItem(listItem);

            return listItem;
        }

        /// <summary>
        /// 根据ScrollRect的方向设置Container的方向
        /// </summary>
        protected void SetGridMovement()
        {
            if (scrollRect.vertical)
                layoutGroupInner.Direction = Arrangement.Vertical;
            else if (scrollRect.horizontal)
                layoutGroupInner.Direction = Arrangement.Horizontal;
        }

        /// <summary>
        /// 获得子项列表中索引最大的子项
        /// </summary>
        /// <param name="isIncludeNotActive">是否包含不激活的子项</param>
        protected UITool_ListItem GetMaxIndexItem(bool isIncludeNotActive)
        {
            UITool_ListItem item = null;
            int max = -1;
            int count = GetListItemCount(true);
            for (int i = 0; i < count; i++)
            {
                if (listItems[i].gameObject.activeSelf)
                {
                    item = listItems[i];
                    max = item.itemIndex;
                    break;
                }
            }

            for (int i = 0; i < count; i++)
            {
                //去除未激活的物体
                if (!isIncludeNotActive && !listItems[i].gameObject.activeSelf)
                    continue;

                if (listItems[i].itemIndex > max)
                {
                    max = listItems[i].itemIndex;
                    item = listItems[i];
                }
            }
            return item;
        }

        /// <summary>
        /// 获得子项列表中索引最小的子项
        /// </summary>
        /// <param name="isIncludeNotActive">是否包含不激活的子项</param>
        /// <returns></returns>
        protected UITool_ListItem GetMinIndexItem(bool isIncludeNotActive)
        {
            UITool_ListItem item = null;
            int min = -1;
            int count = GetListItemCount(true);
            for (int i = 0; i < count; i++)
            {
                if (listItems[i].gameObject.activeSelf)
                {
                    item = listItems[i];
                    min = item.itemIndex;
                    break;
                }
            }

            for (int i = 1; i < count; i++)
            {
                //去除未激活的物体
                if (!isIncludeNotActive && !listItems[i].gameObject.activeSelf)
                    continue;

                if (listItems[i].itemIndex < min)
                {
                    min = listItems[i].itemIndex;
                    item = listItems[i];
                }
            }
            return item;
        }

        /// <summary>
        /// 将所有的布局进行统一调用
        /// </summary>
        public class LayoutGroupInner
        {
            private float cellWidth;
            private float cellHeight;
            private int column;
            private Arrangement direction;
            private GameObject go;
            private RectTransform rectTrans;
            private UITool_Container container;

            public LayoutGroupInner(GameObject go)
            {
                this.go = go;
                container = go.GetComponentInChildren<UITool_Container>();
                rectTrans = container.GetComponentInChildren<RectTransform>();
            }

            public float CellWidth
            {
                get
                {
                    if (rectTrans.childCount > 0)
                        return rectTrans.GetChild(0).GetComponent<RectTransform>().rect.width;
                    else
                        return 0;
                }
                //set
                //{
                //    cellWidth = value;
                //    container.cellWidth = cellWidth;
                //}
            }

            public float CellHeight
            {
                get
                {
                    if (rectTrans.childCount > 0)
                        return rectTrans.GetChild(0).GetComponent<RectTransform>().rect.height;
                    else
                        return 0;
                }
                //set
                //{
                //    cellHeight = value;
                //    container.cellHeight = cellHeight;
                //}
            }

            public int Column
            {
                get { return container.column; }
                set
                {
                    column = value;
                    container.column = column;
                }
            }

            public int Row
            {
                get { return Mathf.CeilToInt(rectTrans.childCount * 1.0f / Column); }
            }

            public bool IsIgnoreAdapter
            {
                get { return container.isIgnoreAdapter; }
            }

            public Arrangement Direction
            {
                get { return container.arrangement; }
                set
                {
                    direction = value;
                    container.arrangement = direction;
                }
            }

            public RectTransform rectTransform
            {
                get { return rectTrans; }
            }

            public void UpdatePosition()
            {
                container.UpdatePosition();
            }
        }
    }

    public enum Arrangement
    {
        Horizontal,
        Vertical
    }
}