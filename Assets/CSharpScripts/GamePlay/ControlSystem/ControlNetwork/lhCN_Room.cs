using UnityEngine;
using System.Collections;
using LaoHan.Network;
using System;
using LaoHan.UGUI;
using System.Collections.Generic;
using LaoHan.Data;

namespace LaoHan.Control
{
    public partial class lhControlNetwork
    {
        //#region 房间列表界面
        ///// <summary>
        ///// 请求指定线路的房间列表
        ///// </summary>
        ///// <param name="road">指定线路</param>
        ///// <param name="onSendResult">发送结果回调</param>
        //public static void RequestRoadRoomList(uint road, bool isRegisterCallback = true)
        //{
        //    if (isRegisterCallback)
        //        lhNetwork.RegisterProtobuf<RefreshRoadRoomList>(OnRefreshRoadRoomList);
        //    ReqRoadRoomList str = new ReqRoadRoomList();
        //    str.road = road;
        //    m_network.SendProtobuf<ReqRoadRoomList>(str);
        //}

        //public static void OnRefreshRoadRoomList(IProtocol protocol)
        //{
        //    lhNetwork.RemoveProtobuf<RefreshRoomData>(OnRefreshRoadRoomList);
        //    RefreshRoadRoomList roomList = protocol.GetProtobuf<RefreshRoadRoomList>(typeof(RefreshRoadRoomList));
        //    lhMemoryData.Battle.curRoadId = roomList.road;
        //    lhUIManager.EnterUI("UI_PKRoomList", (uiBase) =>
        //    {
        //        Dictionary<string, object> param = new Dictionary<string, object>();
        //        param.Add("RoomList", roomList);
        //        uiBase.Open(param, null);
        //    });
            
        //}
        //#endregion

        //#region 房间界面
        ///// <summary>
        ///// 创建房间
        ///// </summary>
        ///// <param name="road">指定线路</param>
        //public static void RequestCreateRoom(uint road)
        //{
        //    lhNetwork.RegisterProtobuf<RefreshRoomData>(OnRefreshRoomData);
        //    ReqCreateRoom str = new ReqCreateRoom();
        //    str.road = road;
        //    lhUIManager.ShowNetworkLoading(() =>
        //    {
        //        m_network.SendProtobuf<ReqCreateRoom>(str);
        //    });
        //}

        ///// <summary>
        ///// 加入指定房间
        ///// </summary>
        ///// <param name="road">指定线路</param>
        ///// <param name="roomId">房间ID</param>
        //public static void RequestJoinRoom(uint road, ulong roomId)
        //{
        //    lhNetwork.RegisterProtobuf<RefreshRoomData>(OnRefreshRoomData);
        //    ReqJoinRoom str = new ReqJoinRoom();
        //    str.road = road;
        //    str.roomid = roomId;
        //    lhUIManager.ShowNetworkLoading(() =>
        //    {
        //        m_network.SendProtobuf<ReqJoinRoom>(str);
        //    });
        //}

        //public static void OnRefreshRoomData(IProtocol protocol)
        //{
        //    RefreshRoomData data = protocol.GetProtobuf<RefreshRoomData>(typeof(RefreshRoomData));
        //    lhMemoryData.Battle.curRoomId = data.data.id;
        //    lhUIManager.EnterUI("UI_PKRoom", (uiBase) =>
        //    {
        //        Dictionary<string, object> param = new Dictionary<string, object>();
        //        param.Add("RefreshRoomData", data);
        //        uiBase.Open(param, null);
        //    });
        //    lhNetwork.RemoveProtobuf<RefreshRoomData>(OnRefreshRoomData);
        //}

        ///// <summary>
        ///// 开始战斗
        ///// </summary>
        ///// <param name="road">指定线路</param>
        ///// <param name="roomId">房间ID</param>
        ///// <param name="onSendResult">发送结果</param>
        //public static void RequestRoomBattle(uint road, ulong roomId)
        //{
        //    ReqRoomBattle str = new ReqRoomBattle();
        //    str.road = road;
        //    str.roomid = roomId;
        //    lhUIManager.ShowNetworkLoading(() =>
        //    {
        //        m_network.SendProtobuf<ReqRoomBattle>(str);
        //    });

        //}

        ///// <summary>
        ///// 准备
        ///// </summary>
        //public static void RequestIntoPrepare(uint road, ulong roomId, bool set)
        //{
        //    ReqIntoPrepare reqIntoPrepare = new ReqIntoPrepare();
        //    reqIntoPrepare.road = road;
        //    reqIntoPrepare.roomid = roomId;
        //    reqIntoPrepare.set = set;
        //    lhUIManager.ShowNetworkLoading(() =>
        //    {
        //        m_network.SendProtobuf<ReqIntoPrepare>(reqIntoPrepare);
        //    });

        //}

        ///// <summary>
        ///// 踢出玩家
        ///// </summary>
        //public static void PVPKickOutRoom(uint road, ulong roomId, ulong charId)
        //{
        //    KickOutRoom kickOutRoom = new KickOutRoom();
        //    kickOutRoom.road = road;
        //    kickOutRoom.roomid = roomId;
        //    kickOutRoom.charid = charId;
        //    lhUIManager.ShowNetworkLoading(() =>
        //    {
        //        m_network.SendProtobuf<KickOutRoom>(kickOutRoom);
        //    });

        //}

        ///// <summary>
        ///// 设定房间人数上限
        ///// </summary>
        //public static void SendReqSetRoomMaxUser(uint roadId, ulong roomId, uint maxUser, List<ulong> kick)
        //{
        //    ReqSetRoomMaxUser reqSetRoomMaxUser = new ReqSetRoomMaxUser();
        //    reqSetRoomMaxUser.road = roadId;
        //    reqSetRoomMaxUser.roomid = roomId;
        //    reqSetRoomMaxUser.maxuser = maxUser;
        //    if (kick == null)
        //        reqSetRoomMaxUser.kick.AddRange(new List<ulong>().ToArray());
        //    else
        //        reqSetRoomMaxUser.kick.AddRange(kick.ToArray());
        //    lhUIManager.ShowNetworkLoading(() =>
        //    {
        //        m_network.SendProtobuf<ReqSetRoomMaxUser>(reqSetRoomMaxUser);
        //    });
        //}

        ///// <summary>
        ///// 更换正营
        ///// </summary>
        //public static void SendReqChangeTeam(uint roadId, ulong roomId)
        //{
        //    ReqChangeTeam reqChangeTeam = new ReqChangeTeam();
        //    reqChangeTeam.road = roadId;
        //    reqChangeTeam.roomid = roomId;
        //    m_network.SendProtobuf<ReqChangeTeam>(reqChangeTeam);
        //}

        ///// <summary>
        ///// 修改房间名字
        ///// </summary>
        //public static void SendReqChangeRoomName(uint roadId, ulong roomId, string roomName)
        //{
        //    ReqChangeRoomName changeRoomName = new ReqChangeRoomName()
        //    {
        //        road = roadId,
        //        roomid = roomId,
        //        name = roomName
        //    };

        //    m_network.SendProtobuf<ReqChangeRoomName>(changeRoomName);
        //}

        ///// <summary>
        ///// 退出房间
        ///// </summary>
        ///// <param name="isReciveBackData">是否接受服务器返回数据，true--注册回调方法 false--不注册</param>
        //public static void RequestLeaveRoom(bool isReciveBackData)
        //{
        //    ReqLeaveRoom leaveRoom = new ReqLeaveRoom();
        //    if (isReciveBackData)
        //    {
        //        lhNetwork.RegisterProtobuf<RefreshRoadRoomList>(OnRefreshRoadRoomList);
        //        lhUIManager.ShowNetworkLoading(() =>
        //        {
        //            m_network.SendProtobuf<ReqLeaveRoom>(leaveRoom);
        //        });
        //    }
        //    else
        //        m_network.SendProtobuf<ReqLeaveRoom>(leaveRoom);
        //}

        ///// <summary>
        ///// 切换Pvp地图
        ///// </summary>
        //public static void SendReqChangePvpMap(uint mapId, uint roadId, ulong roomId)
        //{
        //    ReqChangePvpMap reqChangePvpMap = new ReqChangePvpMap();
        //    reqChangePvpMap.mapid = mapId;
        //    reqChangePvpMap.road = roadId;
        //    reqChangePvpMap.roomid = roomId;
        //    lhUIManager.ShowNetworkLoading(() =>
        //    {
        //        m_network.SendProtobuf<ReqChangePvpMap>(reqChangePvpMap);
        //    });
        //}
        //#endregion

        //#region 线路选择界面
        ///// <summary>
        ///// 请求线路
        ///// </summary>
        //public static void RequestRoad()
        //{
        //    lhNetwork.RegisterProtobuf<RetRoadList>(OnReturnRoadList);
        //    ReqRoad reqRoad = new ReqRoad();
        //    lhUIManager.ShowNetworkLoading(() =>
        //    {
        //        m_network.SendProtobuf<ReqRoad>(reqRoad);
        //    });

        //}

        //private static void OnReturnRoadList(IProtocol protocol)
        //{
        //    RetRoadList roadList = protocol.GetProtobuf<RetRoadList>(typeof(RetRoadList));
        //    lhUIManager.EnterUI("UI_PKRoadChoose", (uiBase) =>
        //    {
        //        Dictionary<string, object> param = new Dictionary<string, object>();
        //        param.Add("RoadList", roadList);
        //        uiBase.Open(param, null);
        //    });

        //    lhNetwork.RemoveProtobuf<RetRoadList>(OnReturnRoadList);
        //}
        //#endregion

        //#region PVP战斗场景中的界面
        ///// <summary>
        ///// 玩家复活
        ///// </summary>
        //public static void PVPRequestRespawn(ulong charId, ulong occupation)
        //{
        //    requestRespawn msg = new requestRespawn();
        //    msg.charid = charId;
        //    msg.occupation = occupation;
        //    lhControlNetwork.SendTimelyProtobuf(msg);
        //}

        ///// <summary>
        ///// PVP再来一局
        ///// </summary>
        //public static void SendReqAgainTeamPvp(uint road, ulong roomId)
        //{
        //    ReqAgainTeamPvp str = new ReqAgainTeamPvp();
        //    str.road = road;
        //    str.roomid = roomId;
        //    lhUIManager.ShowNetworkLoading(() =>
        //    {
        //        m_network.SendProtobuf<ReqAgainTeamPvp>(str);
        //    });

        //}

        ///// <summary>
        ///// 离开副本
        ///// </summary>
        //public static void SendReqLeaveMap()
        //{
        //    ReqLeaveMap leaveMap = new ReqLeaveMap();
        //    lhUIManager.ShowNetworkLoading(() =>
        //    {
        //        m_network.SendProtobuf<ReqLeaveMap>(leaveMap);
        //    });
        //}

        ///// <summary>
        ///// 当场景加载成功后，通知服务器
        ///// </summary>
        //public static void SendNotifyServerLoadSuc()
        //{
        //    NotifyServerLoadSuc loadSuc = new NotifyServerLoadSuc();
        //    m_network.SendProtobuf<NotifyServerLoadSuc>(loadSuc);
        //}

        ///// <summary>
        ///// 实时查看战斗信息
        ///// </summary>
        //public static void SendReqPvpBattleInfo()
        //{
        //    ReqPvpBattleInfo battleInfo = new ReqPvpBattleInfo();
        //    m_network.SendProtobuf<ReqPvpBattleInfo>(battleInfo);
        //}
        //#endregion
    }
}