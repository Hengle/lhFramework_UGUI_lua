using System;
using System.Collections.Generic;
using FairyGUI;

namespace LaoHan.FairyGUI
{
    public class lhUIManager
    {
        private static lhUIManager m_instance;
        public static lhUIManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhUIManager();
        }
        lhUIManager()
        {

        }
        public void Dispose()
        {
            m_instance = null;
        }
        public void EnterUI()
        {

        }
        public void CloseUI()
        {

        }
    }
}
