using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LaoHan.Tools.WorldEditor
{
    [CustomEditor(typeof(lhTriggerSettingScriptable))]
    public class lhTriggerSettingEditor : lhInpectorBase<lhTriggerSettingScriptable>
    {
        private static Editor m_currentEditor;
        private static lhTriggerSettingScriptable m_inspector;
        string jsonPath = Application.dataPath + "/Resources/";
        void OnEnable()
        {
            m_currentEditor = this;
        }
        public static void Show()
        {
            if (m_inspector == null)
            {
                m_inspector = ScriptableObject.CreateInstance<lhTriggerSettingScriptable>();
                m_inspector.hideFlags = UnityEngine.HideFlags.DontSave;
            }
            m_inspector.name = "";
            Selection.activeObject = m_inspector;
        }
        public override bool UseDefaultMargins()
        {
            return false;
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("XmlPath:");
                EditorGUILayout.TextField(lhTriggerManager.xmlConfigPath);
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("browse"))
                    lhTriggerManager.xmlConfigPath = EditorUtility.OpenFolderPanel("XmlPath", lhTriggerManager.xmlConfigPath, "");
            } EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("JsonPath:");
                EditorGUILayout.TextField(jsonPath);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("browse"))
                    jsonPath = EditorUtility.OpenFolderPanel("JsonPath", jsonPath, "");
            } EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Configure"))
            {

            }
        }
    }
}