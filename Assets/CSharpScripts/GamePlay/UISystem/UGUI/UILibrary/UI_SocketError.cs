using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Sockets;
using LaoHan.Network;
using LaoHan.Infrastruture;

namespace LaoHan.UGUI
{
    public class UI_SocketError : lhUIBase
    {

        #region public methods
        #endregion

        #region private methods
        #endregion

        #region lhUIBase
        public override void Initialize(Action onInitialOver)
        {
            base.Initialize(onInitialOver);

            //lhNetwork.socketErrorHandler += OnSocketError;
            //lhNetwork.receivedProtoHandler += OnReceivedProtocol;
            //lhNetwork.receivedCmdHandler += OnReceivedCmd;
            //lhNetwork.RegisterCmd(lhMacro.serverReturnLoginFailedCmd, OnServerError);
            //lhNetwork.RegisterProtobuf<ServerChatUserCmd>(OnServerChatUserError);

            onInitialOver();
        }
        public override void Open(Intent parameter, Action<Intent> onOpenOver)
        {
            base.Open(parameter, onOpenOver);
            gameObject.SetActive(true);
            if (onOpenOver != null)
                onOpenOver(null);
        }
        public override void Close(Action<Intent> onCloseOver)
        {
            gameObject.SetActive(false);
            base.Close(onCloseOver);
            if (onCloseOver != null)
                onCloseOver(null);
        }
        public override void Destroy(Action<Intent> onDestoryOver)
        {
            base.Destroy(onDestoryOver);
            if (onDestoryOver != null)
                onDestoryOver(null);
        }
        public override void ReceiveMessage(object mark, object value)
        {
        }
        #endregion

        #region private methods
        private void OnSocketError(SocketError errorCode)
        {
            //rectTransform.SetAsFirstSibling();
            EnterUtility_Dialog("Socket Error", errorCode.ToString(), "Ok");
            lhUIManager.HideNetworkLoading();
        }
        //private void OnServerError(ICmd protocol)
        //{
        //    //rectTransform.SetAsFirstSibling();
        //    CmdMessage bytMsg = (CmdMessage)protocol;
        //    uint returnCode = bytMsg.ReadUInt32();
        //    EnterUtility_Dialog("Server Error", "Server:" + returnCode.ToString(), "Ok");
        //    lhUIManager.HideNetworkLoading();
        //}
        //private void OnServerChatUserError(IProtocol protocol)
        //{
        //    ServerChatUserCmd msg = protocol.GetProtobuf<ServerChatUserCmd>(typeof(ServerChatUserCmd));
        //    EnterUtility_Dialog("ServerChatUserError:" + msg.infotype, msg.content, "Ok");
        //    lhUIManager.HideNetworkLoading();
        //}
        //private void OnReceivedProtocol(IProtocol protocol)
        //{
        //    lhUIManager.HideNetworkLoading();
        //}
        //private void OnReceivedCmd(ICmd cmd)
        //{
        //    lhUIManager.HideNetworkLoading();
        //}
        #endregion
    }
}