/*
 以纵向高度为基准算出对应的cell尺寸和间隔尺寸，
 * 即cell尺寸等于纵向高度乘以cellsizeYRatio，spacing尺寸等于纵向高度乘以spacingRatio,
 * 横向cell.y尺寸由cell.x  cell.y的比例算出
 * 高度是有layout的父节点决定，因为父节点由自适应，content 没有自适应
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using LaoHan.Infrastruture;
namespace LaoHan.UGUI
{
    public class UITool_GridLayoutGroupAdaptive : lhMonoBehaviour
    {
        public GridLayoutGroup layoutGroup;
        public bool playOnStart = true;
        public float spacingRatio;// range 0-1   spacing/height     间隔是横向与纵向相同
        public float cellsizeYRatio;// range 0-1
        void Start()
        {
            if (playOnStart)
                ResetAdaptive();
        }
        public void ResetAdaptive()
        {
            RectTransform rectTransform = layoutGroup.GetComponent<RectTransform>();
            if (rectTransform.parent == null)
            {
                Debug.LogWarning("LaoHan: parent is null ,adaptive is invalid");
                return;
            }
            RectTransform parent = rectTransform.parent.GetComponent<RectTransform>();
            float cell_y = parent.rect.height * cellsizeYRatio;
            float cellRatio = layoutGroup.cellSize.x / layoutGroup.cellSize.y;
            float cell_x = cell_y * cellRatio;
            layoutGroup.cellSize = new Vector2(cell_x, cell_y);
            float spacing = parent.rect.height * spacingRatio;
            layoutGroup.spacing = Vector2.one * spacing;
            int childCount = rectTransform.childCount;
            float width = 0;
            float height = 0;
            if (layoutGroup.startAxis == GridLayoutGroup.Axis.Horizontal)
            {
                width = parent.rect.width; ;
                //宽度等于 cell_x* count+spacing_x*(count-1)   
                //得出的count为小数，直接去掉小数位即为个数
                int rowCount = Mathf.FloorToInt((width - spacing) / (cell_x + spacing));
                int columnCount = childCount / rowCount + 1;
                height = columnCount * cell_y + spacing * (columnCount - 1);
            }
            else
            {
                height = parent.rect.height;
                int columnCount = Mathf.FloorToInt((height - spacing) / (cell_y + spacing));
                int rowCount = childCount / columnCount + 1;
                width = rowCount * cell_x + spacing * (rowCount - 1);
            }
            rectTransform.sizeDelta = new Vector2(width, height);
        }
    }
}