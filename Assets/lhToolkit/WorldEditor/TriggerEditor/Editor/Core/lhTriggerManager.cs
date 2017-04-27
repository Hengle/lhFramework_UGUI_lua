using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using LaoHan.Infrastruture;
using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

namespace LaoHan.Tools.WorldEditor
{
    public class lhTriggerManager
    {
        public enum NodeType
        {
            None=0,
            Event=1,
            Condition=2,
            Behaviour=3,
            And=4,
            Or=5,
            Not=6
        }
        public enum NodeClassify
        {
            None,
            Event=1,
            Condition=2,
            Behaviour=3
        }
        public class Node
        {
            public string name { get; set; }
            public NodeType nodeType { get; set; }
            public NodeClassify nodeClassify { get; set; }
            public Node parent { get; private set; }
            public List<Node> childList { get; private set; }
            public int layer { get; set; }
            public int index { get; set; }
            public bool isSelected { get; set; }

            public Action<Node> deleteHandler;
            public IEnumerable<XElement> menulist { get; set; }
            public XElement content { get; set; }
            public string triggerName { get; set; }
            public Node(string triggerName, Node parent,NodeClassify nodeClassify,NodeType nodeType, int layer, int index, Action<Node> onDeleteOwn)
            {
                this.triggerName = triggerName;
                this.nodeClassify = nodeClassify;
                this.name = nodeType.ToString();
                this.layer = layer;
                this.nodeType = nodeType;
                this.index = index;
                this.parent = parent;
                childList = new List<Node>();
                deleteHandler = onDeleteOwn;
            }
            public Node(XElement element,string triggerName,Node parent,Action<Node> onDeleteOwn,IEnumerable<XElement> menulist)
            {
                childList = new List<Node>();
                this.menulist = menulist;
                deleteHandler = onDeleteOwn;
                this.parent = parent;
                nodeClassify = (NodeClassify)Convert.ToInt32(element.Attribute("nodeClassify").Value);
                nodeType = (NodeType)Convert.ToInt32(element.Attribute("nodeType").Value);
                layer = Convert.ToInt32(element.Attribute("layer").Value);
                index = Convert.ToInt32(element.Attribute("index").Value);
                name = element.Attribute("name").Value;
                this.triggerName = triggerName;
                var eles = element.Elements();
                foreach (var item in eles)
                {
                    if(item.Name.LocalName.Equals("childs"))
                    {
                        foreach (var i in item.Elements())
                        {
                            childList.Add(new Node(i,triggerName, this, DeleteChild, menulist));
                        }
                    }
                    else{
                        var ele = (from e in menulist 
                                   where e.Attribute("name").Value == item.Attribute("name").Value && e.Attribute("classify").Value == item.Attribute("classify").Value 
                                   select e)
                                   .FirstOrDefault();
                        if (ele==null)
                        {
                            DeleteOwn();
                            return;
                        }
                        else{

                            List<XElement> remo = new List<XElement>();
                            foreach (var prop in item.Elements("prop"))
                            {
                                var p = (from e in ele.Elements("prop")
                                         where e.Attribute("name").Value == prop.Attribute("name").Value
                                         select e).FirstOrDefault();
                                if (p==null)
                                {
                                    remo.Add(prop);
                                }
                                else
                                {
                                    prop.Attribute("format").Value = p.Attribute("format").Value;
                                }
                            }
                            foreach (var r in remo)
                            {
                                r.Remove();
                            }
                            foreach (var prop in ele.Elements("prop"))
                            {
                                var p = (from e in item.Elements("prop")
                                         where e.Attribute("name").Value == prop.Attribute("name").Value
                                         select e).FirstOrDefault();
                                if (p==null)
                                {
                                    item.Add(prop);
                                }
                            }
                            content = item;
                        }
                    }
                }
            }
            public void SetMenu(string filePath)
            {
                var root = XElement.Parse(File.ReadAllText(filePath));
                menulist = root.Elements("bean");
            }
            public virtual void AddChild(object obj)
            {
                XElement ele = (XElement)obj;
                var child = new Node(triggerName,this, nodeClassify, nodeType, layer + 1, childList.Count, DeleteChild);
                child.name = ele.Attribute("name").Value;
                if (ele.Attribute("component")!=null)
                {
                    string component=ele.Attribute("component").Value;
                    Type type=Type.GetType(component+", Assembly-CSharp");
                    UnityEngine.GameObject ro = UnityEngine.GameObject.Find("[Trigger]");
                    if (ro == null)
                    {
                        ro = new UnityEngine.GameObject("[Trigger]");
                    }
                    var triggerparent = UnityEngine.GameObject.Find("[Trigger]/" + triggerName);
                    if(triggerparent==null)
                    {
                        triggerparent = new UnityEngine.GameObject(triggerName);
                        triggerparent.transform.parent = ro.transform;
                    }
                    var parent = UnityEngine.GameObject.Find("[Trigger]/" + triggerName +"/"+ nodeClassify.ToString());
                    if (parent==null)
                    {
                        parent = new UnityEngine.GameObject(nodeClassify.ToString());
                        parent.transform.parent = triggerparent.transform;
                    }
                    UnityEngine.GameObject o = new UnityEngine.GameObject(child.name);
                    o.transform.parent = parent.transform;
                    o.AddComponent(type);
                }
                child.content = new XElement(ele.Name);
                foreach (var item in ele.Attributes())
                {
                    child.content.SetAttributeValue(item.Name, item.Value);
                }
                child.content.Add(ele.Elements());
                child.menulist = menulist;
                childList.Add(child);
            }
            public void AddChild(Node node)
            {
                node.parent = this;
                node.ChangeLayer(layer + 1);
                node.deleteHandler = DeleteChild;
                childList.Add(node);
                node.ChangeIndex(childList.IndexOf(node));
            }
            public void Copy()
            {
                Node newNode = new Node(triggerName,parent, nodeClassify, nodeType, layer, index + 1, parent.DeleteChild);
                newNode.menulist=menulist;
                newNode.content = new XElement(content.Name);
                foreach (var item in content.Attributes())
                {
                    newNode.content.SetAttributeValue(item.Name, item.Value);
                }
                newNode.content.Add(content.Elements());
                newNode.name = name;
                int currentNodeIndex = parent.childList.IndexOf(this);
                parent.childList.Insert(currentNodeIndex + 1, newNode);
                newNode.ChangeIndex(childList.IndexOf(newNode));
            }
            public void InsertChildDown(Node node)
            {
                node.parent = parent;
                node.ChangeLayer(layer);
                node.deleteHandler = parent.DeleteChild;
                int currentNodeIndex = parent.childList.IndexOf(this);
                parent.childList.Insert(currentNodeIndex + 1, node);
                node.ChangeIndex(childList.IndexOf(node));
            }
            public void InsertChildUp(Node node)
            {
                node.parent = parent;
                node.ChangeLayer(layer);
                node.deleteHandler = parent.DeleteChild;
                int currentNodeIndex = parent.childList.IndexOf(this);
                if (currentNodeIndex == 0)
                    parent.childList.Insert(0, node);
                else
                    parent.childList.Insert(currentNodeIndex , node);
                node.ChangeIndex(childList.IndexOf(node));
            }
            public void AddJudge(object obj)
            {
                var type=(NodeType)obj;
                var node = new Node(triggerName,this, nodeClassify, type, layer + 1, childList.Count, DeleteChild);
                node.name = type.ToString().ToLower();
                childList.Add(node);
                node.ChangeIndex(childList.IndexOf(node));
            }
            public void DeleteOwn()
            {
                deleteHandler(this);
            }
            public bool HasNode(Node node)
            {
                bool has = false;
                for (int i = 0; i < childList.Count; i++)
                {
                    if (childList[i] == node)
                        has = true;
                    else
                        has = childList[i].HasNode(node);
                }
                return has;
            }
            public bool HasSelected()
            {
                bool has = false;
                if (isSelected)
                    has = true;
                else
                {
                    for (int i = 0; i < childList.Count; i++)
                    {
                        has = childList[i].HasSelected();
                    }
                }
                return has;
            }
            public void ChangeLayer(int layer)
            {
                this.layer = layer;
                if (childList.Count != 0)
                {
                    for (int i = 0; i < childList.Count; i++)
                        childList[i].ChangeLayer(layer + 1);
                }
            }
            public void ChangeIndex(int index)
            {
                this.index = index;
            }
            public void ShowPing()
            {
                if (content == null) return;
                if(content.Attribute("component")!=null)
                {
                    Type type=Type.GetType(content.Attribute("component").Value+", Assembly-CSharp");
                    var objs=UnityEngine.Object.FindObjectsOfType(type);
                    var propId = (from e in content.Elements() where e.Attribute("name").Value.Equals("id") select e).FirstOrDefault();

                    foreach (var item in objs)
                    {
                        FieldInfo idInfo = item.GetType().GetField("id", BindingFlags.Public | BindingFlags.Instance);
                        var va=idInfo.GetValue(item);
                        if (va!=null && propId.Value.Equals(va.ToString()))
                        {
                            EditorGUIUtility.PingObject(item.GetInstanceID());
                        }
                    }
                }
            }
            public JsonObject ToJson()
            {
                var json = new JsonObject();
                return json;
            }
            public XElement ExportToXml()
            {
                List<XAttribute> attrs = new List<XAttribute>();
                if (content != null)
                {
                    var props = content.Elements("prop");
                    foreach (var item in props)
	                {
                        if (item.Attribute("format").Value.Equals("object"))
                        {
                            var obj=UnityEngine.GameObject.Find( item.Value);
                            if (obj!=null)
	                        {
                                string radius = "";
                                var rangePoint = obj.GetComponent<lhTriggerRangePoint>();
                                if (rangePoint != null)
                                {
                                    radius = "," + rangePoint.radius;
                                }
                                attrs.Add(new XAttribute(item.Attribute("name").Value, lhConvert.Vector3ToString(obj.transform.position) + radius));
	                        }
                        }
                        else
                            attrs.Add(new XAttribute(item.Attribute("name").Value, item.Value));
	                }
                }
                string eleName = name;
                if (parent==null)
                {
                    if (nodeClassify==NodeClassify.Event)
                    {
                        eleName = "event";
                    }
                    else if(nodeClassify==NodeClassify.Condition)
                    {
                        eleName="condition";
                    }
                    else if(nodeClassify==NodeClassify.Behaviour)
                    {
                        eleName="behaviour";
                    }
                }
                XElement ele = new XElement(eleName, attrs);
                for (int i = 0; i < childList.Count; i++)
                {
                    ele.Add(childList[i].ExportToXml());
                }
                return ele;
            }
            public XElement Save()
            {
                XAttribute[] attrs = new XAttribute[]{
                    new XAttribute("name",name),
                    new XAttribute("layer",layer),
                    new XAttribute("index",index),
                    new XAttribute("nodeClassify",(int)nodeClassify),
                    new XAttribute("nodeType",(int)nodeType)
                };
                XElement ele = new XElement(name, attrs);
                XElement childs = new XElement("childs");
                for (int i = 0; i < childList.Count; i++)
                {
                    childs.Add(childList[i].Save());
                }
                ele.Add(childs);
                ele.Add(content);
                return ele;
            }
            private void DeleteChild(Node node)
            {
                childList.Remove(node);
            }
        }
        public class Trigger
        {
            #region class
            #endregion

            #region property
            public string triggerName { get { return m_triggerName; } set { m_triggerName = value; } }
            public string oldGroupName { get; set; }
            public bool open { get; set; }
            public bool renaming { get; set; }
            public int index { get; set; }
            public List<Node> treeList { get; set; }
            #endregion

            #region private member
            private string m_triggerName = "";
            #endregion

            #region Ctor
            public Trigger(string triggerName,int id)
            {
                index = id;
                treeList = new List<Node>();
                Node eventNode = new Node(triggerName,null, NodeClassify.Event, NodeType.None, 0, id, null);
                eventNode.name = "事件";
                eventNode.SetMenu(UnityEngine.Application.dataPath + "/lhToolkit/WorldEditor/TriggerEditor/Sources/Xml/Event.xml");
                treeList.Add(eventNode);

                Node conditionNode = new Node(triggerName,null, NodeClassify.Condition, NodeType.None, 0, id, null);
                conditionNode.name = "条件";
                conditionNode.SetMenu(UnityEngine.Application.dataPath + "/lhToolkit/WorldEditor/TriggerEditor/Sources/Xml/Condition.xml");
                treeList.Add(conditionNode);

                Node behaviourNode = new Node(triggerName,null, NodeClassify.Behaviour, NodeType.None, 0, id, null);
                behaviourNode.name = "行为";
                behaviourNode.SetMenu(UnityEngine.Application.dataPath + "/lhToolkit/WorldEditor/TriggerEditor/Sources/Xml/Behaviour.xml");
                treeList.Add(behaviourNode);

                this.m_triggerName = triggerName;
                this.oldGroupName = triggerName;
                open = true;
                renaming = true;
            }
            public Trigger(XElement element)
            {
                treeList = new List<Node>();
                m_triggerName = element.Attribute("name").Value;
                oldGroupName = m_triggerName;
                index = Convert.ToInt32(element.Attribute("id").Value);
                open = Convert.ToBoolean(element.Attribute("open").Value);
                var elements=element.Elements();
                foreach (var item in elements)
                {
                    string filePath = null;
                    if (item.Attribute("nodeClassify").Value == ((int)NodeClassify.Event).ToString())
                    {
                        filePath=UnityEngine.Application.dataPath + "/lhToolkit/WorldEditor/TriggerEditor/Sources/Xml/Event.xml";
                    }
                    else if (item.Attribute("nodeClassify").Value == ((int)NodeClassify.Condition).ToString())
                    {
                        filePath = UnityEngine.Application.dataPath + "/lhToolkit/WorldEditor/TriggerEditor/Sources/Xml/Condition.xml";
                    }
                    else if (item.Attribute("nodeClassify").Value == ((int)NodeClassify.Behaviour).ToString())
                    {
                        filePath = UnityEngine.Application.dataPath + "/lhToolkit/WorldEditor/TriggerEditor/Sources/Xml/Behaviour.xml";
                    }
                    var root = XElement.Parse(File.ReadAllText(filePath));
                    treeList.Add(new Node(item,m_triggerName, null, null, root.Elements("bean")));
                }
            }
            #endregion

            #region public methods
            public JsonObject ToJson()
            {
                var json = new JsonObject();
                return json;
            }
            public XElement ExportToXml(int index)
            {
                XElement triggerEle = new XElement("trigger", new XAttribute("id", index));
                foreach (var item in treeList)
                {
                    triggerEle.Add(item.ExportToXml());
                }
                return triggerEle;
            }
            public XElement Save(int index)
            {
                var attrs = new XAttribute[]
                {
                    new XAttribute("id", index),
                    new XAttribute("name",triggerName),
                    new XAttribute("open",open)
                };
                XElement triggerEle = new XElement("trigger", attrs);
                foreach (var item in treeList)
                {
                    triggerEle.Add(item.Save());
                }
                return triggerEle;
            }
            public void StartRename()
            {
                renaming = true;
                oldGroupName = triggerName;
            }
            public void EndRename()
            {
                var triggerParent = UnityEngine.GameObject.Find("[Trigger]/" + m_triggerName);
                if (triggerParent!=null)
                {
                    triggerParent.name = oldGroupName;
                }
                renaming = false;
                triggerName = oldGroupName;
            }
            #endregion
        }

        #region public static property
        public List<Trigger> triggerList
        {
            get
            {
                return m_triggerlist;
            }
        }
        public static string xmlConfigPath
        {
            get
            {
                return m_instance.m_xmlConfigPath;
            }
            set
            {
                m_instance.m_xmlConfigPath = value;
            }
        }
        #endregion

        #region private member
        private List<Trigger> m_triggerlist;
        private string m_xmlConfigPath =UnityEngine.Application.dataPath;
        private string m_localSavePath = UnityEngine.Application.dataPath + "/lhToolkit/WorldEditor/TriggerEditor/Sources/localConfig.xml";
        #endregion

        private static lhTriggerManager m_instance;
        public static lhTriggerManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhTriggerManager();
        }
        lhTriggerManager()
        {
            m_triggerlist = new List<Trigger>();
            Initialize();
        }
        public void Dispose()
        {
            m_instance = null;
        }
        public void CreateGroup()
        {
            string groupName = "new group" + m_instance.m_triggerlist.Count;

            m_triggerlist.Add(new Trigger(groupName, m_triggerlist.Count));
        }
        public void DeleteGroup(Trigger group)
        {
            m_triggerlist.Remove(group);
        }
        public void Apply()
        {
            if(string.IsNullOrEmpty(EditorApplication.currentScene))
            {
                EditorUtility.DisplayDialog("Error", "Please save scene", "Ok");
                return;
            }
            FileInfo info = new FileInfo(EditorApplication.currentScene);
            string cureentSceneName = info.Name.Replace(info.Extension, "");

            XElement[] triggerArr = new XElement[m_triggerlist.Count];
            for (int i = 0; i < m_triggerlist.Count; i++)
            {
                triggerArr[i] = m_triggerlist[i].Save(i);
            }

            XElement root = XElement.Parse(File.ReadAllText(m_localSavePath));
            XElement sceneEle = null;
            try
            {
                sceneEle = root.Element(cureentSceneName);
            }
            catch { }
            if (sceneEle==null)
            {
                var ele = new XElement(cureentSceneName);
                ele.Add(triggerArr);
                root.Add(ele);
            }
            else
            {
                sceneEle.RemoveAll();
                sceneEle.Add(triggerArr);
            }
            root.Save(m_localSavePath);
            EditorUtility.DisplayDialog("Information", "Save success", "Ok");
        }
        public void Export()
        {
            ExportXml();
        }
        public void Revert()
        {
            m_triggerlist.Clear();
            Initialize();
            EditorUtility.DisplayDialog("Information", "Rever success", "Ok");
        }
        public void Clear()
        {
            triggerList.Clear();
        }
        public bool HasSameGroupName(Trigger group,string groupName)
        {
            bool has=false;
            foreach(var g in triggerList)
            {
                if(g.triggerName==groupName && g!=group)
                {
                    has = true;
                    break;
                }
            }
            return has;
        }
        private void Initialize()
        {
            if (File.Exists(m_localSavePath))
            {
                XElement root = XElement.Parse(File.ReadAllText(m_localSavePath));
                XElement sceneEle = null;
                try
                {
                    FileInfo info=new FileInfo(EditorApplication.currentScene);
                    string sceneName = info.Name.Replace(info.Extension, "");
                    sceneEle = root.Element(sceneName);
                }
                catch { }
                if (sceneEle != null && sceneEle.HasElements)
                {
                    var childs = sceneEle.Elements();
                    foreach (var item in childs)
                    {
                        m_triggerlist.Add(new Trigger(item));
                    }
                }
            }
        }
        private void ExportXml()
        {
            //XElement[] triggerArr=new XElement[m_triggerlist.Count];
            //for (int i = 0; i < m_triggerlist.Count; i++)
            //{
            //    triggerArr[i] = m_triggerlist[i].ExportToXml(i);
            //}
            //XElement root = new XElement("root", triggerArr);

            //FileInfo info = new FileInfo(EditorApplication.currentScene);
            //string currentSceneName = info.Name.Replace(info.Extension, "");
            //string file=m_xmlConfigPath.Replace("client/FPSClient/Assets", "");
            //file += "cehua/map/logics/triggerConfig_" + currentSceneName + ".xml";
            //if (!File.Exists(file))
            //    File.Create(file);
            //UnityEngine.Debug.Log(file);
            //root.Save(file);
            //EditorUtility.DisplayDialog("Information", "Save success", "Ok");
        }
    }
}