using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEditor;

namespace LaoHan.Tools.WorldEditor
{
    [CustomEditor(typeof(lhTriggerAnimator))]
    public class lhTriggerAnimatorEditor : Editor
    {
        void OnSceneGUI()
        {

            lhTriggerAnimator trigger = (lhTriggerAnimator)target;

            var style=new GUIStyle();
            style.fontSize=22;
            style.normal.textColor = Color.red;
            Handles.Label(trigger.transform.position ,
                        trigger.id, style);

            //Handles.BeginGUI();

            ////规定GUI显示区域
            //GUILayout.BeginArea(new Rect(100, 100, 100, 100));

            ////GUI绘制一个按钮
            //if (GUILayout.Button("这是一个按钮!"))
            //{
            //    Debug.Log("test");
            //}
            ////GUI绘制文本框
            //GUILayout.Label("我在编辑Scene视图");

            //GUILayout.EndArea();

            //Handles.EndGUI();
        }
    }
}