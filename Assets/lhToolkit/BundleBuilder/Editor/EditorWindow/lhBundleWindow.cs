using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

namespace LaoHan.Tools.BundleBuilder
{
    public class lhBundleWindow : EditorWindow
    {
        #region private member
        private lhGUIStyles m_GUIStyles;
        private lhBundleBuilder m_bundleManager;
        private lhDragHandler m_dragHandler;
        private Vector2 m_scrollPosition;
        private List<lhBundleBuilder.BundleGroup.DependenceNode> m_selectList;
        #endregion

        #region UnityEditor
        [MenuItem("lhTools/AssetBundle/BundleBuilder4.x")]
        static void Intialize()
        {
            lhBundleWindow window = EditorWindow.GetWindow<lhBundleWindow>(true, "BundleManager    (LaoHan)      QQ:369016334", true);
            window.Show();
        }
        void OnDestroy()
        {
            if (m_bundleManager != null)
            {
                m_bundleManager.Dispose();
                m_bundleManager = null;
            }
        }
        void OnGUI()
        {
            if (m_bundleManager==null)
            {
                m_GUIStyles = lhGUIStyles.GetInstance();
                m_bundleManager=lhBundleBuilder.GetInstance();
                m_dragHandler = new lhDragHandler();
                m_selectList = new List<lhBundleBuilder.BundleGroup.DependenceNode>();
            }
            Rect curRectWindow=EditorGUILayout.BeginVertical(lhGUIStyles.GetStyle("OL Box"));
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
                {
                    Rect createRect=GUILayoutUtility.GetRect(new GUIContent("Create"),EditorStyles.toolbarDropDown,GUILayout.ExpandWidth(false));
                    if (GUI.Button(createRect, "Create", EditorStyles.toolbarButton))
                    {
                        m_bundleManager.CreateGroup();
                        GUI.FocusControl("GroupRename");
                    }

                    Rect buildRect = GUILayoutUtility.GetRect(new GUIContent("Build"), EditorStyles.toolbarDropDown, GUILayout.ExpandWidth(false));
                    if(GUI.Button(buildRect,"Build",EditorStyles.toolbarButton))
                    {
                        if (EditorUtility.DisplayDialog("Information", "Are you sure BuildAll", "Ok", "Cancel"))
                        {
                            m_bundleManager.BuildAll();
                        }
                    }

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Revert", EditorStyles.toolbarButton))
                    {
                        if(EditorUtility.DisplayDialog("Information","Are you sure Revert","Ok","Cancel"))
                        {
                            m_bundleManager.Revert();
                        }
                        
                    }
                    if(GUILayout.Button("Apply",EditorStyles.toolbarButton))
                    {
                        if (EditorUtility.DisplayDialog("Information", "Are you sure Save", "Ok", "Cancel"))
                        {
                            m_bundleManager.Apply();
                        }
                    }
                    if (GUILayout.Button("Settings", EditorStyles.toolbarButton))
                        lhSettingEditor.Show();

                } EditorGUILayout.EndHorizontal();

                GUI.SetNextControlName("ScrollPosition");
                m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
                {
                    foreach(var group in lhBundleBuilder.bundleGroup)
                    {
                        Rect itemRect=EditorGUILayout.BeginHorizontal(EditorStyles.toolbarButton,GUILayout.ExpandWidth(true));
                        {
                            if (group.treeList.Count != 0)
                            {
                                GUIContent content = group.editorOpen ? new GUIContent("▲") : new GUIContent("▼");
                                Rect dropButton = GUILayoutUtility.GetRect(content, EditorStyles.miniLabel, GUILayout.ExpandWidth(false));
                                if (GUI.Button(dropButton, content, EditorStyles.miniLabel))
                                {
                                    group.editorOpen = !group.editorOpen;
                                }
                            }

                            if(group.renaming)
                            {
                                GUIContent cancelContent = new GUIContent("×");
                                if (GUI.Button(GUILayoutUtility.GetRect(cancelContent, EditorStyles.miniButton, GUILayout.ExpandWidth(false)), cancelContent))
                                {
                                    group.renaming = false;
                                    GUI.FocusControl("ScrollPosition");
                                }
                                GUIContent certainContent = new GUIContent("√");
                                if (GUI.Button(GUILayoutUtility.GetRect(certainContent, EditorStyles.miniButton, GUILayout.ExpandWidth(false)), certainContent))
                                {
                                    if (m_bundleManager.HasSameGroupName(group, group.oldGroupName))
                                    {
                                        EditorUtility.DisplayDialog("Error", "Has the same groupName,please rename the groupname", "Ok");
                                    }
                                    else
                                    {
                                        group.renaming = false;
                                        group.groupName = group.oldGroupName;
                                        GUI.FocusControl("ScrollPosition");
                                    }
                                }
                                group.oldGroupName = EditorGUILayout.TextField(group.oldGroupName);
                            }
                            else
                            {
                                //GUIContent renameContent = new GUIContent(" Q ");
                                //if (GUI.Button(GUILayoutUtility.GetRect(renameContent, EditorStyles.miniLabel, GUILayout.ExpandWidth(false)),renameContent, EditorStyles.miniLabel))
                                //{
                                //    group.renaming = true;
                                //    group.oldGroupName = group.groupName;
                                //    Repaint();
                                //}
                                GUILayout.Label(group.groupName);
                            }

                            GUILayout.FlexibleSpace();
                            Rect createRect = GUILayoutUtility.GetRect(new GUIContent("Create"), EditorStyles.miniLabel, GUILayout.ExpandWidth(false));
                            if (GUI.Button(createRect, "Create", EditorStyles.miniLabel))
                            {
                                group.CreateBundle();
                            }
                            Rect buildRect = GUILayoutUtility.GetRect(new GUIContent("Build"), EditorStyles.toolbarDropDown, GUILayout.ExpandWidth(false));
                            if (GUI.Button(buildRect, "Build", EditorStyles.toolbarDropDown))
                            {
                                GenericMenu menu = new GenericMenu();
                                menu.AddItem(new GUIContent("Build All"), false, group.BuildAll);
                                menu.AddItem(new GUIContent("Build Selected"), false, group.BuildSelected);
                                menu.AddItem(new GUIContent("Clear All"), false, group.Clear);
                                menu.DropDown(buildRect);
                            }

                            GUIContent deleteContent = new GUIContent("x");
                            if (GUI.Button(GUILayoutUtility.GetRect(deleteContent, EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)), deleteContent, EditorStyles.toolbarButton))
                            {
                                if(EditorUtility.DisplayDialog("Information", "Are you sure delete group?", "Ok","Cancel"))
                                {
                                    m_bundleManager.DeleteGroup(group);
                                    Repaint();
                                    return;
                                }
                            }


                        } GUILayout.EndHorizontal();
                        if(group.editorOpen)
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                TreeLayout(group.treeList);
                            } EditorGUILayout.EndVertical();
                        }

                        if (IsMouseOn(itemRect))
                        {
                            if(Event.current.keyCode==KeyCode.F2)
                            {
                                group.renaming = true;
                                group.oldGroupName = group.groupName;
                                Repaint();
                            }
                            if(Event.current.keyCode==KeyCode.Return)
                            {
                                if (m_bundleManager.HasSameGroupName(group, group.oldGroupName))
                                {
                                    EditorUtility.DisplayDialog("Error", "Has the same groupName,please rename the groupname", "Ok");
                                }
                                else
                                {
                                    group.renaming = false;
                                    group.groupName = group.oldGroupName;
                                    Repaint();
                                    GUI.FocusControl("ScrollPosition");
                                }
                            }
                            if(Event.current.type==EventType.DragUpdated)
                            {
                                m_dragHandler.SetVisualMode(DragAndDropVisualMode.Move);
                            }
                            if (Event.current.type == EventType.DragPerform)
                            {
                                var movedNode = (lhBundleBuilder.BundleGroup.DependenceNode)m_dragHandler.GetGenericData("DependenceNode");
                                if (!group.treeList.Contains(movedNode))
                                {
                                    movedNode.DeleteOwn();
                                    group.AddTree(movedNode);
                                    m_dragHandler.AcceptDrag();
                                }
                            }
                        }
                    }
                } EditorGUILayout.EndScrollView();

            } EditorGUILayout.EndVertical();
        }
        #endregion

        #region private methods
        private bool HasFocuse()
        {
            return this == EditorWindow.focusedWindow;
        }
        private bool IsMouseOn(Rect rect)
        {
            return rect.Contains(Event.current.mousePosition);
        }
        private void BuildAll()
        {

        }
        private void ClearAll()
        {
            if (EditorUtility.DisplayDialog("Warning:", "Delete All\n You can never recover, please be careful.", "ok", "cancel"))
            {
                if (EditorUtility.DisplayDialog("老晗:", "卧槽，你真要全删啊？删了就真无法恢复了啊", "确定", "取消"))
                {
                    m_bundleManager.Clear();
                }
            }
        }
        private void TreeLayout(List<lhBundleBuilder.BundleGroup.DependenceNode> treeList)
        {
            if (treeList == null) return;
            for (int i = 0; i < treeList.Count; i++)
            {
                var node = treeList[i];
                GUIContent content = node.editorOpen ? new GUIContent("▲") : new GUIContent("▼");
                bool isSelected = m_selectList.Contains(node);
                GUIStyle selectStyle = isSelected ? lhGUIStyles.GetStyle("TreeSelectedBlue") : lhGUIStyles.GetStyle("TreeSelectedGrey");
                Rect itemRect = EditorGUILayout.BeginHorizontal(selectStyle);
                {
                    GUILayout.Space(node.layer * 20);
                    if (node.childList.Count != 0)
                    {
                        Rect dropNode = GUILayoutUtility.GetRect(content, EditorStyles.miniLabel, GUILayout.ExpandWidth(false));
                        if (GUI.Button(dropNode, content, EditorStyles.miniLabel))
                        {
                            node.editorOpen = !node.editorOpen;
                        }
                    }
                    //-------------rename GUI
                    if (node.renaming)
                    {
                        GUIContent cancelContent = new GUIContent("×");
                        if (GUI.Button(GUILayoutUtility.GetRect(cancelContent, EditorStyles.miniButton, GUILayout.ExpandWidth(false)), cancelContent))
                        {
                            node.renaming = false;
                            GUI.FocusControl("ScrollPosition");
                        }
                        GUIContent certainContent=new GUIContent("√");
                        if (GUI.Button(GUILayoutUtility.GetRect(certainContent, EditorStyles.miniButton, GUILayout.ExpandWidth(false)), certainContent))
                        {
                            if (node.group.HasSameNodeName(node, node.oldBundleName))
                            {
                                EditorUtility.DisplayDialog("Error", "Has the Same Name,please rename this", "Ok");
                            }
                            else
                            {
                                node.renaming = false;
                                node.bundleName = node.oldBundleName;
                                GUI.FocusControl("ScrollPosition");
                            }
                        }
                        node.oldBundleName = GUILayout.TextField(node.oldBundleName, EditorStyles.textField);
                    }
                    else
                        GUILayout.Label(node.bundleName);

                } EditorGUILayout.EndHorizontal();

                if(node.editorOpen )
                {
                    TreeLayout(node.childList);
                }

                //-----------------------GenericMenu
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("CreateAssetBundle"), false, node.CreateBundle);
                menu.AddItem(new GUIContent("CreateScene"), false, node.CreateScene);
                menu.AddItem(new GUIContent("Rename"), false, () => {
                    node.renaming = true;
                    node.oldBundleName = node.bundleName;
                    Repaint();
                });
                menu.AddItem(new GUIContent("Delete"), false, node.DeleteOwn);

                if (IsMouseOn(itemRect))
                {
                    if( Event.current.button == 1 && Event.current.type == EventType.mouseUp )
                    {
                        Vector2 mousePos = Event.current.mousePosition;
                        menu.DropDown(new Rect(mousePos.x, mousePos.y, 0, 0));
                    }
                    if(Event.current.button == 0)
                    {
                        //--------------------selected blue or grey
                        if(Event.current.type==EventType.MouseDown)
                        {
                            if(Event.current.shift)
                            {
                                AddSelected(node);
                            }
                            else
                            {
                                AddSingleSelected(node);
                            }
                            lhDependenceNodeEditor.Show(node);
                            lhDependenceNodeEditor.Refresh();
                            Repaint();
                        }

                        if (Event.current.type == EventType.mouseDrag)
                        {
                            m_dragHandler.PrepareStartDrag(new string[] { }, new Object[] { });
                            m_dragHandler.SetGenericData("DependenceNode",node);
                            m_dragHandler.MouseDrag("DependenceNodeDrag");
                        }
                        if(Event.current.type==EventType.DragUpdated)
                        {
                            m_dragHandler.SetVisualMode(DragAndDropVisualMode.Move);
                        }
                        if(Event.current.type == EventType.DragPerform)
                        {
                            //--------------drag file object to GUI
                            if(m_dragHandler.HasObject())
                            {
                                string[] filePathArr= m_dragHandler.GetAssetPaths();
                                if(node.bundleType==lhBundleBuilder.BundleGroup.BundleType.AssetBundle)
                                {
                                    List<string> filePathList = new List<string>();
                                    foreach(string filePath in filePathArr)
                                    {
                                        FileInfo fileInfo = new FileInfo(filePath);
                                        if (fileInfo.Extension == ".cs" || fileInfo.Extension == ".js" || fileInfo.Extension == ".unity" || Directory.Exists(filePath))
                                        {
                                            EditorUtility.DisplayDialog("Error!", ".cs , .js or .unity is nont Build AssetBundle:" + filePath,"Ok");
                                            continue;
                                        }
                                        filePathList.Add(filePath);
                                    }
                                    node.AddInclude(filePathList.ToArray());
                                }
                                else
                                {
                                    if (node.includeList.Count >= 1)
                                        EditorUtility.DisplayDialog("Error!", " Build Scene is only support Single Scene:\n", "Ok");
                                    else
                                    {
                                        FileInfo fileInfo = new FileInfo(filePathArr[0]);
                                        if (fileInfo.Extension != ".unity" || Directory.Exists(filePathArr[0]))
                                        {
                                            EditorUtility.DisplayDialog("Error!", " Build Scene is only support .unity:\n" + fileInfo.FullName, "Ok");
                                            continue;
                                        }
                                        else
                                            node.AddInclude(filePathArr);

                                    }
                                }
                                AddSingleSelected(node);
                                lhDependenceNodeEditor.Show(node);
                                lhDependenceNodeEditor.Refresh();
                            }
                            else//-------------GUI drag
                            {
                                var movedNode = (lhBundleBuilder.BundleGroup.DependenceNode)m_dragHandler.GetGenericData("DependenceNode");
                                if(movedNode!=node)
                                {
                                    if (!movedNode.HasNode(node))
                                    {
                                        movedNode.DeleteOwn();
                                        node.AddChild(movedNode);
                                        m_dragHandler.AcceptDrag();
                                    }
                                }
                            }
                        }
                    }
                    if(Event.current.keyCode==KeyCode.F2)
                    {
                        node.renaming = true;
                        node.oldBundleName = node.bundleName;
                        GUI.FocusControl("RenameTextField");
                        Repaint();
                    }
                    if(Event.current.keyCode==KeyCode.Return)
                    {
                        if (node.group.HasSameNodeName(node,node.oldBundleName))
                        {
                            EditorUtility.DisplayDialog("Error", "Has the Same Name,please rename this","Ok");
                        }
                        else
                        {
                            node.renaming = false;
                            node.bundleName = node.oldBundleName;
                            Repaint();
                        }
                    }
                }
            }
        }
        private void AddSelected(lhBundleBuilder.BundleGroup.DependenceNode node)
        {
            if (!m_selectList.Contains(node))
            {
                m_selectList.Add(node);
                node.isSelected = true;
            }
        }
        private void AddSingleSelected(lhBundleBuilder.BundleGroup.DependenceNode node)
        {
            foreach (var selNode in m_selectList)
            {
                selNode.isSelected = false;
            }
            m_selectList.Clear();
            m_selectList.Add(node);
            node.isSelected = true;
        }
        #endregion
    }
}