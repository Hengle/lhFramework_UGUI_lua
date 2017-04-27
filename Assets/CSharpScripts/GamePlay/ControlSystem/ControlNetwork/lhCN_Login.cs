using UnityEngine;
using System.Collections;
using LaoHan.Network;
using System;
using System.Net;
using LaoHan.Infrastruture;
//using comm.login;

namespace LaoHan.Control
{
    public partial class lhControlNetwork
    {
        /// <summary>
        /// 连接  Login服务器
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <param name="onConnectResult"></param>
        /// <param name="time">链接失败时循环链接次数</param>
        /// <returns></returns>
        public static bool ConnectLoginServer(IPEndPoint ipEndPoint, Action<bool> onConnectResult, int time = 3)
        {
            bool ret = false;
            if (time-- <= 0)
            {
                onConnectResult(false);
                return false;
            }
            //if (m_network.Connect(ServerType.LoginServer, ipEndPoint, onConnectResult))
            //{
            //    ret = true;
            //}
            //else
            //{
            //    ret = ConnectLoginServer(ipEndPoint, onConnectResult, time);
            //}
            return ret;
        }
        /// <summary>
        /// 登录 login服务器
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="gameType">游戏类型编号，目前一律添0 game</param>
        /// <param name="strict">游戏区编号 zone</param>
        /// <param name="netType">网关网络类型，0电信，1网通</param>
        public static void SendLogin(string account,ushort gameType,ushort strict,ushort netType)
        {
            //CmdMessage msgVersion = new CmdMessage();
            //msgVersion.messageID = lhMacro.loginServerVersion;
            //msgVersion.WriteUInt32(0);
            //m_network.SendCmd(msgVersion);
            
            //CmdMessage msg = new CmdMessage();
            //msg.messageID = lhMacro.loginServerData;
            //msg.WriteString(account, lhMacro.MAX_ACCNAMESIZE);
            //msg.WriteUInt16(gameType);
            //msg.WriteUInt16(strict);
            //msg.WriteUInt16(netType);      
            //m_network.SendCmd(msg);
        }
        /// <summary>
        /// 连接游戏服务器
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <param name="onConnectResult"></param>
        public static void ConnectGameServer(IPEndPoint ipEndPoint, Action<bool> onConnectResult)
        {
            //m_network.Connect(ServerType.GameServer, ipEndPoint, onConnectResult);
        }
        /// <summary>
        /// 登录游戏服务器
        /// </summary>
        /// <param name="pstrAccount"></param>
        /// <param name="loginTempID"></param>
        /// <param name="userType"></param>
        public static void SendGameLogin(string pstrAccount,uint loginTempID,uint userType)
        {
            //CmdMessage cmdMsg = new CmdMessage();
            //cmdMsg.messageID = lhMacro.userVerifyVerCmd;
            //cmdMsg.WriteUInt32(lhMacro.gameServerVersion);//version
            //m_network.SendCmd(cmdMsg);

            //LoginGateUserCmd userCmd = new LoginGateUserCmd();
            //userCmd.loginTempID = loginTempID;
            //userCmd.pstrAccount = pstrAccount;
            //userCmd.userType = userType;
            //m_network.SendProtobuf<LoginGateUserCmd>(userCmd);
        }
    }
}