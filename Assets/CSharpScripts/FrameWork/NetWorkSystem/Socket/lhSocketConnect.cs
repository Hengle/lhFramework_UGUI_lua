using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using com.ynrd.dota.msg.cmd;

namespace LaoHan.Network
{
    public class lhSocketConnect
    {
        private Socket m_clientSocket;
        private SocketError m_socketError;
        private static lhSocketConnect m_instance;
        private long m_heartInterval=5;
        private int m_heartCount;
        private byte[] m_buffer;
        private int m_magic = 123456;
        private int m_version = 1;
        private Queue<Header> m_sendHeaderQueue;
        private Dictionary<EGameCmd, Type> m_cmdToClass;
        private Dictionary<EGameCmd, object> m_actuatorList;
        private Dictionary<EGameCmd, object> m_receiveList;
        private Queue<byte[]> m_sendList;
        private com.ynrd.dota.ProtobufSerializer m_protobufSerializer;
        private bool m_sending;
        private struct Header
        {
            public int magic;
            public int verision;
            public short cmd;
            public short bodyLength;
        }
        public bool connected
        {
            get
            {
                return m_clientSocket.Connected;
            }
        }
        public static lhSocketConnect GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhSocketConnect();
        }
        lhSocketConnect()
        {
            m_sendHeaderQueue = new Queue<Header>();
            m_receiveList = new Dictionary<EGameCmd, object>();
            m_actuatorList = new Dictionary<EGameCmd, object>();
            m_sendList = new Queue<byte[]>();
            m_protobufSerializer = new com.ynrd.dota.ProtobufSerializer();
            m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public Dictionary<EGameCmd, object> GetMessage()
        {
            m_actuatorList.Clear();
            m_actuatorList = CloneDictionary(m_receiveList) as Dictionary<EGameCmd, object>;
            m_receiveList.Clear();
            return m_actuatorList;
        }
        public void Connect(string ip, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            
            IAsyncResult asyncResult=m_clientSocket.BeginConnect(ipEndPoint,new AsyncCallback(OnConnectResult), m_clientSocket);
            asyncResult.AsyncWaitHandle.WaitOne(2000, true);
            if (asyncResult.IsCompleted)
            {
                m_buffer = new byte[4096];
                m_clientSocket.BeginReceive(m_buffer, 0, m_buffer.Length, SocketFlags.None, out m_socketError, new AsyncCallback(OnReceiveResult), m_clientSocket);
            }
            else
            {
                m_socketError = SocketError.TimedOut;
            }
        }
        public void Disconnect(bool reuse)
        {
            if (m_clientSocket == null || !m_clientSocket.Connected) return;
            m_clientSocket.BeginDisconnect(reuse, new AsyncCallback(OnDisconnect), m_clientSocket);
        }
        public void Close()
        {
            if (m_clientSocket == null ) return;
            m_clientSocket.Close();
        }
        public void SendMessage(EGameCmd cmd,object obj)
        {
            byte[] bodyBytes = SerializeBytes(obj);
            int bodyLength = bodyBytes.Length;
            if (m_sendHeaderQueue.Count == 0) m_sendHeaderQueue.Enqueue(new Header());
            Header header = m_sendHeaderQueue.Dequeue();
            header.cmd = (short)cmd;
            header.magic = m_magic;
            header.verision = m_version;
            header.bodyLength = (short)bodyLength;
            byte[] headerBytes = StructToBytes(header);
            int headerLength = headerBytes.Length;
            byte[] endBytes = new byte[headerLength + bodyLength];
            Array.Copy(headerBytes, 0, endBytes, 0, headerLength);
            Array.Copy(bodyBytes, 0, endBytes, headerLength, bodyLength);
            if (m_sendList.Count == 0 && !m_sending)
                BeginSend(endBytes);
            else
                m_sendList.Enqueue(endBytes);
        }
        public void RegisterCmdClass(EGameCmd cmd,Type type)
        {
            if (!m_cmdToClass.ContainsKey(cmd))
                m_cmdToClass.Add(cmd, type);
        }
        public void RemoveCmdCalss(EGameCmd cmd)
        {
            if (m_cmdToClass.ContainsKey(cmd))
                m_cmdToClass.Remove(cmd);
        }
        private void OnConnectResult(IAsyncResult result)
        {
            var socket = result.AsyncState as Socket;
            socket.EndConnect(result);
        }
        private void OnDisconnect(IAsyncResult result)
        {
            var socket = result.AsyncState as Socket;
            socket.EndDisconnect(result);
        }
        private void OnReceiveResult(IAsyncResult result)
        {
            var socket = result.AsyncState as Socket;
            int receiveLength = socket.EndReceive(result);
            if (receiveLength <= 0)
            {
                socket.Close();
                return;
            }
            SplitPackage(m_buffer, 0);
            Array.Clear(m_buffer, 0, m_buffer.Length);
            m_clientSocket.BeginReceive(m_buffer, 0, m_buffer.Length, SocketFlags.None, out m_socketError, new AsyncCallback(OnReceiveResult), socket);
        }
        private void OnSendResult(IAsyncResult result)
        {
            m_sending = false;
            var socket = result.AsyncState as Socket;
            int length = socket.EndSend(result);
            if (length < 0)
            {
                Debug.Log("LaoHan: SendMessage is Failed!");
                return;
            }
            if (m_sendList.Count!=0)
            {
                BeginSend(m_sendList.Dequeue());
            }
        }
        private void BeginSend(byte[] bytes)
        {
            m_sending = true;
            var asyncResult = m_clientSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(OnSendResult), m_clientSocket);
            asyncResult.AsyncWaitHandle.WaitOne(2000, true);
            if (!asyncResult.IsCompleted)
            {
                m_socketError = SocketError.TimedOut;
            }
        }
        private void SplitPackage(byte[] bytes, int index)
        {
            byte[] headBytes = new byte[12];
            while (true)
            {
                if (index >= bytes.Length) break;
                Array.Copy(bytes, index, headBytes, 0, headBytes.Length);
                Header header = (Header)BytesToStruct(headBytes, typeof(Header));
                if (!CheakHeader(header)) break;
                int bodyLength = header.bodyLength;
                index += headBytes.Length;
                byte[] bodyBytes = new byte[bodyLength];
                Array.Copy(bytes, index, bodyBytes, 0, bodyLength);
                //--------------------------------------
                ImportReceiveDictionary(header.cmd, bodyBytes);
                //--------------------------------------
                index += bodyLength;
            }
        }
        /// <summary>
        /// Byte transfer structure
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="structType"></param>
        /// <returns></returns>
        private object BytesToStruct(byte[] bytes, Type structType)
        {
            int size = Marshal.SizeOf(structType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure(buffer, structType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        /// <summary>
        /// Structure to byte
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Byte[] StructToBytes(object obj)
        {
            int size = Marshal.SizeOf(obj);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(obj, buffer, true);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        private bool CheakHeader(Header header)
        {
            if (header.magic != m_magic)
            {
                //Debug.Log("LaoHan: header.magic is error!");
                return false;
            }
            if (header.verision != m_version)
            {
                //Debug.Log("LaoHan: header version is error!");
                return false;
            }
            return true;
        }
        private void ImportReceiveDictionary(short cmd, byte[] bytes)
        {
            EGameCmd gameCmd = (EGameCmd)cmd;//(EGameCmd)Enum.Parse(typeof(EGameCmd), cmd.ToString());
            object obj = DeserializeBytes(m_cmdToClass[gameCmd], bytes);
            m_receiveList.Add(gameCmd, obj);
        }
        private byte[] SerializeBytes(object obj)
        {
            byte[] bytes;
            using (MemoryStream stream = new MemoryStream())
            {
                m_protobufSerializer.Serialize(stream, obj);
                stream.Position = 0;
                int length = (int)stream.Length;
                bytes = new byte[length];
                stream.Read(bytes, 0, length);
            }
            return bytes;
        }
        private object DeserializeBytes(Type type, byte[] bytes)
        {
            object obj = null;
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                obj = m_protobufSerializer.Deserialize(stream, null, type);
            }
            return obj;
        }
        private object CloneDictionary(object obj)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter Formatter =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone));
            object clonedObj = null;
            using (MemoryStream stream = new MemoryStream())
            {
                Formatter.Serialize(stream, obj);
                stream.Position = 0;
                clonedObj = Formatter.Deserialize(stream);
            }
            return clonedObj;
        }
    }
}