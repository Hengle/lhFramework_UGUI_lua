using UnityEngine;
using System.Collections;
using System;
using LaoHan.UGUI;

namespace LaoHan.Control
{
    public partial class lhControlNetwork
    {
        /// <summary>
        /// 切换当前职业
        /// </summary>
        /// <param name="occupation">职业</param>
        /// <param name="onSendResult">发送后回调</param>
        //public static void Hall_ReqChangeOccupation(uint occupation)
        //{
        //    ReqChangeOccupation reqChangeOccupation = new ReqChangeOccupation();
        //    reqChangeOccupation.occupation = occupation;
        //    lhUIManager.ShowNetworkLoading(() =>
        //    {
        //        m_network.SendProtobuf<ReqChangeOccupation>(reqChangeOccupation);
        //    });
        //}

        ///// <summary>
        ///// 聊天消息, GM消息
        ///// </summary>
        ///// <param name="charId">角色Id</param>
        ///// <param name="content">聊天内容</param>
        ///// <param name="onSendResult">回调</param>
        //public static void Hall_ChatUserCmd(ulong charId, string content)
        //{
        //    ChatUserCmd chatUserCmd = new ChatUserCmd();
        //    chatUserCmd.fromid = charId;
        //    chatUserCmd.content = content;
        //    m_network.SendProtobuf<ChatUserCmd>(chatUserCmd);
        //}
    }
}