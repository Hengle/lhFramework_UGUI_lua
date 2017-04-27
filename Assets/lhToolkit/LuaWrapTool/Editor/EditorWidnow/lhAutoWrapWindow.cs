using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;

namespace LaoHan.Tools.AutoWrap
{
    public class lhAutoWrapWindow : EditorWindow
    {
        private lhAutoWrap m_autoWrap;
        private string m_newAssembly;
        private Vector2 m_scrollPosition;
        private lhGUIStyles m_guiStyle;
        private static lhAutoWrapWindow window;
        [MenuItem("lhTools/LuaTool/AutoWrapTools %i")]
        static void Init()
        {
            window = null;
            window = EditorWindow.GetWindow<lhAutoWrapWindow>(true, "AutoWrap     (LaoHan)      QQ:369016334", true);
            window.Show();
        }
        void OnDestroy()
        {
            if (m_autoWrap!=null)
            {
                m_autoWrap.Dispose();
                m_autoWrap = null;
            }
            if (m_guiStyle!=null)
            {
                m_guiStyle.Dispose();
                m_guiStyle = null;
            }
        }
        void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                GUILayout.Label("loading....");
                return;
            }
            if (m_autoWrap==null)
            {
                m_autoWrap = lhAutoWrap.GetInstance();
                m_guiStyle = lhGUIStyles.GetInstance();
            }
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                {
                    GUILayout.Label("Assembly");
                    m_newAssembly = EditorGUILayout.TextField(m_newAssembly);
                    if (GUILayout.Button("Create", EditorStyles.toolbarButton))
                    {
                        m_autoWrap.CreateGroup(m_newAssembly);
                        m_newAssembly = "";
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Settings", EditorStyles.toolbarButton))
                    {
                        m_autoWrap.Settings();
                    }
                } EditorGUILayout.EndHorizontal();

                m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
                {
                    EditorGUILayout.BeginVertical();
                    {
                        foreach (var item in m_autoWrap.wrapGroupDic)
                        {
                            var wrapGroup = item.Value;
                            EditorGUILayout.BeginHorizontal(lhGUIStyles.GetStyle("TreeSelectedGrey"));
                            {
                                string unicode = wrapGroup.open ? "-" : "+";
                                if (GUILayout.Button(unicode, EditorStyles.label))
                                {
                                    wrapGroup.open = !wrapGroup.open;
                                }
                                GUILayout.Label(wrapGroup.assemblyName);
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("Search");
                                wrapGroup.search = EditorGUILayout.TextField(wrapGroup.search);
                                if (string.IsNullOrEmpty(wrapGroup.search))
                                    wrapGroup.searchTypeList.Clear();
                                wrapGroup.filterGroup = (lhAutoWrap.FilterGroup)EditorGUILayout.EnumPopup(wrapGroup.filterGroup, GUILayout.Width(100));
                            }
                            EditorGUILayout.EndHorizontal();
                            if (wrapGroup.open)
                            {
                                Action<List<lhAutoWrap.BindType>> ShowTargetTypeList = (typeList) =>
                                {
                                    int index = 0;
                                    for (int j = 0; j < typeList.Count; j++)
                                    {
                                        var bindType = typeList[j];
                                        if (wrapGroup.filterGroup == lhAutoWrap.FilterGroup.Wrapped && !bindType.isBuilded && !bindType.drop) continue;
                                        if (wrapGroup.filterGroup == lhAutoWrap.FilterGroup.NoneWrap && bindType.isBuilded && bindType.drop) continue;
                                        index++;
                                        GUIStyle selectStyle = wrapGroup.selectedList.Contains(bindType)? lhGUIStyles.GetStyle("TreeSelectedBlue") : lhGUIStyles.GetStyle("TreeSelectedGrey");
                                        
                                        Rect itemRect = EditorGUILayout.BeginHorizontal(selectStyle);
                                        {
                                            var current = GUI.color;
                                            if (bindType.isBuilded)
                                            {
                                                GUILayout.Label("✔");
                                                GUI.color = Color.green;
                                            }
                                            else
                                            {
                                                if (bindType.drop)
                                                    GUILayout.Label("✘");
                                                else
                                                    GUILayout.Space(23);
                                            }
                                            GUILayout.Label(index.ToString() + "  ");
                                            EditorGUILayout.LabelField(bindType.type.FullName);
                                            GUILayout.FlexibleSpace();
                                            GUILayout.Label(bindType.typeClassify);
                                            GUILayout.Space(20);
                                            GUI.color = current;
                                        }EditorGUILayout.EndHorizontal();
                                        bool shift = false;
                                        if (Event.current.keyCode == KeyCode.LeftShift || Event.current.keyCode == KeyCode.RightShift)
                                            shift = true;

                                        if (IsMouseOn(itemRect))
                                        {
                                            if (Event.current.type == EventType.mouseDown && Event.current.button == 0)
                                            {
                                                if (shift)
                                                {
                                                    if (wrapGroup.selectedList.Contains(bindType))
                                                        wrapGroup.selectedList.Remove(bindType);
                                                    else
                                                        wrapGroup.selectedList.Add(bindType);
                                                }
                                                else
                                                {
                                                    m_autoWrap.SelectClear();
                                                    wrapGroup.selectedList.Add(bindType);
                                                    lhExportWrapEditor.Show(bindType, (result) =>
                                                    {
                                                        switch(result)
                                                        {
                                                            case lhExportWrapEditor.ExportResult.Generate:
                                                                wrapGroup.GenerateWrap(bindType);
                                                                break;
                                                            case lhExportWrapEditor.ExportResult.ReGenerate:
                                                                wrapGroup.GenerateWrap(bindType);
                                                                break;
                                                            case lhExportWrapEditor.ExportResult.DeleteGenerate:
                                                                wrapGroup.DeleteWrap(bindType);
                                                                break;
                                                            case lhExportWrapEditor.ExportResult.BuildDelegate:
                                                                wrapGroup.BuildDelegate(bindType);
                                                                break;
                                                            case lhExportWrapEditor.ExportResult.ReBuildDelegate:
                                                                wrapGroup.BuildDelegate(bindType);
                                                                break;
                                                            case lhExportWrapEditor.ExportResult.DeleteDelegate:
                                                                wrapGroup.DeleteDelegate(bindType);
                                                                break;
                                                            case lhExportWrapEditor.ExportResult.Drop:
                                                                wrapGroup.DropWrap(bindType);
                                                                break;
                                                            case lhExportWrapEditor.ExportResult.UnDrop:
                                                                wrapGroup.UnDropWrap(bindType);
                                                                break;
                                                        }
                                                    });
                                                }
                                                Repaint();
                                            }
                                        }
                                    }
                                };
                                if (string.IsNullOrEmpty(wrapGroup.search))
                                {
                                    ShowTargetTypeList(wrapGroup.allTypeList);
                                }
                                else
                                {
                                    if (!string.Equals(wrapGroup.oldSearch, wrapGroup.search))
                                        wrapGroup.SearchTarget(wrapGroup.search);
                                    ShowTargetTypeList(wrapGroup.searchTypeList);
                                }
                            }
                        }
                    } EditorGUILayout.EndVertical();
                } EditorGUILayout.EndScrollView();

            } EditorGUILayout.EndVertical();
        }
        private bool IsMouseOn(Rect rect)
        {
            return rect.Contains(Event.current.mousePosition);
        }
    }
}