using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LaoHan.UGUI;
using System;
using UnityEngine.UI;
using LaoHan.Data;
using LaoHan.Control;
using System.Net;
using LaoHan.Infrastruture;
using LaoHan.Network;
namespace LaoHan.UGUI
{
    public class UI_ServerChoose : lhUIBase
    {
        #region public member

        public Text blockStateText;
        public Text serverStateText;
        public Text serverNameText;
        public RectTransform leftArrow;
        public RectTransform rightArrow;

        public GameObject uiServerList;
        public UITool_ListViewNormal serverListView;

        #endregion

        #region private member
        private string account;
        private string pwd;
        private int zoneIndex = 0;
        private int mapIndex = 0;
        private enum LoginType
        {
            None,
            Waiting,
            Success,
            Fail
        }
        private LoginType m_loginType;
        #endregion

        #region lhUIBase
        public override void Initialize(Action onInitialOver)
        {
            base.Initialize(onInitialOver);
            onInitialOver();
        }
        public override void Open(Intent parameter, Action<Intent> onOpenOver)
        {
            //lhNetwork.RegisterCmd(lhMacro.loginServerReturnSuccessCmd, LoginCallback);
            //lhNetwork.RegisterProtobuf<MapDataInfo>(LoginGameCallback);
            //lhNetwork.RegisterProtobuf<stDataCharacterMain>(OnDataCharacterMain);
            //lhNetwork.RegisterProtobuf<RefreshPassCopyList>(OnRefreshPassCopyList);
            //lhNetwork.RegisterProtobuf<RefreshCopyRewardList>(OnRefreshCopyRewardList);

            account = parameter.GetExtras("account").ToString();
            pwd = parameter.GetExtras("pwd").ToString();

            //MoveArrow(leftArrow);
            //MoveArrow(rightArrow);

            if (lhCacheData.HasHistoryValue("mapIndex"))
                mapIndex = int.Parse(lhCacheData.GetHistoryValue("mapIndex"));
            if (lhCacheData.HasHistoryValue("zoneIndex"))
                zoneIndex = int.Parse(lhCacheData.GetHistoryValue("zoneIndex"));

            serverNameText.text = lhConfigData.cdn.zone[zoneIndex].name;

            base.Open(parameter, onOpenOver);
            gameObject.SetActive(true);
            if (onOpenOver != null)
                onOpenOver(null);
        }
        public override void Close(Action<Intent> onCloseOver)
        {
            Dispose();

            gameObject.SetActive(false);
            base.Close(onCloseOver);
            if (onCloseOver != null)
                onCloseOver(null);
        }
        public override void Destroy(Action<Intent> onDestoryOver)
        {
            Dispose();
            base.Destroy(onDestoryOver);
            if (onDestoryOver != null)
                onDestoryOver(null);
        }
        #endregion

        #region private methods
        private void Dispose()
        {
            //lhNetwork.RemoveCmd(lhMacro.loginServerReturnSuccessCmd, LoginCallback);
            //lhNetwork.RemoveProtobuf<MapDataInfo>(LoginGameCallback);
            //lhNetwork.RemoveProtobuf<stDataCharacterMain>(OnDataCharacterMain);
            //lhNetwork.RemoveProtobuf<RefreshPassCopyList>(OnRefreshPassCopyList);
            //lhNetwork.RemoveProtobuf<RefreshCopyRewardList>(OnRefreshCopyRewardList);
        }

        public void MoveArrow(RectTransform arrow)
        {
            UnityEngine.Vector3 endPos = arrow.localPosition;
            if (arrow.gameObject == leftArrow.gameObject)
                arrow.localPosition += new UnityEngine.Vector3(-1f, 0);
            else if (arrow.gameObject == rightArrow.gameObject)
                arrow.localPosition += new UnityEngine.Vector3(1f, 0);

            //Tweener tweener = arrow.DOMoveX(endPos.x, 0.5f);
            //tweener.SetUpdate(true);
            //tweener.SetEase(Ease.Linear);
            //tweener.SetLoops(-1, LoopType.Yoyo);
            //tweener.OnComplete<Tweener> (()=>
            //{

            //});
        }
        #endregion

        #region Button event
        public void OnBtnChangeServerClick()
        {
            uiServerList.SetActive(true);
            serverListView.AddDSWithCreateItems(lhConfigData.cdn.zone.Count, OnCreateListViewItem, OnUpdateListViewItem, OnClickListViewItem);
        }

        public void OnBtnEnterGameClick()
        {
            lhUIManager.ShowNetworkLoading(null);
            lhControlNetwork.ConnectLoginServer(new IPEndPoint(IPAddress.Parse(lhConfigData.cdn.map[mapIndex].ip), lhConfigData.cdn.map[mapIndex].port), (result) =>
            {
                if (result)
                {
                    m_loginType = LoginType.Success;
                }
                else
                {
                    m_loginType = LoginType.Fail;
                }
            });
        }
        #endregion

        #region ServerEvent
        //private void LoginCallback(ICmd protocol)
        //{
        //    CmdMessage bytMsg = (CmdMessage)protocol;
        //    uint userId = bytMsg.ReadUInt32();
        //    string ip = bytMsg.ReadString(16);
        //    ushort port = bytMsg.ReadUInt16();
        //    lhControlNetwork.ConnectGameServer(new IPEndPoint(IPAddress.Parse(ip), port), (result) =>
        //    {
        //        if (result)
        //        {
        //            lhControlNetwork.SendGameLogin(account, userId, 0);
        //        }
        //        else
        //        {
        //            m_loginType = LoginType.Fail;
        //        }
        //    });
        //}
        //private void LoginGameCallback(IProtocol protocol)
        //{
        //    MapDataInfo cmd = protocol.GetProtobuf<MapDataInfo>(typeof(MapDataInfo));

        //    lhUIManager.EnterUI("UI_MainTemplate", (uiBase) =>
        //    {
        //        uiBase.Open(null, null);
        //    }, null, false);
        //    lhUIManager.EnterUI("UI_Hall", (uiBase) =>
        //    {
        //        uiBase.Open(null, (param) =>
        //        {
        //            lhUIManager.DestroyUI("UI_ServerChoose");
        //        });
        //    });
        //}

        //private void OnDataCharacterMain(IProtocol protocol)
        //{
        //    stDataCharacterMain charData = protocol.GetProtobuf<stDataCharacterMain>(typeof(stDataCharacterMain));

        //    //初始化角色数据
        //    lhMemoryData.Character.charId = charData.data.charid;
        //    lhMemoryData.Character.name = charData.data.name;
        //    lhMemoryData.Character.level = charData.data.level;
        //    lhMemoryData.Character.exp = charData.data.exp;
        //    lhMemoryData.Character.money = charData.data.money;
        //    lhMemoryData.Character.gold = charData.data.gold;
        //    lhMemoryData.Character.tilizhi = charData.data.tilizhi;
        //}

        //private void OnRefreshPassCopyList(IProtocol protocol)
        //{
        //    lhMemoryData.Stage.passStageList = protocol.GetProtobuf<RefreshPassCopyList>(typeof(RefreshPassCopyList));
        //}

        //private void OnRefreshCopyRewardList(IProtocol protocol)
        //{
        //    lhMemoryData.Stage.stageRewardList = protocol.GetProtobuf<RefreshCopyRewardList>(typeof(RefreshCopyRewardList));
        //}
        #endregion

        #region Unity methods
        void Update()
        {
            if (m_loginType == LoginType.Fail)
            {
                lhUIManager.HideNetworkLoading();
                EnterUtility_Dialog("Error", "Login Error", "Ok");
                m_loginType = LoginType.None;
            }
            else if(m_loginType==LoginType.Success)
            {
                lhControlNetwork.SendLogin(account, 1, lhConfigData.cdn.zone[zoneIndex].id, 0);
                m_loginType = LoginType.None;
            }
        }
        #endregion

        #region ListView
        public void OnCreateListViewItem(UITool_ListItem item)
        {
            ServerItemInfo info = new ServerItemInfo();
            info.zoneName = item.GetComponentInItem<Text>("Text");
            item.param = info;
        }

        public void OnUpdateListViewItem(UITool_ListItem item)
        {
            ServerItemInfo info = (ServerItemInfo)item.param;
            info.zoneName.text = lhConfigData.cdn.zone[item.itemIndex].name;
        }

        public void OnClickListViewItem(UITool_ListItem item)
        {
            uiServerList.SetActive(false);
            mapIndex = item.itemIndex; //UnityEngine.Random.Range(0, lhConfigData.cdn.map.Count);
            zoneIndex = item.itemIndex;

            lhCacheData.SetHistoryValue("mapIndex", mapIndex.ToString());
            lhCacheData.SetHistoryValue("zoneIndex", zoneIndex.ToString());

            //更新界面显示
            serverNameText.text = lhConfigData.cdn.zone[item.itemIndex].name;
        }

        struct ServerItemInfo
        {
            public Text zoneName;
        }
        #endregion
    } 
}
