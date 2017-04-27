using UnityEngine;
using System.Collections;

namespace LaoHan.Data
{
    public partial class lhMemoryData
    {
        private static lhMemoryData m_instance;
        public static lhMemoryData GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhMemoryData();
        }
    }
}