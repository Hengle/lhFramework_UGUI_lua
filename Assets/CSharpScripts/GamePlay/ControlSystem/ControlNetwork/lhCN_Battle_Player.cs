using UnityEngine;
using System.Collections;
using LaoHan.Network;
using System;

namespace LaoHan.Control
{
    public partial class lhControlNetwork
    {
        public static void SendTimelyProtobuf<T>(T message)
        {
            //if (m_network != null)
            //    m_network.SendProtobuf<T>(message);
        }
    }
}