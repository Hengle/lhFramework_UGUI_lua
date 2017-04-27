using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LaoHan.Infrastruture
{
    public class lhCsv
    {
        public static object Parse(Type type,string csv)
        {
            Type[] arguments = type.GetGenericArguments();
            if (arguments.Length != 2)
            {
                lhDebug.LogError("LaoHan: lhCsv Parse arguments length=  " + arguments.Length);
                return null;
            }
            var makeme = typeof(Dictionary<,>).MakeGenericType(arguments);
            var dicObj = Activator.CreateInstance(makeme);
            MethodInfo method = dicObj.GetType().GetMethod("Add");

            string[] lineArray = csv.Split("\r"[0]);
            var array = new string[lineArray.Length][];

            for (int i = 0; i < lineArray.Length; i++)
            {
                var colArr= lineArray[i].Split(',');
                var key = colArr[0];
                var list = new List<string>();
                list.AddRange(colArr);
                object o = ToObject(arguments[1], list);
                method.Invoke(dicObj, new object[] { key, o });
            }
            return null;
        }
        public static string[][] Parse(string csv,char cha='\r')
        {
            string[] lineArray = csv.Split(cha);
            var array = new string[lineArray.Length][];

            for (int i = 0; i < lineArray.Length; i++)
            {
                array[i] = lineArray[i].Split(',');
            }
            return array;
        }
        static object ToObject(Type type, List<string> list)
        {
            object obj = Activator.CreateInstance(type);
            var propertyInfos = type.GetProperties();
            foreach (var info in propertyInfos)
            {
                if (!list.Contains(info.Name))
                    continue;
                Type propertyType = info.PropertyType;
                if (propertyType == typeof(System.Double))
                    info.SetValue(obj, Convert.ToDouble(list[list.IndexOf(info.Name)]), null);
                else if (propertyType == typeof(System.Single))
                    info.SetValue(obj, Convert.ToSingle(list[list.IndexOf(info.Name)]), null);
                else if (propertyType == typeof(System.UInt16))
                    info.SetValue(obj, Convert.ToUInt16(list[list.IndexOf(info.Name)]), null);
                else if (propertyType == typeof(System.Int16))
                    info.SetValue(obj, Convert.ToInt16(list[list.IndexOf(info.Name)]), null);
                else if (propertyType == typeof(System.Int32))
                    info.SetValue(obj, Convert.ToInt32(list[list.IndexOf(info.Name)]), null);
                else if (propertyType == typeof(System.Int64))
                    info.SetValue(obj, Convert.ToInt64(list[list.IndexOf(info.Name)]), null);
                else if (propertyType == typeof(System.String))
                    info.SetValue(obj, list[list.IndexOf(info.Name)], null);
                else if (propertyType == typeof(System.Boolean))
                    info.SetValue(obj, Convert.ToBoolean(list[list.IndexOf(info.Name)]), null);
                else
                {
                    lhDebug.LogError("LaoHan: dont has this propertyType:  " + propertyType);
                }
            }
            return obj;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
