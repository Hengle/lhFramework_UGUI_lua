using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LaoHan.UGUI
{
    public class UITool_Grid : UITool_Container
    {
        private Vector2 originItemPos;
        private Vector2 originItemSize;
        private Rect originItemRect;



        public override void UpdatePosition()
        {
            int totalCount = transform.childCount;
            int row = Mathf.CeilToInt(totalCount * 1.0f / column);

            if (transform.childCount > 0)
            {
                RectTransform rtFirstChild = transform.GetChild(0).GetComponent<RectTransform>();
                originItemPos = rtFirstChild.anchoredPosition;
                originItemSize = rtFirstChild.sizeDelta;
                originItemRect = rtFirstChild.rect;
            }
            RectTransform rtGrid = GetComponent<RectTransform>();
            //Debug.Log(rtGrid.rect);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    //Debug.Log((i * column + j) + "," + i + " " + column + " " + j);
                    int index = i * column + j;
                    //防止除以column除不尽的情况
                    if (index < totalCount)
                    {
                        Transform child = transform.GetChild(index);
                        RectTransform rtChild = child.GetComponent<RectTransform>();
                        float ratioWidth = rtChild.rect.width / rtGrid.rect.width;
                        float ratioHeight = rtChild.rect.height / rtGrid.rect.height;

                        if (arrangement == Arrangement.Vertical)
                        {
                            if (isIgnoreAdapter)
                            {
                                rtChild.anchoredPosition = originItemPos + new Vector2(rtChild.rect.width * j, -rtChild.rect.height * i);
                            }
                            else
                            {
                                rtChild.anchorMin = new Vector2(ratioWidth * j, 1 - ratioHeight * (i + 1));
                                rtChild.anchorMax = new Vector2(ratioWidth * (j + 1), 1 - ratioHeight * i);
                                rtChild.anchoredPosition = Vector3.zero;
                                rtChild.sizeDelta = Vector3.zero;
                            }
                        }
                        else if (arrangement == Arrangement.Horizontal)
                        {
                            if (isIgnoreAdapter)
                            {
                                rtChild.anchoredPosition = originItemPos + new Vector2(rtChild.rect.width * i, -rtChild.rect.height * j);
                            }
                            else
                            {
                                rtChild.anchorMin = new Vector2(ratioWidth * i, 1 - ratioHeight * (j + 1));
                                rtChild.anchorMax = new Vector2(ratioWidth * (i + 1), 1 - ratioHeight * j);
                                rtChild.anchoredPosition = Vector3.zero;
                                rtChild.sizeDelta = Vector3.zero;
                            }
                        }
                    }
                }
            }
        }
    }
}
