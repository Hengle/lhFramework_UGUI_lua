using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using com.ynrd.dota.msg.cmd;
using LaoHan.Infrastruture;
using com.ynrd.dota.msg.login;

namespace LaoHan.Network
{
    public class lhNetwork
    {
        private lhSocketConnect m_socketConnect;
        //private string m_ip = "218.244.148.179";
        ////private string m_ip = "192.168.0.99";
        //private int m_port = 4000;
        
        private Dictionary<EGameCmd, Action<object>> m_cmdToEvent;//cmd to eventhandler

        public static lhNetwork m_instance;
        public static lhNetwork GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhNetwork();
        }
        lhNetwork()
        {
            m_cmdToEvent = new Dictionary<EGameCmd, Action<object>>();
            m_socketConnect = lhSocketConnect.GetInstance();
        }
        public void Dispose()
        {
            if (m_socketConnect != null)
            {
                m_socketConnect.Close();
                m_socketConnect = null;
            }
            m_instance = null;
        }

        public static void RegisterProtobuf<T>(EGameCmd cmd,Action<object> handler)
        {
            m_instance.m_socketConnect.RegisterCmdClass(cmd, typeof(T));
            if (m_instance.m_cmdToEvent.ContainsKey(cmd))
            {
                m_instance.m_cmdToEvent[cmd] += handler;
            }
            else
                m_instance.m_cmdToEvent.Add(cmd, handler);
        }
        public static void RemoveProtobuf(EGameCmd cmd,Action<object> handler)
        {
            m_instance.m_socketConnect.RemoveCmdCalss(cmd);
            if (m_instance.m_cmdToEvent.ContainsKey(cmd))
            {
                m_instance.m_cmdToEvent[cmd] -= handler;
            }
            else
            {
                lhDebug.LogWarning("LaoHan: cmdToEvent dont contains this cmd:  " + cmd);
            }
        }
        public static void SendMessage(EGameCmd cmd, object obj)
        {
            m_instance.m_socketConnect.SendMessage(cmd, obj);
        }
        public static void ConnectSocket(string ip,int port)
        {
            m_instance.m_socketConnect.Connect(ip, port);
        }
        public static void CloseSocket()
        {
            m_instance.m_socketConnect.Close();
        }
        public static void Disconnect(bool reuse)
        {
            m_instance.m_socketConnect.Disconnect(reuse);
        }
        public void Update()
        {
            if (m_socketConnect == null || !m_socketConnect.connected) return;
            Dictionary<EGameCmd, object> messageList = m_socketConnect.GetMessage();
            foreach (KeyValuePair<EGameCmd, object> messgae in messageList)
            {
                Action<object> handler = m_cmdToEvent[messgae.Key];
                if (handler != null)
                    handler(messgae.Value);
            }
        }
    }
}
