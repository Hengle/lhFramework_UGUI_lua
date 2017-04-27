using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace LaoHan.Tools.BundleBuilder
{
    [CustomEditor(typeof(lhInspectorSettingObject))]
    class lhSettingEditor:Editor
    {
        private static lhInspectorSettingObject m_inspectorSetting=null;

        public static void Show()
        {
            if(m_inspectorSetting==null)
            {
                m_inspectorSetting = ScriptableObject.CreateInstance<lhInspectorSettingObject>();
                m_inspectorSetting.hideFlags = HideFlags.DontSave;
                m_inspectorSetting.name = "BundleManager Setting";
            }
            Selection.activeObject = m_inspectorSetting;
        }
        public override bool UseDefaultMargins()
        {
            return false;
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("ApplicationPath:");
                    lhBundleBuilder.buildParameter.applicationPath =(lhBundleBuilder.ApplicationPath) EditorGUILayout.EnumPopup(lhBundleBuilder.buildParameter.applicationPath);
                } GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("OutputRootFolder:");
                    lhBundleBuilder.buildParameter.outputRootFolder = EditorGUILayout.TextField(lhBundleBuilder.buildParameter.outputRootFolder);
                } GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Stuff:");
                    lhBundleBuilder.buildParameter.stuff = EditorGUILayout.TextField(lhBundleBuilder.buildParameter.stuff);
                } GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("crc:");
                    lhBundleBuilder.buildParameter.crc = (uint)EditorGUILayout.IntField((int)lhBundleBuilder.buildParameter.crc);
                } GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("buildAssetBundleOptions:");
                    lhBundleBuilder.buildParameter.buildAssetBundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup(lhBundleBuilder.buildParameter.buildAssetBundleOptions);
                } GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("buildOptions:");
                    lhBundleBuilder.buildParameter.buildOptions = (BuildOptions)EditorGUILayout.EnumPopup(lhBundleBuilder.buildParameter.buildOptions);
                } GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("buildTarget:");
                    lhBundleBuilder.buildParameter.buildTarget = (BuildTarget)EditorGUILayout.EnumPopup(lhBundleBuilder.buildParameter.buildTarget);
                } GUILayout.EndHorizontal();
            } EditorGUILayout.EndVertical();
        }
    }
}
