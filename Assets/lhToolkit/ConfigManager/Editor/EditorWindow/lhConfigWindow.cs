using UnityEngine;
using UnityEditor;
using System.Collections;

namespace LaoHan.Tools.ConfigManager
{
    public class lhConfigWindow : EditorWindow
    {
        private Vector2 m_menuScrollPosition;
        private Vector2 m_contentScrollPosition;
        private lhConfigManager m_configManager;
        private static lhConfigWindow m_window;
        [MenuItem("lhTools/ConfigManager %u")]
        static void Init()
        {
            lhConfigWindow window = EditorWindow.GetWindow<lhConfigWindow>(true, "ConfigManager    (LaoHan)      QQ:369016334", true);
            m_window = window;
            window.Show();
        }
        void OnGUI()
        {
            if (m_configManager==null)
            {
                m_configManager = new lhConfigManager();
            }
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("+",GUILayout.Width(20)))
                        {

                        }
                        if (GUILayout.Button("♍",GUILayout.Width(20)))
                        {
                            
                        }
                    }EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    m_menuScrollPosition = EditorGUILayout.BeginScrollView(m_menuScrollPosition, GUILayout.Width(200));
                    {
                        m_configManager.DisplayMenu();
                    }EditorGUILayout.EndScrollView();
                }EditorGUILayout.EndVertical();
                
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        m_configManager.DisplayToolbar();
                    }EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    m_contentScrollPosition = EditorGUILayout.BeginScrollView(m_contentScrollPosition);
                    {
                        m_configManager.DisplayContent();
                    }EditorGUILayout.EndScrollView();
                }EditorGUILayout.EndVertical();
            }EditorGUILayout.EndHorizontal();

        }
        public static void Refresh()
        {
            m_window.Repaint();
        }
    }
}