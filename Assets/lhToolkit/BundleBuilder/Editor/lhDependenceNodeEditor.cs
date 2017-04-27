using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LaoHan.Tools.BundleBuilder
{
    [CustomEditor(typeof(lhInspectorNodeObject))]
    class lhDependenceNodeEditor:Editor
    {
        private static Editor m_currentEditor;
        private static lhInspectorNodeObject m_inspector;
        private static lhBundleBuilder.BundleGroup.DependenceNode m_currentNode;
        private Vector2 m_scrollPosition;
        private bool m_filter=true;
        private lhBundleBuilder.BundleGroup.BundleType m_currentBundleType;
        private static Dictionary<lhBundleBuilder.AssetData, string> m_assetHashCode = new Dictionary<lhBundleBuilder.AssetData, string>();
        private static Dictionary<lhBundleBuilder.AssetData, string> m_referenceHashCode = new Dictionary<lhBundleBuilder.AssetData, string>();
        private static System.Security.Cryptography.SHA1Managed m_shal1 = new System.Security.Cryptography.SHA1Managed();
        void OnEnable()
        {
            m_currentEditor = this;
        }
        public static void Show(lhBundleBuilder.BundleGroup.DependenceNode node)
        {
            if(m_inspector==null)
            {
                m_inspector = ScriptableObject.CreateInstance<lhInspectorNodeObject>();
                m_inspector.hideFlags = UnityEngine.HideFlags.DontSave;
            }
            m_assetHashCode.Clear();
            m_referenceHashCode.Clear();
            m_inspector.name = node.bundleName;
            m_currentNode = node;
            Selection.activeObject = m_inspector;
            InitHashCode();
        }
        public override bool UseDefaultMargins()
        {
            return false;
        }
        public override void OnInspectorGUI()
        {
            if (m_currentNode == null) return;
            m_currentBundleType = m_currentNode.bundleType;
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("BundeType:");
                    m_currentNode.bundleType=(lhBundleBuilder.BundleGroup.BundleType)EditorGUILayout.EnumPopup(m_currentNode.bundleType);
                    if (m_currentBundleType != m_currentNode.bundleType)
                    {
                        if (m_currentNode.includeList.Count != 0)
                        {
                            if (EditorUtility.DisplayDialog("Warning", "if you change BundleType include must be Clear\nAre you sure?", "Ok", "Cancel"))
                                m_currentNode.DeleteIncludeAll();
                        }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Version: " + m_currentNode.version);
                } EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("BuildSize: " + (m_currentNode.size == -1 ? "Unkown" : Mathf.CeilToInt(m_currentNode.size / 1024f) + " KB"));
                    GUILayout.FlexibleSpace();
                    //GUILayout.Label("Priority:");
                    //m_currentNode.priority = EditorGUILayout.Popup(m_currentNode.priority, new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }, GUILayout.MaxWidth(40));

                } EditorGUILayout.EndHorizontal();

            } EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();

            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
            {
                IncludeGUI();
                EditorGUILayout.Separator();
                ReferenceGUI();
            } EditorGUILayout.EndScrollView();
            EditorGUILayout.BeginHorizontal();
            {
                if (m_filter)
                {
                    if(GUILayout.Button("ShowAll"))
                    {
                        m_filter = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Filter"))
                        m_filter = true;
                }
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("Refresh"))
                {
                    Repaint();
                }
            } EditorGUILayout.EndHorizontal();
        }
        public static void Refresh()
        {
            if (m_currentEditor!=null)
                m_currentEditor.Repaint();
        }
        private static void InitHashCode()
        {
            foreach (var asset in m_currentNode.includeList)
            {
                m_assetHashCode.Add(asset, getFileHashCode(asset));
            }
            foreach (var asset in m_currentNode.referenceList)
            {
                m_referenceHashCode.Add(asset, getFileHashCode(asset));
            }
        }
        private static string getFileHashCode(lhBundleBuilder.AssetData asset)
        {
            byte[] fileBytes = File.ReadAllBytes(Application.dataPath.Replace("Assets", "") + asset.path);
            byte[] hashBytes = m_shal1.ComputeHash(fileBytes);
            var newHashCode = Convert.ToBase64String(hashBytes);

            string newMetaHashCode = "";
            string metaFile = Application.dataPath.Replace("Assets", "") + asset.path + ".meta";
            if (File.Exists(metaFile))
            {
                byte[] metaBytes = File.ReadAllBytes(metaFile);
                newMetaHashCode = Convert.ToBase64String(m_shal1.ComputeHash(metaBytes));
            }
            else
                newMetaHashCode = "";
            return newHashCode + "," + newMetaHashCode;
        }
        private void IncludeGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                GUILayout.Label("Include:");
                var includePath = m_currentNode.includeList;
                string deletePath = null;
                foreach(var asset in includePath)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        string fileName = Path.GetFileNameWithoutExtension(asset.path);
                        if(m_currentNode.bundleType==lhBundleBuilder.BundleGroup.BundleType.AssetBundle)
                        {
                            Rect mainAssetRect = GUILayoutUtility.GetRect(new GUIContent("√"), EditorStyles.miniLabel, GUILayout.ExpandWidth(false));
                            if(GUI.Button(mainAssetRect, string.Equals(m_currentNode.mainAsset.path, asset.path)?"√":"" , EditorStyles.miniLabel))
                            {
                                m_currentNode.mainAsset = asset;
                            }
                        }
                        EditorGUIUtility.SetIconSize(new Vector2(14f, 14f));
                        GUILayout.Label(AssetDatabase.GetCachedIcon(asset.path));

                        GUILayout.Label(fileName);
                        GUILayout.FlexibleSpace();

                        if (m_currentNode.bundleType == lhBundleBuilder.BundleGroup.BundleType.AssetBundle)
                        {
                            GUIContent pingContent = new GUIContent("◎");
                            Rect pingRect = GUILayoutUtility.GetRect(pingContent, EditorStyles.miniLabel, GUILayout.ExpandWidth(false));
                            if (GUI.Button(pingRect, pingContent, EditorStyles.miniLabel))
                            {
                                PingObject(asset.path);
                            }
                        }

                        Color curColor = GUI.color;
                        GUIContent deleteContent = new GUIContent("×");
                        Rect deleteRect = GUILayoutUtility.GetRect(deleteContent, EditorStyles.miniLabel, GUILayout.ExpandWidth(false));
                        if (GUI.Button(deleteRect, deleteContent, EditorStyles.miniLabel))
                        {
                            deletePath = asset.path;
                        }

                        ShowChange(asset,m_assetHashCode[asset]);
                        GUILayout.Space(13);

                    } EditorGUILayout.EndHorizontal();
                }
                if(!string.IsNullOrEmpty(deletePath))
                {
                    if(EditorUtility.DisplayDialog("LaoHan:","Are you sure you want to delete it","ok","cancel"))
                    {
                        m_currentNode.DeleteInclude(deletePath);
                    }
                }
            } EditorGUILayout.EndVertical();
        }
        private void ReferenceGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                GUILayout.Label("References:");
                var rederenceList = m_currentNode.referenceList;
                var parentReferenceList = m_currentNode.GetParentReferences();
                foreach (var asset in rederenceList)
                {
                    bool filter = true;
                    FileInfo fileInfo = new FileInfo(asset.path);
                    if(m_filter)
                    {
                        if (fileInfo.Extension == ".cs" || fileInfo.Extension == ".js")
                            continue;
                    }
                    else
                    {
                        if (fileInfo.Extension == ".cs" || fileInfo.Extension == ".js")
                            filter = false;
                    }

                    Rect itemRect=EditorGUILayout.BeginHorizontal();
                    {
                        Color curColor = GUI.color;
                        string fileName = Path.GetFileNameWithoutExtension(asset.path);
                        EditorGUIUtility.SetIconSize(new Vector2(14f, 14f));
                        GUILayout.Label(AssetDatabase.GetCachedIcon(asset.path));

                        if (!filter)
                            GUI.color = Color.grey;
                        else
                        {
                            for (int j = 0; j < parentReferenceList.Count; j++)
                            {
                                if(parentReferenceList[j].path.Equals(asset.path))
                                    GUI.color = Color.green;
                            }
                        }
                        GUILayout.Label(fileName);
                        GUI.color = curColor;
                        GUILayout.FlexibleSpace();
                        ShowChange(asset,m_referenceHashCode[asset]);
                        GUILayout.Space(10);
                    } EditorGUILayout.EndHorizontal();
                    if(IsMouseOn(itemRect) && Event.current.type==EventType.MouseDown)
                    {
                        PingObject(asset.path);
                    }
                }

            } EditorGUILayout.EndVertical();
        }
        private bool IsMouseOn(Rect rect)
        {
            return rect.Contains(Event.current.mousePosition);
        }
        private void PingObject(string path)
        {
            UnityEngine.Object[] obj=AssetDatabase.LoadAllAssetsAtPath(path);
            int instanceID=obj[0].GetInstanceID();
            EditorGUIUtility.PingObject(instanceID);
        }
        private void ShowChange(lhBundleBuilder.AssetData asset,string hasCode)
        {
            string[] split = hasCode.Split(',');
            if (asset.hashCode.Equals(split[0]) && asset.metaHashCode.Equals(split[1]))
            {
                GUI.color = Color.green;
                GUILayout.Label("√");
                GUI.color = Color.white;
            }
            else
            {
                GUI.color = Color.yellow;
                GUILayout.Label("!");
                GUI.color = Color.white;
            }
        }
    }
}
