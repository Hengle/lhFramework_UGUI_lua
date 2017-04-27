using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using LaoHan.Infrastruture;

namespace LaoHan.Tools.DefineManager
{
    public class lhDefineManager
    {

        public enum DefineTargetGroup
        {
            Standalone = 0,
            Android = 1,
            iPhone = 2
        }
        public class DefineData
        {
            public bool active { get; set; }
            public bool renaming { get; set; }
            public string defineName { get; set; }
            public string oldDefineName { get; set; }
            public int targetGroupValue { get; set; }
            public DefineData(bool active, string defineName)
            {
                this.active = active;
                this.defineName = defineName;
                this.oldDefineName = defineName;
                renaming = true;
                targetGroupValue = -1;
            }
            public DefineData(JsonObject json)
            {
                ToObject(json);
            }
            public JsonObject ToJson()
            {
                JsonObject json = new JsonObject();
                json["Active"] = new JsonString(active.ToString());
                json["DefineName"] = new JsonString(defineName);
                json["TargetGroupValue"] = new JsonNumber(targetGroupValue);
                return json;
            }
            private void ToObject(JsonObject json)
            {
                active = Convert.ToBoolean(json["Active"].AsString());
                defineName = json["DefineName"].AsString();
                targetGroupValue = json["TargetGroupValue"].AsInt();
            }
        }
        public static List<DefineData> defineList
        {
            get
            {
                if (m_instance == null)
                    m_instance = new lhDefineManager();
                return m_instance.m_defineList;
            }
        }
        private List<DefineData> m_defineList;
        private static lhDefineManager m_instance;
        private Dictionary<DefineTargetGroup, string> m_buildDic;
        private string m_definePath = Application.dataPath + "/lhToolkit/DefineManager/Sources/lhDefine.txt";
        public static lhDefineManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhDefineManager();
        }
        lhDefineManager()
        {
            m_defineList = new List<DefineData>();
            m_buildDic = new Dictionary<DefineTargetGroup, string>();
            List<DefineTargetGroup> list = new List<DefineTargetGroup>();
            string[] group = Enum.GetNames(typeof(DefineTargetGroup));
            foreach (var str in group)
            {
                m_buildDic.Add((DefineTargetGroup)Enum.Parse(typeof(DefineTargetGroup), str), "");
            }
            if (System.IO.File.Exists(m_definePath))
            {
                string defineStr = System.IO.File.ReadAllText(m_definePath, System.Text.Encoding.UTF8);
                if (!string.IsNullOrEmpty(defineStr))
                {
                    var json = lhJson.Parse(defineStr) as JsonArray;
                    foreach (var j in json)
                    {
                        m_defineList.Add(new DefineData((JsonObject)j));
                    }
                }
            }
        }
        public void Dispose()
        {
            m_instance = null;
        }
        public static void CreateDefine()
        {
            m_instance.m_defineList.Add(new DefineData(true, "NewDefine" + m_instance.m_defineList.Count));
        }
        public static void DeleteDefine(DefineData define)
        {
            m_instance.m_defineList.Remove(define);
        }
        public static void Apply()
        {
            m_instance.ClearBuildDic();
            foreach (var define in m_instance.m_defineList)
            {
                if (define.active)
                {
                    List<DefineTargetGroup> list;
                    if (define.targetGroupValue == -1)
                        list = m_instance.GetAllBuildTargetGroup();
                    else if (define.targetGroupValue == 0)
                        list = new List<DefineTargetGroup>();
                    else
                        list = m_instance.MaskValueToEnumList(define.targetGroupValue);
                    foreach (var l in list)
                    {
                        if (m_instance.m_buildDic.ContainsKey(l))
                        {
                            m_instance.m_buildDic[l] += define.defineName + ";";
                        }
                        else
                            m_instance.m_buildDic.Add(l, define.defineName + ";");
                    }
                }
            }
            JsonArray json = new JsonArray();
            foreach (var d in m_instance.m_buildDic)
            {
                string value = d.Value;
                if (value.Contains(";"))
                {
                    value = value.Substring(0, value.LastIndexOf(';'));
                }
                BuildTargetGroup buildTarget = (BuildTargetGroup)Enum.Parse(typeof(BuildTargetGroup), d.Key.ToString());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, value);
            }
            foreach (var d in m_instance.m_defineList)
            {
                json.Add(d.ToJson());
            }
            System.IO.File.WriteAllText(m_instance.m_definePath,lhJson.PrettyPrint( json.ToString()), System.Text.Encoding.UTF8);
            AssetDatabase.Refresh();
        }
        private List<DefineTargetGroup> MaskValueToEnumList(int value)
        {
            List<DefineTargetGroup> list = new List<DefineTargetGroup>();
            int length = DecimalToBinary(value).Length;
            for (int i = 0; i < length; i++)
            {
                int v = value >> i;
                if (v % 2 != 0)
                {
                    list.Add((DefineTargetGroup)i);
                }
            }
            return list;
        }
        private List<DefineTargetGroup> GetAllBuildTargetGroup()
        {
            List<DefineTargetGroup> list = new List<DefineTargetGroup>();
            string[] group = Enum.GetNames(typeof(DefineTargetGroup));
            foreach (var str in group)
            {
                list.Add((DefineTargetGroup)Enum.Parse(typeof(DefineTargetGroup), str));
            }
            return list;
        }
        private string DecimalToBinary(int num)
        {
            num = Mathf.Abs(num);
            if ((int)num / 2 == 0)
            {
                return (num % 2).ToString();
            }
            else
            {
                string result = DecimalToBinary((int)num / 2) + (num % 2).ToString();
                return result;
            }
        }
        private void ClearBuildDic()
        {
            List<DefineTargetGroup> list = new List<DefineTargetGroup>();
            foreach (var b in m_buildDic)
            {
                list.Add(b.Key);
            }
            for (int i = 0; i < list.Count; i++)
                m_buildDic[list[i]] = "";
        }
    }
}