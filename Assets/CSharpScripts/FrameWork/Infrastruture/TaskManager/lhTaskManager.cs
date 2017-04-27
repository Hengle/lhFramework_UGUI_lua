using UnityEngine;
using System;
using System.Collections.Generic;

namespace LaoHan.Infrastruture
{
    public class lhTaskManager
    {
        private List<lhTransactionManager> m_transactionList = new List<lhTransactionManager>();
        private static lhTaskManager m_instance;
        public static lhTaskManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhTaskManager();
        }
        lhTaskManager()
        {

        }
        public void Dispose()
        {
            m_instance = null;
        }
        public void Update()
        {
            for (int i = 0; i < m_transactionList.Count; i++)
            {
                m_transactionList[i].Update();
            }
        }
        public static void AddTransaction(lhTransactionManager transaction)
        {
            m_instance.m_transactionList.Add(transaction);
        }
    }
}
