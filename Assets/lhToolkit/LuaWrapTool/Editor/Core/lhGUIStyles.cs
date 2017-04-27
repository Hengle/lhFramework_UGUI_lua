using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LaoHan.Tools.AutoWrap
{
    public class lhGUIStyles
    {
        Dictionary<string, GUIStyle> m_styleDict = new Dictionary<string, GUIStyle>();
        Dictionary<string, Texture2D> m_iconDict = new Dictionary<string, Texture2D>();

        private static lhGUIStyles m_instance = null;
        public static lhGUIStyles GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance=new lhGUIStyles();
        }
        public void Dispose()
        {
            m_instance = null;
        }
        lhGUIStyles()
        {
            m_styleDict = new Dictionary<string, GUIStyle>();
            m_iconDict = new Dictionary<string, Texture2D>();
            lhGUIStyleSet styleSet = (lhGUIStyleSet)AssetDatabase.LoadAssetAtPath("Assets/lhToolkit/LuaWrapTool/Sources/CustomStyles.asset", typeof(lhGUIStyleSet));
            
            var styles = EditorGUIUtility.isProSkin ? styleSet.styles : styleSet.freeStyles;
            foreach (GUIStyle style in styles)
            {
                if (m_styleDict.ContainsKey(style.name))
                    Debug.LogError("Duplicated GUIStyle " + style.name);
                else
                    m_styleDict.Add(style.name, style);
            }

            foreach (Texture2D icon in styleSet.icons)
            {
                if (m_iconDict.ContainsKey(icon.name))
                    Debug.LogError("Duplicated icon " + icon.name);
                else
                    m_iconDict.Add(icon.name, icon);
            }
        }
        public static GUIStyle GetStyle(string name)
        {
            if (!m_instance.m_styleDict.ContainsKey(name))
                m_instance.m_styleDict.Add(name, new GUIStyle(name));

            return m_instance.m_styleDict[name];
        }

        public static Texture2D GetIcon(string name)
        {
            if (!m_instance.m_iconDict.ContainsKey(name))
                return null;
            else
                return m_instance.m_iconDict[name];
        }
    }
}
