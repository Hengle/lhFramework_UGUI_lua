//LaoHan:   
//Json support : double, int, ushort, short, bool, long ,,dont support float
using UnityEngine;
using System.Reflection;
using System;
using LaoHan.Infrastruture;
using System.Collections.Generic;

namespace LaoHan.Data
{
    public enum ConfigType
    {
        None,
        Unity,
        Json,
        Xml
    }
    public class lhConfigDataAttribute : Attribute
    {
        public lhConfigDataAttribute(string filePath,ConfigType configType=ConfigType.Json, bool dynamicPath = true)
        {
            this.filePath = filePath;
            this.dynamicPath = dynamicPath;
            this.configType = configType;
        }
        public ConfigType configType { get; private set; }
        public string filePath { get; private set; }
        public bool dynamicPath { get; private set; }
    }
    public partial class lhConfigData
    {
        #region Construture
        private PropertyInfo[] propertyInfos;
        private static lhConfigData m_instance;
        public static lhConfigData GetInstance(Action onInstanceOver)
        {
            if (m_instance != null) return null;
            return m_instance = new lhConfigData(onInstanceOver);
        }
        public void Dispose()
        {
            //Important;Dispose configClass
            foreach (var info in propertyInfos)
            {
                var attribute = Attribute.GetCustomAttribute(info, typeof(lhConfigDataAttribute));
                if (attribute != null)
                {
                    info.SetValue(this, null, null);
                }
            }
            m_instance = null;
        }
        lhConfigData(Action onInstanceOver)
        {
            //Important;Deserialize configClass
            propertyInfos = GetType().GetProperties();
            List<PropertyInfo> propertyList = new List<PropertyInfo>();
            foreach (var info in propertyInfos)
            {
                var attribute = Attribute.GetCustomAttribute(info,typeof(lhConfigDataAttribute));
                if(attribute!=null)
                {
                    propertyList.Add(info);
                }
            }
            int count = propertyList.Count;
            Action InstanceOver = () =>
            {
                count--;
                if (count <= 0)
                    onInstanceOver();
            };
            foreach (var info in propertyList)
            {
                var attribute = Attribute.GetCustomAttribute(info, typeof(lhConfigDataAttribute));
                lhConfigDataAttribute attr = (lhConfigDataAttribute)attribute;
                string text = null;
                if (attr.configType == ConfigType.Json)
                {
                    if (attr.dynamicPath)
                    {
                        var fo = info;
                        lhResources.Load(attr.filePath, (o) => {
                            text = (o as TextAsset).text;
                            fo.SetValue(this, lhJson.Parse(fo.PropertyType, text), null);
                            InstanceOver();
                        });
                    }
                    else
                    {
                        text = (Resources.Load(attr.filePath, typeof(TextAsset)) as TextAsset).text;
                        info.SetValue(this, lhJson.Parse(info.PropertyType, text), null);
                        InstanceOver();
                    }
                }
                else if(attr.configType==ConfigType.Unity)
                {
                    if (attr.dynamicPath)
                    {
                        var fo = info;
                        lhResources.Load(attr.filePath, (o) =>
                        {
                            fo.SetValue(this, o, null);
                            //o.GetType().GetMethod("Initialize").Invoke(o, null);
                            InstanceOver();
                        });
                    }
                    else
                    {
                        var obj = Resources.Load(attr.filePath);
                        info.SetValue(this, obj, null);
                        InstanceOver();
                    }
                }
            }
        }
        #endregion

        #region private member
        //Important:set configProperty,must private set and public get;
        #endregion

        #region private static methods
        #endregion
    }
}