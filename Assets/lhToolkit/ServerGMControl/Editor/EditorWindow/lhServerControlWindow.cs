using UnityEngine;
using UnityEditor;
using System.Collections;

namespace LaoHan.Tools.ServerControl
{
    public class lhServerControlWindow : EditorWindow
    {
        private lhServerControlManager m_serverControl;
        private Vector2 m_scrollPosition;
        [MenuItem("lhTools/Server Contrl %j")]
        static void Init()
        {
            lhServerControlWindow window = EditorWindow.GetWindow<lhServerControlWindow>(true, "Server Contrl    (LaoHan)      QQ:369016334", true);
            window.Show();
        }
        void Update()
        {
            if (m_serverControl!=null)
                m_serverControl.Update();
        }
        void OnDestroy()
        {
            m_serverControl.Dispose();
            m_serverControl = null;
        }
        void OnGUI()
        {
            if (m_serverControl==null)
            {
                m_serverControl = lhServerControlManager.GetInstance();
            }
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                {
                    if (GUILayout.Button("Create", EditorStyles.toolbarButton))
                    {
                        m_serverControl.Create();
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Save", EditorStyles.toolbarButton))
                    {
                        m_serverControl.Save();
                    }
                } EditorGUILayout.EndHorizontal();
                m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
                {
                    m_serverControl.ShowServerData();

                } EditorGUILayout.EndScrollView();

            } EditorGUILayout.EndVertical();
        }
    }
}