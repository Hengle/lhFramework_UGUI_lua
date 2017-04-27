using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace LaoHan.Infrastruture
{
    public class lhListen
    {
        private static lhListen m_instance;
        public static lhListen GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhListen();
        }
        private Dictionary<Enum, Action<object>> m_dic = new Dictionary<Enum, Action<object>>();
        public static void Register(Enum type,Action<object> callback)
        {
            if (m_instance.m_dic.ContainsKey(type))
            {
                m_instance.m_dic[type] += callback;
            }
            else
            {
                m_instance.m_dic.Add(type, callback);
            }
        }
        public static void Send(Enum type, object value, float delay = -1)
        {
            Action Sender = () =>
            {
                if (m_instance.m_dic.ContainsKey(type))
                    m_instance.m_dic[type](value);
                else
                    lhDebug.LogWarning((object)("LaoHan: this type is exists:" + type));
            };
            if (delay > 0)
                lhInvoke.Invoke(Sender, delay);
            else
                Sender();
        }
        public static void Receive(Enum type, Action<object> callback)
        {
            if (m_instance.m_dic.ContainsKey(type))
            {
                m_instance.m_dic[type] += callback;
            }
            else
                m_instance.m_dic.Add(type, callback);
        }
        public static void Remove(Enum type,Action<object> callback)
        {
            if (m_instance.m_dic.ContainsKey(type))
            {
                if (m_instance.m_dic[type]==null)
                {
                    m_instance.m_dic.Remove(type);
                }
                else
                {
                    m_instance.m_dic[type] -= callback;
                    if (m_instance.m_dic[type]==null)
                    {
                        m_instance.m_dic.Remove(type);
                    }
                }
            }
            else
            {
                lhDebug.LogWarning(("LaoHan: this type is dont exists:" + type));
            }
        }
    }

}