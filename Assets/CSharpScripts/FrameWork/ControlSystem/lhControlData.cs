using UnityEngine;
using System.Collections;
using LaoHan.Data;
namespace LaoHan.Control
{
    public partial class lhControlData
    {
        private static lhMemoryData m_memory;
        private static lhControlData m_instance;
        public static lhControlData GetInstance(lhMemoryData memoryData)
        {
            if (m_instance != null) return null;
            m_memory = memoryData;
            return m_instance = new lhControlData();
        }
        lhControlData() { }
        public void Dispose()
        {

        }
    }
}