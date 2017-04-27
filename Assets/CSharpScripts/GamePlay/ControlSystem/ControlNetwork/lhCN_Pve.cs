using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LaoHan.Control
{
    public partial class lhControlNetwork
    {
        //Pve闯关-----------------------------------------------------------
        /// <summary>
        /// 请求进入地图
        /// </summary>
        /// <param name="level">通关难度(0:正常 1:困难 2:绝望)</param>
        //public static void PassReqEnterMap(uint mapId, uint level)
        //{
        //    ReqEnterMap reqEnterMap = new ReqEnterMap();
        //    reqEnterMap.mapid = mapId;
        //    reqEnterMap.level = level;
        //    m_network.SendProtobuf<comm.map.ReqEnterMap>(reqEnterMap);
        //}

        ///// <summary>
        ///// 请求离开副本
        ///// </summary>
        //public static void PassReqLeaveMap()
        //{
        //    ReqLeaveMap leaveMap = new ReqLeaveMap();
        //    m_network.SendProtobuf<ReqLeaveMap>(leaveMap);
        //}

        ////Pve合作--------------------------------------------------------------
        //#region 房间界面
        ///// <summary>
        ///// 创建房间
        ///// </summary>
        ///// <param name="mapId"></param>
        ///// <param name="level">难度等级 0:普通 1:困难 2:变态</param>
        //public static void CooperateReqCreateMpveRoom(int mapId, int level)
        //{
        //    ReqCreateMpveRoom pveRoom = new ReqCreateMpveRoom();
        //    pveRoom.mapid = (uint)mapId;
        //    pveRoom.level = (uint)level;
        //    m_network.SendProtobuf<ReqCreateMpveRoom>(pveRoom);
        //}

        ///// <summary>
        ///// 快速匹配
        ///// </summary>
        //public static void CooperateReqQuickJoinRoom()
        //{
        //    ReqQuickJoinRoom quickJoinRoom = new ReqQuickJoinRoom();
        //    m_network.SendProtobuf<ReqQuickJoinRoom>(quickJoinRoom);
        //}

        ///// <summary>
        ///// 请求修改房间名
        ///// </summary>
        //public static void CooperateReqChangeMpveRoomName(ulong roomId, string roomName)
        //{
        //    ReqChangeMpveRoomName changeRoomName = new ReqChangeMpveRoomName();
        //    changeRoomName.id = roomId;
        //    changeRoomName.name = roomName;
        //    m_network.SendProtobuf<ReqChangeMpveRoomName>(changeRoomName);
        //}

        ///// <summary>
        ///// 切换地图
        ///// </summary>
        //public static void CooperateReqChangeMpveMap(ulong roomId, uint mapId)
        //{
        //    ReqChangeMpveMap changeMpveMap = new ReqChangeMpveMap();
        //    changeMpveMap.id = roomId;
        //    changeMpveMap.mapid = mapId;
        //    m_network.SendProtobuf<ReqChangeMpveMap>(changeMpveMap);
        //}

        ///// <summary>
        ///// 玩家准备
        ///// </summary>
        ///// <param name="roomId">房间Id</param>
        ///// <param name="set">true--准备 false--取消准备</param>
        //public static void CooperateReqIntoMpvePrepare(ulong roomId, bool set)
        //{
        //    ReqIntoMpvePrepare prepare = new ReqIntoMpvePrepare();
        //    prepare.id = roomId;
        //    prepare.set = set;
        //    m_network.SendProtobuf<ReqIntoMpvePrepare>(prepare);
        //}

        ///// <summary>
        ///// 开始游戏
        ///// </summary>
        //public static void CooperateReqMpveRoomBattle(ulong roomId)
        //{
        //    ReqMpveRoomBattle roomBattle = new ReqMpveRoomBattle();
        //    roomBattle.id = roomId;
        //    m_network.SendProtobuf<ReqMpveRoomBattle>(roomBattle);
        //}

        ///// <summary>
        ///// 退出房间
        ///// </summary>
        //public static void CooperateReqLeaveMpveRoom()
        //{
        //    ReqLeaveMpveRoom leaveRoom = new ReqLeaveMpveRoom();
        //    m_network.SendProtobuf<ReqLeaveMpveRoom>(leaveRoom);
        //}
        //#endregion
    }
}
