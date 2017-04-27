using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace LaoHan.UGUI
{
    public class UITool_ListViewNormal : UITool_ListView
    {
        /// <summary>
        /// 是否只创建一次
        /// </summary>
        public bool isCreateOnce;

        private Vector3 originContentVec;
        private float curContentPos;
        private float preContentPos;

        public override void Start()
        {
            base.Start();


        }

        public override void Update()
        {
            //WrapContent();
        }

        protected void WrapContent()
        {
            curContentPos = ContentPosSingle;

            if (curContentPos - preContentPos >= CellSingle)
            {
                RectTransform rtItem1 = listItems[0].GetComponent<RectTransform>();
                RectTransform rtItem2 = listItems[listItems.Count - 1].GetComponent<RectTransform>();
                rtItem1.anchoredPosition = rtItem2.anchoredPosition - new Vector2(0, CellSingle);
                layoutGroupInner.rectTransform.sizeDelta += new Vector2(0, CellSingle);
                UITool_ListItem item = listItems[0];
                listItems.RemoveAt(0);
                listItems.Add(item);
                preContentPos = curContentPos;
            }
        }


        protected override void OnCreateItems(int itemCount)
        {
            SetGridMovement();

            originContentVec = layoutGroupInner.rectTransform.position;
            curContentPos = preContentPos = ContentPosSingle;

            ///CreateItems(Mathf.CeilToInt(ScrollRectSizeSingle / CellSingle) + 1);
            //Debug.Log(listItems[0].GetComponent<RectTransform>().localPosition);

            if (isCreateOnce)
            {
                if (GetListItemCount(false) <= 0)
                    CreateItems(itemCount);
                else
                    UpdateItems();
            }
            else
            {
                ClearItems();
                CreateItems(itemCount);
            }
        }

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="index">在指定的索引的地方添加</param>
        /// <param name="isAddFore">true--在前面添加， false--在后面添加</param>
        public override void AddItem(int index, bool isAddFore)
        {
            index = isAddFore ? index : (index + 1);
            CreateItemOne(index);
            AjustItems();
            
            CalculateContent();
            layoutGroupInner.UpdatePosition();
        }

        /// <summary>
        /// 删除子项
        /// </summary>
        /// <param name="index">要删除子项的索引</param>
        public override void DeleteItem(int index)
        {
            if (index < 0) return;
            UITool_ListItem item = FindItem(index);
            DestroyImmediate(item.gameObject);
            listItems.Remove(item);
            UpdateItemsIndex();
            //UpdateItemsUI();
            CalculateContent();
            layoutGroupInner.UpdatePosition();
        }
    }
}