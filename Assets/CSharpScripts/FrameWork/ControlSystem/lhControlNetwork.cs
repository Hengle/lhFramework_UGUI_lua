using UnityEngine;
using System.Collections;
using LaoHan.Network;
namespace LaoHan.Control
{
    public partial class lhControlNetwork
    {
        private static lhNetwork m_network;
        private static lhControlNetwork m_instance;
        public static lhControlNetwork GetInstance(lhNetwork network)
        {
            if (m_instance != null) return null;
            m_network = network;
            return m_instance = new lhControlNetwork();
        }
        lhControlNetwork()
        {
        }
    }
}