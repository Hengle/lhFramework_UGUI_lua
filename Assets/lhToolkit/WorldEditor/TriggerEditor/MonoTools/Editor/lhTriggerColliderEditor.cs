using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEditor;

namespace LaoHan.Tools.WorldEditor
{
    [CustomEditor(typeof(lhTriggerCollider))]
    public class lhTriggerColliderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            lhTriggerCollider trigger = (lhTriggerCollider)target;
            if (trigger.triggerType==lhTriggerCollider.ETriggerType.Stay)
            {
                EditorGUILayout.Separator();
                trigger.interval = EditorGUILayout.FloatField("stay interval", trigger.interval);
            }
        }
        void OnSceneGUI()
        {

            lhTriggerCollider trigger = (lhTriggerCollider)target;

            var style = new GUIStyle();
            style.fontSize = 22;
            style.normal.textColor = Color.red;
            Handles.Label(trigger.transform.position,
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