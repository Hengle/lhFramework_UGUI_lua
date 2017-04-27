using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace LaoHan.Tools.AutoWrap
{
    [CustomEditor(typeof(lhInspectorExportWrap))]
    public class lhExportWrapEditor : Editor
    {
        private static Editor m_currentEditor;
        private static lhInspectorExportWrap m_inspector;
        private static Action<ExportResult> m_resultHanlder;

        private Vector2 m_scrollPosition;
        public enum ExportResult
        {
            Generate,
            ReGenerate,
            BuildDelegate,
            ReBuildDelegate,
            Drop,
            UnDrop,
            DeleteGenerate,
            DeleteDelegate,
        }
        void OnEnable()
        {
            m_currentEditor = this;
        }
        public static void Show(lhAutoWrap.BindType bindType,Action<ExportResult> onResult)
        {
            m_inspector = ScriptableObject.CreateInstance<lhInspectorExportWrap>();
            m_inspector.name = bindType.type.FullName;
            m_inspector.hideFlags = UnityEngine.HideFlags.DontSave;
            m_inspector.bindType = bindType;

            Selection.activeObject = m_inspector;
            m_resultHanlder = null;
            m_resultHanlder = onResult;
        }
        public override bool UseDefaultMargins()
        {
            return false;
        }
        public override void OnInspectorGUI()
        {
            if (m_inspector == null) return;
            var bindType = m_inspector.bindType;
            EditorGUILayout.LabelField("assemblyName:", bindType.assemblyName);
            EditorGUILayout.LabelField("nameSpace:", bindType.nameSpace);
            if (bindType.isDelegate)
            {
                if (bindType.isBuilded)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Reset Delegate"))
                        {
                            m_resultHanlder(ExportResult.ReBuildDelegate);
                        }
                        if (GUILayout.Button("Delete Delegate"))
                        {
                            m_resultHanlder(ExportResult.DeleteDelegate);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    if (GUILayout.Button("Delegate Build"))
                    {
                        m_resultHanlder(ExportResult.BuildDelegate);
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("className", bindType.className);
                bindType.baseClass=EditorGUILayout.TextField("baseClass:", bindType.baseClass);
                EditorGUILayout.LabelField("wrapName:", bindType.wrapName);
                EditorGUILayout.LabelField("libName:", bindType.libName);
                bindType.preload = EditorGUILayout.Toggle(new GUIContent("preload"), bindType.preload);
                if (bindType.isBuilded)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Reset Gen"))
                        {
                            m_resultHanlder(ExportResult.ReGenerate);
                        }
                        if (GUILayout.Button("Delete"))
                        {
                            m_resultHanlder(ExportResult.DeleteGenerate);
                        }
                    }EditorGUILayout.EndHorizontal();
                }
                else
                {
                    if (bindType.drop)
                    {
                        if (GUILayout.Button("UnDrop"))
                        {
                            m_resultHanlder(ExportResult.UnDrop);
                        }
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("Generate"))
                            {
                                m_resultHanlder(ExportResult.Generate);
                            }
                            if (GUILayout.Button("Drop"))
                            {
                                m_resultHanlder(ExportResult.Drop);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }
    }
}