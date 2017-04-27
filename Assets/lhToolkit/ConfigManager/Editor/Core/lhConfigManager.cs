using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LaoHan.Infrastruture;

namespace LaoHan.Tools.ConfigManager
{
    public class lhConfigManager
    {
        public enum FileType
        {
            Csv,
            Json,
            Xml
        }
        private class ConfigFile
        {
            public FileType fileType { get; set; }
            public string fileName { get; set; }
            public string path { get; set; }
            public int index { get; set; }
            public ConfigFile()
            {

            }
        }
        private class ConfigContent
        {
            public ConfigContent(ConfigFile config)
            {

            }
            public virtual void DisplayContent()
            {

            }
        }
        private class CsvContent: ConfigContent
        {
            private string[] m_titleArr;
            private string[][] m_contentArr;
            public CsvContent(ConfigFile config) :base(config)
            {
                var text=File.ReadAllText(config.path);
                m_contentArr=lhCsv.Parse(text,'\n');
                m_titleArr = m_contentArr[0];
            }
            public override void DisplayContent()
            {
                EditorGUILayout.BeginVertical();
                {
                    if (m_titleArr!=null)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            for (int i = 0; i < m_titleArr.Length; i++)
                            {
                                EditorGUILayout.LabelField(m_titleArr[i], GUILayout.Width(100), GUILayout.Height(20));
                            }
                        }EditorGUILayout.EndHorizontal();
                     }
                    EditorGUILayout.BeginVertical();
                    {
                        for (int i = 1; i < m_contentArr.Length; i++)
                        {
                            var rowArr = m_contentArr[i];
                            EditorGUILayout.BeginHorizontal();
                            {
                                for (int j = 0; j < rowArr.Length; j++)
                                {
                                    var colArr = rowArr[j];
                                    EditorGUILayout.TextField(colArr, GUILayout.Width(100), GUILayout.Height(20));
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }EditorGUILayout.EndVertical();
                }EditorGUILayout.EndVertical();
                base.DisplayContent();
            }
        }
        private class JsonContent: ConfigContent
        {

            public class Node
            {
                public EJsonType jsonType;
                public int index;
                public int layer;
                public Node parent;
                public string key;
                public IJsonNode value;
                public List<Node> childs=new List<Node>();

                public bool open=true;
                public Node()
                {

                }
                public Node(int index,int layer,IJsonNode json)
                {
                    this.index = index;
                    this.layer = layer;
                    jsonType = json.type;
                    if (json.type==EJsonType.Object)
                    {
                        var jsonObj = json as JsonObject;
                        int i = 0;
                        foreach (var item in jsonObj)
                        {
                            childs.Add(new Node(i, this.layer + 1, item.Key, item.Value));
                            i++;
                        }
                    }
                    else if (json.type==EJsonType.Array)
                    {
                        var jsonArr = json as JsonArray;
                        for (int i = 0; i < jsonArr.Count; i++)
                        {
                            childs.Add(new Node(i, this.layer + 1, jsonArr[i]));
                        }
                    }
                    else if (json.type==EJsonType.Number)
                    {
                    }
                    else
                    {
                    }
                    this.value = json;
                }
                public Node(int index,int layer,string key, IJsonNode json)
                {
                    jsonType = EJsonType.Object;
                    this.key = key;
                    this.index = index;
                    this.layer = layer;
                    if (json.type == EJsonType.Object)
                    {
                        var jsonObj = json as JsonObject;
                        int i = 0;
                        foreach (var item in jsonObj)
                        {
                            childs.Add(new Node(i, this.layer + 1, item.Key, item.Value));
                            i++;
                        }
                    }
                    else if (json.type == EJsonType.Array)
                    {
                        var jsonArr = json as JsonArray;
                        for (int i = 0; i < jsonArr.Count; i++)
                        {
                            childs.Add(new Node(i, this.layer + 1, jsonArr[i]));
                        }
                    }
                    else if (json.type == EJsonType.Number)
                    {
                    }
                    else
                    {
                    }
                    this.value = json;
                }
                public void DisplayContent()
                {
                    var color = GUI.color;
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUI.color = Color.grey;
                        if (GUILayout.Button("※", EditorStyles.largeLabel, GUILayout.Width(20)))
                        {

                        }
                        if (GUILayout.Button("☑", EditorStyles.largeLabel, GUILayout.Width(20)))
                        {

                        }
                        GUI.color = color;
                        if (jsonType == EJsonType.Object || value.type == EJsonType.Array)
                        {
                            if (value.type == EJsonType.Object || value.type == EJsonType.Array)
                            {
                                GUILayout.Space(layer * 20);
                                string content = open ? "▲" : "▼";
                                if (GUILayout.Button(content,EditorStyles.largeLabel, GUILayout.Width(20)))
                                {
                                    open = !open;
                                }
                                if (string.IsNullOrEmpty(key))
                                {
                                    GUI.color = Color.grey;
                                    EditorGUILayout.LabelField(index + "  {" + childs.Count + "}");
                                    GUI.color = color;
                                }
                                else
                                {
                                    EditorGUILayout.LabelField(key);
                                }
                            }
                            else
                            {
                                GUILayout.Space((layer + 1) * 20);
                                GUILayout.Label(key + " : ");
                                var va = value.AsString();
                                GUILayout.Label(va);
                                if (value.AsString() != va)
                                {
                                    value = new JsonString(va);
                                }
                            }
                        }
                        else if (jsonType==EJsonType.Number)
                        {
                            GUILayout.Space((layer + 1) * 20);
                            EditorGUILayout.LabelField(key + " : ");
                            var va = (float)value.AsDouble();
                            va=EditorGUILayout.FloatField(va);
                            if ((float)value.AsDouble()!=va)
                            {
                                value = new JsonNumber(va);
                            }
                        }
                        else
                        {
                            GUILayout.Space((layer + 1) * 20);
                            EditorGUILayout.LabelField(key + " : ");
                            var va = value.AsString();
                            va=EditorGUILayout.TextField(va);
                            if (value.AsString() != va)
                            {
                                value = new JsonString(va);
                            }
                        }
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                    if (open)
                    {
                        for (int i = 0; i < childs.Count; i++)
                        {
                            childs[i].DisplayContent();
                        }
                    }
                }
                private bool IsMouseOn(Rect rect)
                {
                    return rect.Contains(Event.current.mousePosition);
                }


            }
            private Node m_jsonTree;
            private ConfigFile m_configFile;
            private string m_text;
            public JsonContent(ConfigFile config) : base(config)
            {
                var text = File.ReadAllText(config.path);
                var json = lhJson.Parse(text);
                m_text = text;
                m_configFile = config;
                m_jsonTree = new Node(0,0,json);
            }
            public override void DisplayContent()
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    {
                        m_text=EditorGUILayout.TextArea(m_text,GUILayout.Width(400), GUILayout.Height(500));
                    }EditorGUILayout.EndVertical();
                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginVertical();
                    {
                        if (GUILayout.Button("☞"))
                        {

                        }
                        if (GUILayout.Button("☜"))
                        {

                        }
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.Separator();
                    EditorGUILayout.BeginVertical(EditorStyles.textArea,GUILayout.MinHeight(500));
                    {
                        m_jsonTree.DisplayContent();
                    }EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
                base.DisplayContent();
            }
        }
        private class XmlContent: ConfigContent
        {
            public XmlContent(ConfigFile config) : base(config)
            {

            }
            public override void DisplayContent()
            {
                base.DisplayContent();
            }
        }
        private string m_configFolder=Application.dataPath+"/Resources/ConfigData/";
        private List<ConfigFile> m_configFilList;
        private Dictionary<ConfigFile, ConfigContent> m_configContentDic;
        private ConfigContent m_curConfigContent;
        private int m_doubleClick;
        public lhConfigManager()
        {
            m_configFilList = new List<ConfigFile>();
            m_configContentDic = new Dictionary<ConfigFile, ConfigContent>();
            Refresh();
        }
        public void Refresh()
        {
            m_configFilList.Clear();
            string[] files = Directory.GetFiles(m_configFolder, "*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var info = new FileInfo(file);
                string extension = info.Extension;
                if (extension.ToLower().Equals(".meta")) continue;
                else
                {
                    var type = FileType.Csv;
                    if (extension.ToLower().Equals(".csv"))
                    {
                        type = FileType.Csv;
                    }
                    else if (extension.ToLower().Equals(".xml"))
                    {
                        type = FileType.Xml;
                    }
                    else if (extension.ToLower().Equals(".json"))
                    {
                        type = FileType.Json;
                    }
                    m_configFilList.Add(new ConfigFile()
                    {
                        fileType = type,
                        index=i,
                        fileName= info.Name,
                        path= m_configFolder+info.Name
                    });
                }
            }
        }
        public void DisplayMenu()
        {
            for (int i = 0; i < m_configFilList.Count; i++)
            {
                var config = m_configFilList[i];
                var rect = EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(config.fileName);
                    if (IsMouseOn(rect))
                    {
                        if (Event.current.type==EventType.MouseUp && Event.current.button == 1)
                        {
                            GenericMenu menu = new GenericMenu();
                            menu.AddItem(new GUIContent("Delete"), false, () => { });
                            menu.AddItem(new GUIContent("Rename"), false, () => { });
                            menu.AddItem(new GUIContent("Build cs"), false, () => { });
                            Vector2 mousePos = Event.current.mousePosition;
                            menu.DropDown(new Rect(mousePos.x, mousePos.y, 0, 0));
                        }
                        if (m_doubleClick == 0)
                        {
                            m_doubleClick = System.DateTime.Now.Millisecond;
                        }
                        else
                        {
                            if (System.DateTime.Now.Millisecond - m_doubleClick < 170)
                            {
                                m_doubleClick = 0;
                                OpenNewContent(config);
                                lhConfigWindow.Refresh();
                            }
                        }
                    }
                } EditorGUILayout.EndHorizontal();
            }
        }
        public void DisplayToolbar()
        {
            foreach (var item in m_configContentDic)
            {
                try
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(item.Key.fileName, GUILayout.Width(100));
                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {

                        }
                    }EditorGUILayout.EndHorizontal();
                }
                catch { }
            }
        }
        public void DisplayContent()
        {
            //try
            // {
                if (m_curConfigContent!=null)
                {
                    m_curConfigContent.DisplayContent();
                }
            //}
            //catch { }
        }
        private void OpenNewContent(ConfigFile file)
        {
            if (m_configContentDic.ContainsKey(file))
            {
                m_curConfigContent = m_configContentDic[file];
            }
            else
            {
                ConfigContent content = null;
                if (file.fileType == FileType.Csv)
                {
                    content = new CsvContent(file);
                }
                else if (file.fileType == FileType.Json)
                {
                    content = new JsonContent(file);
                }
                else if (file.fileType == FileType.Xml)
                {
                    content = new XmlContent(file);
                }
                else
                    return;
                m_configContentDic.Add(file, content);
                m_curConfigContent = content;
            }
        }
        private bool IsMouseOn(Rect rect)
        {
            return rect.Contains(Event.current.mousePosition);
        }
    }
}
