using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LaoHan.Tools.WorldEditor
{
    public class lhTriggerEditorWindow : EditorWindow
    {
        private lhGUIStyles m_GUIStyles;
        private lhTriggerManager m_triggerManager;
        private lhDragHandler m_dragHandler;
        private Vector2 m_scrollPosition;
        private List<lhTriggerManager.Node> m_selectList;
        private lhTriggerManager.Node m_isAdd;
        private lhTriggerManager.Node m_isInsertUp;
        private lhTriggerManager.Node m_isInsertDown;
        private bool m_isControl;
        private float m_doubleClick;
        [MenuItem("lhTools/WorldEditor/TriggerEditor %l")]
        static void Intialize()
        {
            lhTriggerEditorWindow window = EditorWindow.GetWindow<lhTriggerEditorWindow>(false, "TriggerEditor   (LaoHan)      QQ:369016334", true);
            window.Show();
        }
        void OnDestroy()
        {
            if (m_triggerManager != null)
            {
                if (EditorUtility.DisplayDialog("Information","Are you sure save?","Ok","Cancel"))
                {
                    m_triggerManager.Apply();
                }
                m_triggerManager.Dispose();
                m_triggerManager = null;
            }
            if (m_GUIStyles != null)
            {
                m_GUIStyles.Dispose();
                m_GUIStyles = null;
            }
        }
        void OnGUI()
        {
            if (m_triggerManager == null)
            {
                m_GUIStyles = lhGUIStyles.GetInstance();
                m_triggerManager = lhTriggerManager.GetInstance();
                m_dragHandler = new lhDragHandler();
                m_selectList = new List<lhTriggerManager.Node>();
            }
            if(Event.current.keyCode==KeyCode.LeftControl)
            {
                if(Event.current.type==EventType.KeyDown )
                {
                    m_isControl = true;
                }
                else if (Event.current.type==EventType.keyUp)
                {
                    m_isControl = false;
                }
            }
            Rect curRectWindow = EditorGUILayout.BeginVertical(lhGUIStyles.GetStyle("OL Box"));
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
                {
                    if (GUILayout.Button("Create", EditorStyles.toolbarButton))
                    {
                        m_triggerManager.CreateGroup();

                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Revert", EditorStyles.toolbarButton))
                    {
                        if (EditorUtility.DisplayDialog("Information", "Are you sure Revert", "Ok", "Cancel"))
                        {
                            m_triggerManager.Revert();
                        }

                    }
                    if (GUILayout.Button("Apply", EditorStyles.toolbarButton))
                    {
                        if (EditorUtility.DisplayDialog("Information", "Are you sure Save", "Ok", "Cancel"))
                        {
                            m_triggerManager.Apply();
                        }
                    }
                    if (GUILayout.Button("Export", EditorStyles.toolbarButton))
                    {
                        if (EditorUtility.DisplayDialog("Information", "Are you sure Save", "Ok", "Cancel"))
                        {
                            m_triggerManager.Export();
                        }
                    }
                    if (GUILayout.Button("Settings", EditorStyles.toolbarButton))
                    {
                        lhTriggerSettingEditor.Show();
                    }

                } EditorGUILayout.EndHorizontal();

                GUI.SetNextControlName("ScrollPosition");
                m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
                {
                    foreach (var group in m_triggerManager.triggerList)
                    {
                        Rect itemRect = EditorGUILayout.BeginHorizontal(EditorStyles.toolbarButton, GUILayout.ExpandWidth(true));
                        {
                            GUILayout.Label(group.index.ToString());
                            GUIContent content = group.open ? new GUIContent("-") : new GUIContent("+");
                            Rect dropButton = GUILayoutUtility.GetRect(content, EditorStyles.miniLabel, GUILayout.ExpandWidth(false));
                            if (GUI.Button(dropButton, content, EditorStyles.miniLabel))
                            {
                                group.open = !group.open;
                            }
                            if (group.renaming)
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
                                    if (m_triggerManager.HasSameGroupName(group, group.oldGroupName))
                                    {
                                        EditorUtility.DisplayDialog("Error", "Has the same groupName,please rename the groupname", "Ok");
                                    }
                                    else
                                    {
                                        group.EndRename();
                                        GUI.FocusControl("ScrollPosition");
                                    }
                                }
                                group.oldGroupName = EditorGUILayout.TextField(group.oldGroupName);
                            }
                            else
                            {
                                GUILayout.Label(group.triggerName);
                            }

                            GUILayout.FlexibleSpace();

                            GUIContent deleteContent = new GUIContent("x");
                            if (GUI.Button(GUILayoutUtility.GetRect(deleteContent, EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)), deleteContent, EditorStyles.toolbarButton))
                            {
                                if (EditorUtility.DisplayDialog("Information", "Are you sure delete group?", "Ok", "Cancel"))
                                {
                                    m_triggerManager.DeleteGroup(group);
                                    Repaint();
                                    return;
                                }
                            }
                        } GUILayout.EndHorizontal();

                        if (group.open)
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                for (int i = 0; i < group.treeList.Count; i++)
                                {
                                    TreeLayout(group.treeList[i]);
                                }
                            } EditorGUILayout.EndVertical();
                        }

                        if (IsMouseOn(itemRect))
                        {
                            if (Event.current.keyCode == KeyCode.F2)
                            {
                                group.StartRename();
                                Repaint();
                            }
                            if (Event.current.keyCode == KeyCode.Return)
                            {
                                if (m_triggerManager.HasSameGroupName(group, group.oldGroupName))
                                {
                                    EditorUtility.DisplayDialog("Error", "Has the same groupName,please rename the groupname", "Ok");
                                }
                                else
                                {
                                    group.EndRename();
                                    Repaint();
                                    GUI.FocusControl("ScrollPosition");
                                }
                            }
                            if (Event.current.type == EventType.DragUpdated)
                            {
                                m_dragHandler.SetVisualMode(DragAndDropVisualMode.Move);
                            }
                            if (Event.current.type == EventType.DragPerform)
                            {
                                //var movedNode = (lhTriggerManager.TriggerGroup.Node)m_dragHandler.GetGenericData("DependenceNode");
                                //if (!group.treeList.Contains(movedNode))
                                //{
                                //    movedNode.DeleteOwn();
                                //    group.AddTree(movedNode);
                                //    m_dragHandler.AcceptDrag();
                                //}
                            }
                        }
                    }
                } EditorGUILayout.EndScrollView();

            } EditorGUILayout.EndVertical();
        }
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
                    m_triggerManager.Clear();
                }
            }
        }
        private bool TreeLayout(lhTriggerManager.Node node)
        {
            bool isSelected = m_selectList.Contains(node);
            GUIStyle selectStyle = isSelected ? lhGUIStyles.GetStyle("TreeSelectedBlue") : lhGUIStyles.GetStyle("TreeSelectedGrey");
            if (m_isAdd == node)
                selectStyle = lhGUIStyles.GetStyle("TreeAdd");
            if (m_isInsertUp == node)
                selectStyle = lhGUIStyles.GetStyle("TreeInsertUp");
            if (m_isInsertDown == node)
                selectStyle = lhGUIStyles.GetStyle("TreeInsertDown");
            if(isSelected)
            {
                if (m_isControl)
                {
                    if (Event.current.keyCode == KeyCode.D && Event.current.type == EventType.keyUp)
                    {
                        node.Copy();
                        Repaint();
                        return true;
                    }
                }
            }
            Rect nodeRect = EditorGUILayout.BeginHorizontal(selectStyle);
            {
                GUILayout.Space(20 * node.layer);
                EditorGUILayout.LabelField(node.name);
                if (node.content!=null)
                {
                    var eles=node.content.Elements("prop");
                    string label = "";
                    int count = 0;
                    foreach (var item in eles)
                    {
                        label += item.Attribute("name").Value + "=" + item.Value ;
                        if (count<eles.Count()-1)
                        {
                            label += ",";
                        }
                        count++;
                    }
                    GUILayout.Label("("+label+")",GUILayout.MaxWidth(999999999));
                }
                GUILayout.FlexibleSpace();
                if (node.layer != 0)
                {
                    if (GUILayout.Button("-", EditorStyles.miniLabel))
                    {
                        node.DeleteOwn();
                        Repaint();
                        return true;
                    }
                }
            } EditorGUILayout.EndHorizontal();
            GenericMenu menu = new GenericMenu();
            if (node.menulist != null)
            {
                foreach (var item in node.menulist)
                {
                    string name = item.Attribute("name").Value;
                    string classify = item.Attribute("classify").Value;
                    menu.AddItem(new GUIContent(classify + "/" + name), false, node.AddChild, item);
                }
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("And"), false, node.AddJudge, lhTriggerManager.NodeType.And);
                menu.AddItem(new GUIContent("Or"), false, node.AddJudge, lhTriggerManager.NodeType.Or);
                menu.AddItem(new GUIContent("Not"), false, node.AddJudge, lhTriggerManager.NodeType.Not);
            }
            if (IsMouseOn(nodeRect))
            {
                if (Event.current.button == 1 && Event.current.type == EventType.mouseUp)
                {
                    Vector2 mousePos = Event.current.mousePosition;
                    menu.DropDown(new Rect(mousePos.x, mousePos.y, 0, 0));
                }
                if (Event.current.button==0)
                {
                    if(Event.current.type==EventType.mouseDown)
                    {
                        if (node.parent!=null)
                        {
                            if (Event.current.shift)
                                AddSelected(node);
                            else
                                AddSingleSelected(node);
                            Repaint();
                            lhTriggerPropertyEditor.Show(node);
                        }
                        if (m_doubleClick==0)
                        {
                            m_doubleClick = System.DateTime.Now.Millisecond;
                        }
                        else
                        {
                            if (System.DateTime.Now.Millisecond - m_doubleClick <170)
                            {
                                m_doubleClick = 0;
                                node.ShowPing();
                            }
                        }
                    }
                    if (Event.current.type == EventType.mouseDrag)
                    {
                        if (node.parent != null)
                        {
                            m_dragHandler.PrepareStartDrag(new string[] { }, new Object[] { });
                            m_dragHandler.SetGenericData("Node", node);
                            m_dragHandler.MouseDrag("NodeMove");
                        }
                    }
                    if (Event.current.type == EventType.DragExited)
                    {
                        m_isAdd = null;
                        m_isInsertUp = null;
                        m_isInsertDown = null;
                    }
                    if (Event.current.type == EventType.DragUpdated)
                    {
                        var movedNode = (lhTriggerManager.Node)m_dragHandler.GetGenericData("Node");
                        if (movedNode != node && movedNode.nodeClassify == node.nodeClassify)
                        {
                            if (Event.current.mousePosition.y > nodeRect.center.y + nodeRect.height / 3)//down
                            {
                                if (node.parent != null)
                                {
                                    m_isInsertUp = null;
                                    m_isInsertDown = node;
                                    m_isAdd = null;
                                }
                                else
                                {
                                    m_isInsertUp = null;
                                    m_isInsertDown = null;
                                    m_isAdd = null;
                                }
                            }
                            else if (Event.current.mousePosition.y < nodeRect.center.y - nodeRect.height / 3)//up
                            {
                                if (node.parent != null)
                                {
                                    m_isInsertDown = null;
                                    m_isInsertUp = node;
                                    m_isAdd = null;
                                }
                                else
                                {
                                    m_isInsertUp = null;
                                    m_isInsertDown = null;
                                    m_isAdd = null;
                                }
                            }
                            else
                            {
                                m_isAdd = node;
                                m_isInsertUp = null;
                                m_isInsertDown = null;
                            }
                            m_dragHandler.SetVisualMode(DragAndDropVisualMode.Move);
                            Repaint();
                        }
                    }
                    if (Event.current.type == EventType.DragPerform)
                    {
                        var movedNode = (lhTriggerManager.Node)m_dragHandler.GetGenericData("Node");
                        if (movedNode != node && movedNode.nodeClassify == node.nodeClassify && !movedNode.HasNode(node))
                        {
                            if (Event.current.mousePosition.y > nodeRect.center.y + nodeRect.height / 3 )//down
                            {
                                if (node.parent != null)
                                {
                                    movedNode.DeleteOwn();
                                    node.InsertChildDown(movedNode);
                                }
                            }
                            else if( Event.current.mousePosition.y < nodeRect.center.y - nodeRect.height / 3)//up
                            {
                                if (node.parent != null)
                                {
                                    movedNode.DeleteOwn();
                                    node.InsertChildUp(movedNode);
                                }
                            }
                            else
                            {
                                movedNode.DeleteOwn();
                                node.AddChild(movedNode);
                            }
                            m_dragHandler.AcceptDrag();
                            Repaint();
                            m_isInsertUp = null;
                            m_isInsertDown = null;
                            m_isAdd = null;
                            return true;
                        }
                        else
                        {
                            m_isInsertUp = null;
                            m_isInsertDown = null;
                            m_isAdd = null;
                        }
                    }
                }
            }
            foreach (var item in node.childList)
            {
                bool repaint=TreeLayout(item);
                if (repaint)
                    return true;
            }
            return false;
        }
        private void AddSelected(lhTriggerManager.Node node)
        {
            if (!m_selectList.Contains(node))
            {
                m_selectList.Add(node);
                node.isSelected = true;
            }
        }
        private void AddSingleSelected(lhTriggerManager.Node node)
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