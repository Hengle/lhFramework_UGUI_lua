using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace LaoHan.UGUI
{
    public class UITool_ListItem : UIBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// 子项的索引
        /// </summary>
        public int itemIndex;
        /// <summary>
        /// 子项所需要更新的数据
        /// </summary>
        //public object itemData;
        /// <summary>
        /// 子项所属于的ListView
        /// </summary>
        public UITool_ListView listView;
        /// <summary>
        /// 子项是否已经被选中，通过点击事件来选的，其余的事件不起作用
        /// </summary>
        public bool isSelected;
        /// <summary>
        /// 子项的宽度
        /// </summary>
        public float width;
        /// <summary>
        /// 子项的高度
        /// </summary>
        public float height;
        /// <summary>
        /// 用来携带或保存数据
        /// </summary>
        public object param;
        public BaseEventData baseEventData;

        private Button btnItem;

        /// <summary>
        /// 获得子项中某个子节点上的组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="name">相对于子项的子节点层级</param>
        /// <returns></returns>
        public T GetComponentInItem<T>(string name) where T : Component
        {
            T t = default(T);
            Transform trans = transform.Find(name);
            if (trans)
                t = trans.GetComponent<T>();
            return t;
        }

        public Component GetComponentInItem(string name, Type t)
        {
            return transform.Find(name).GetComponent(t);
        }

        public static UITool_ListItem FindItemByChildObj(GameObject childObj)
        {
            UITool_ListItem item = childObj.transform.parent.GetComponent<UITool_ListItem>();
            if (!item)
                FindItemByChildObj(childObj);

            return item;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            baseEventData = eventData;
            listView.HandleClickItem(this);
            //Debug.Log(eventData.selectedObject.name);
            //Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        }
    }
}