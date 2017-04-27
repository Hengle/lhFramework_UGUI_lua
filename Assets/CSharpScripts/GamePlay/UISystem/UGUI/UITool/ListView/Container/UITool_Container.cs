using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LaoHan.Infrastruture;

namespace LaoHan.UGUI
{
    public abstract class UITool_Container : lhMonoBehaviour
    {
        public bool playOnStart;
        [HideInInspector]
        public Arrangement arrangement;
        [Tooltip("一共有多少列")]
        public int column = 1;
        [HideInInspector]
        public float cellWidth = 200f;
        [HideInInspector]
        public float cellHeight = 200f;
        [Tooltip("是否忽略屏幕适配")]
        public bool isIgnoreAdapter;

        protected virtual void Start()
        {
            if (playOnStart)
                UpdatePosition();
        }

        public abstract void UpdatePosition();
    }
}