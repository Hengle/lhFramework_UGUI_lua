using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LaoHan.Infrastruture;
using LaoHan.Control;

namespace LaoHan.Battle
{
    public class lhSendManager
    {
        #region private member
        private Queue<object> m_sendQueue = new Queue<object>();
        private float m_sendInterval = 0.1f;
        #endregion

        #region Construture
        private static lhSendManager m_instance;
        public static lhSendManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhSendManager();
        }
        lhSendManager()
        {
            lhCoroutine.StartCoroutine(ESendProtocol());
        }
        #endregion

        #region public static methods
        
        public static void SendProtocol<T>(T protocol,bool direct=false)
        {
            if (m_instance != null)
            {
                if (direct)
                    lhControlNetwork.SendTimelyProtobuf(protocol);
                else
                    m_instance.m_sendQueue.Enqueue(protocol);
            }
        }
        #endregion

        #region public meth0dos
        public void Dispose()
        {
            m_instance = null;
        }
        #endregion

        #region IEnumerator
        IEnumerator ESendProtocol()
        {
            while (true)
            {
                yield return new lhWaitForSeconds(m_sendInterval);
                for (int i = 0; i < m_sendQueue.Count; i++)
                {
                    lhControlNetwork.SendTimelyProtobuf(m_sendQueue.Dequeue());
                }
            }
        }
        #endregion
    }

}