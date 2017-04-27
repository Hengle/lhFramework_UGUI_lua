using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LaoHan.Infrastruture
{
    public class lhComponent
    {
        private static lhComponent m_instance;
        private Dictionary<GameObject, Dictionary<Type, Component>> m_componentLibrary;

        public static lhComponent GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhComponent();
        }
        private lhComponent()
        {
            m_componentLibrary = new Dictionary<GameObject, Dictionary<Type, Component>>();
        }
        public static Component GetComponent(GameObject obj, Type type)
        {
            return m_instance.m_componentLibrary[obj][type];
        }
        public static void AddComponent(GameObject obj, Type type, Component mono)
        {
            if (m_instance.m_componentLibrary.ContainsKey(obj))
            {
                if (m_instance.m_componentLibrary[obj].ContainsKey(type))
                {
                    Debug.LogError("LaoHan: type is exists!");
                    return;
                }
                else
                    m_instance.m_componentLibrary[obj].Add(type, mono);
            }
            else
            {
                Dictionary<Type, Component> comDic = new Dictionary<Type, Component>();
                comDic.Add(type, mono);
                m_instance.m_componentLibrary.Add(obj, comDic);
            }
        }
        public void Dispose()
        {
            m_componentLibrary = null;
        }
    }
}