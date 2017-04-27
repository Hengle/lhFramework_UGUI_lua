using UnityEditor;
using System.Collections;
using UnityEngine;
namespace LaoHan.Tools.DefineManager
{
    public class lhDefineWindow : EditorWindow
    {
        #region private member
        private Vector2 m_scrollPosition;
        private bool m_initialize;
        private lhDefineManager m_defineManager;
        #endregion

        #region Editor methods

        [MenuItem("lhTools/DefineWindow %h")]
        static void Initialize()
        {
            lhDefineWindow window = EditorWindow.GetWindow<lhDefineWindow>(true, "DefineManager    (LaoHan)      QQ:369016334", true);
            window.Show();
        }
        void OnDestroy()
        {
            if (m_defineManager!=null)
            {
                m_defineManager.Dispose();
            }
        }
        void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                GUILayout.Label("loading....");
                return;
            }
            if (!m_initialize)
            {
                m_defineManager = lhDefineManager.GetInstance();
                m_initialize = true;
                lhDefineManager.Apply();
            }
            EditorGUILayout.BeginVertical();
            {
                Rect item=EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                {
                    GUIContent createContent = new GUIContent("Create");
                    Rect createRect = GUILayoutUtility.GetRect(createContent, EditorStyles.toolbarButton);
                    if (GUI.Button(createRect, createContent, EditorStyles.toolbarButton))
                    {
                        lhDefineManager.CreateDefine();
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Apply", EditorStyles.toolbarButton))
                    {
                        if (EditorUtility.DisplayDialog("Warning", "Are you sure Apply?", "Ok", "Cancel"))
                            lhDefineManager.Apply();
                    }
                } EditorGUILayout.EndHorizontal();
                GUI.SetNextControlName("ScrollPosition");
                m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
                {
                    foreach(var define in lhDefineManager.defineList)
                    {
                        Rect itemRect=EditorGUILayout.BeginHorizontal();
                        {
                            define.active = GUILayout.Toggle(define.active,"");
                            if (define.renaming)
                            {
                                GUIContent cancelContent = new GUIContent("×");
                                if (GUI.Button(GUILayoutUtility.GetRect(cancelContent, EditorStyles.miniButton, GUILayout.ExpandWidth(false)), cancelContent))
                                {
                                    define.renaming = false;
                                    GUI.FocusControl("ScrollPosition");
                                }
                                GUIContent certainContent = new GUIContent("√");
                                if (GUI.Button(GUILayoutUtility.GetRect(certainContent, EditorStyles.miniButton, GUILayout.ExpandWidth(false)), certainContent))
                                {
                                    define.renaming = false;
                                    define.defineName = define.oldDefineName;
                                }
                                define.oldDefineName=EditorGUILayout.TextField(define.oldDefineName);
                            }
                            else
                            {
                                GUILayout.Label(define.defineName);
                            }
                            GUILayout.FlexibleSpace();
                            define.targetGroupValue = EditorGUILayout.MaskField(define.targetGroupValue, System.Enum.GetNames(typeof(lhDefineManager.DefineTargetGroup)), EditorStyles.layerMaskField);
                            //Debug.Log(define.targetGroupValue);
                            if (GUILayout.Button("X", EditorStyles.miniLabel))
                            {
                                if (EditorUtility.DisplayDialog("Warning", "Are you sure delete it", "Ok", "Cancel"))
                                {
                                    lhDefineManager.DeleteDefine(define);
                                    return;
                                }
                            }

                        } EditorGUILayout.EndHorizontal();

                        if (IsMouseOn(itemRect))
                        {
                            if(Event.current.keyCode==KeyCode.F2)
                            {
                                define.renaming = true;
                                define.oldDefineName = define.defineName;
                                Repaint();
                            }
                            if(Event.current.keyCode==KeyCode.Return)
                            {
                                define.renaming = false;
                                define.defineName = define.oldDefineName;
                                Repaint();
                            }
                        }
                    }
                } EditorGUILayout.EndScrollView();
            } EditorGUILayout.EndVertical();
        }
        #endregion

        #region private methods
        private bool IsMouseOn(Rect rect)
        {
            return rect.Contains(Event.current.mousePosition);
        }
        #endregion
    }
}
