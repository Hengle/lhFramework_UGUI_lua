using UnityEngine;
using System.Collections;
namespace LaoHan.Infrastruture
{
    public class lhMacro
    {
        //--------------------------------------------------------------------------------------Network
        public const int MAX_NAMESIZE = 32;                         //昵称最大长度
        public const int MAX_ACCNAMESIZE = 48;                      //帐号最大长度
        public const int MAX_PASSWORD = 16;                         //密码最大长度
        public const int MAX_IP_LENGTH = 16;                        // IP地址最大长度
        public const int BUFFER_LENGTH = 1024 * 64;                 //消息byte数组最大长度
        public const int BUFFER_LENGTH_MINI = 1024 * 4;             //消息byte数组最大长度
        public const int MSGNAME_MAXLEN = 32;                       //昵称最大长度
        public const int MAX_PROTO_NAMESIZE = 48;                   // proto最大长度

        //----------------------------------------------------------------------------------------MessageId
        public const ushort loginServerVersion = 267;               //平台发送版本信息
        public const ushort loginServerData = 523;                   //平台发送玩家信息
        public const ushort loginServerReturnSuccessCmd = 1035;      //平台返回成功
        public const ushort loginServerFailCmd = 872;               //平台返回失败
        public const ushort userVerifyVerCmd = 268;                 //登录网关服务器发送版本验证
        public const ushort iphoneLoginUserCmd = 2009;               //登录网关服务器发送UUID账号和密码
        public const ushort gameServerTimeCmd = 258;                //网关向用户发送游戏时间
        public const ushort userGameTime = 770;                     //向网关发送心跳
        public const ushort protoMessage = 524;                     //protobuf消息
        public const ushort luaMessage = 1036;                 //protobuf_lua消息
        public const ushort gameServerVersion = 1999;               //游戏服务器版本信息
        public const ushort serverReturnLoginFailedCmd=780;         //服务器返回错误信息

    }
}
