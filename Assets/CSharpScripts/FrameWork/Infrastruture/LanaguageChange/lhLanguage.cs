using UnityEngine;
using System;
using System.Collections.Generic;
using LaoHan.Data;

namespace LaoHan.Infrastruture
{
    public class lhLanguage
    {
        public static Action languageChangedEventHandler;
        public static IDictionary<string, string> languageTypeDictionary
        {
            get
            {
                return m_instance.m_languageLibrary["L_LanguageName"];
            }
        }


        public bool isInitialed { get; private set; }

        private Dictionary<string, IDictionary<string, string>> m_languageLibrary;
        private string m_currentType;
        private static lhLanguage m_instance;
        public static lhLanguage GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhLanguage();
        }
        lhLanguage()
        {
            m_languageLibrary = new Dictionary<string, IDictionary<string, string>>();
            foreach (var item in lhConfigData.languageConfig)
            {
                m_languageLibrary.Add(item["id"], item);
            }
            m_currentType = lhDefine.languageType;
            PrivateChangeLanguage(m_currentType);
        }
        public static void ChangeLanguage(string type)
        {
            if (string.Equals(m_instance.m_currentType, type)) return;
            m_instance.PrivateChangeLanguage(type);
            if (languageChangedEventHandler != null)
                languageChangedEventHandler();
        }
        public static string GetContent(string index)
        {
            if (m_instance == null) return null;
            if (!m_instance.m_languageLibrary.ContainsKey(index))
                return null;
            return m_instance.m_languageLibrary[index][m_instance.m_currentType].ToString();
        }
        private void PrivateChangeLanguage(string type)
        {
            m_currentType = type;
            lhLanguageComponent[] coms = GameObject.FindObjectsOfType<lhLanguageComponent>();
            for (int i = 0; i < coms.Length; i++)
                coms[i].ChangeLanguage();
        }
    }
}
