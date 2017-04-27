using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System;
using LaoHan.Infrastruture;

namespace LaoHan.Tools.WorldEditor
{
    [CustomEditor(typeof(lhTriggerPropertyScriptable))]
    public class lhTriggerPropertyEditor : lhInpectorBase<lhTriggerPropertyScriptable>
    {
        private class Data
        {
            public object value;
            public XElement element;
            public string type;
            public Data(object value,XElement element)
            {
                this.value = value;
                this.element = element;
            }
        }
        private static Editor m_currentEditor;
        private static lhTriggerPropertyScriptable m_inspector;
        private static lhTriggerManager.Node m_currentNode;
        private static List<Data> values;
        private static IEnumerable<XElement> elements;
        void OnEnable()
        {
            m_currentEditor = this;
        }
        void OnDisable()
        {
            if (m_currentNode.content == null) return;
        }
        public static void Show(lhTriggerManager.Node node)
        {
            if (m_inspector != null)
                m_inspector = null;
            m_inspector = ScriptableObject.CreateInstance<lhTriggerPropertyScriptable>();
            m_inspector.hideFlags = UnityEngine.HideFlags.DontSave;
            m_inspector.name = node.name;
            m_currentNode = node;
            values = new List<Data>();
            if (node.content != null)
            {
                elements = node.content.Descendants("prop");
                foreach (var item in elements)
                {
                    var data = new Data(null, item);
                    if (item.Attribute("typeof")!=null)
                    {
                        data.type = item.Attribute("typeof").Value;
                    }
                    values.Add(data);
                }
            }
            else
            {
            }
            Selection.activeObject = m_inspector;
        }
        public override bool UseDefaultMargins()
        {
            return false;
        }
        public override void OnInspectorGUI()
        {
            if (m_currentNode.content == null) return;
            var props=m_currentNode.content.Elements("prop");
            foreach (var data in props)
            {
                string format = data.Attribute("format").Value;
                string name = data.Attribute("name").Value;
                if (format.ToLower().Equals("int"))
                {
                    if (string.IsNullOrEmpty(data.Value )) data.SetValue(0);
                    int value=Convert.ToInt32(data.Value);
                    value = EditorGUILayout.IntField(new GUIContent(name), value);
                    data.SetValue(value);
                }
                else if (format.ToLower().Equals("float"))
                {
                    if (string.IsNullOrEmpty(data.Value)) data.SetValue(0.0f);
                    float value = Convert.ToSingle(data.Value);
                    value = EditorGUILayout.FloatField(new GUIContent(name), value);
                    data.SetValue(value);
                }
                else if (format.ToLower().Equals("string"))
                {
                    if (string.IsNullOrEmpty(data.Value)) data.SetValue("");
                    data.Value = EditorGUILayout.TextField(new GUIContent(name), data.Value);
                }
                else if (format.ToLower().Equals("vector3"))
                {
                    if (string.IsNullOrEmpty(data.Value)) data.SetValue(Vector3.zero);
                    string da = data.Value.Replace("(", "").Replace(")", "");
                    Vector3 value = lhConvert.StringToVector3(da);
                    value = EditorGUILayout.Vector3Field(new GUIContent(name), value);
                    data.SetValue(lhConvert.Vector3ToString(value));
                }
                else if (format.ToLower().Equals("bool"))
                {
                    if (string.IsNullOrEmpty(data.Value)) data.SetValue(false);
                    bool value = Convert.ToBoolean(data.Value);
                    value = EditorGUILayout.Toggle(new GUIContent(name), value);
                    data.SetValue(value);
                }
                else if (format.ToLower().Equals("object"))
                {
                    UnityEngine.Object obj=null;
                    if (data.Value!=null)
                    {
                        obj = GameObject.Find(data.Value);
                    }
                    Type type= typeof(GameObject);
                    var attr = data.Attribute("component");
                    if (attr!=null)
                    {
                        type = Type.GetType(attr.Value);
                    }
                    obj = EditorGUILayout.ObjectField(new GUIContent(name), obj, type, true);
                    if(obj!=null)
                        data.SetValue(GetParentName((GameObject)obj));
                }
            }
        }
        private static string GetParentName(GameObject obj)
        {
            string str=obj.name;
            if (obj.transform.parent != null)
            {
                str = GetParentName(obj.transform.parent.gameObject) + "/" + str;
            }
            return str;
        }

    }
}