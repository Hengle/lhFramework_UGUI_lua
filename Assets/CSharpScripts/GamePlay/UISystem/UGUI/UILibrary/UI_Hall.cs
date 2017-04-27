using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using LaoHan.UGUI;
using LaoHan.Infrastruture;
using UnityEngine.EventSystems;
using LaoHan.Control;
using LaoHan.Network;
using LaoHan.Data;

namespace LaoHan.UGUI
{
    public class UI_Hall : lhUIBase
    {
        public UITool_ListViewNormal occpationListView;
        public Text roleName;
        public Text roleLv;
        public InputField chatInput;

        private OccupationType curOccupation;

        #region public methods

        #endregion

        #region lhUIBase
        public override void Initialize(Action onInitialOver)
        {
            base.Initialize(onInitialOver);
            //CreateList();
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
            //lhNetwork.RemoveProtobuf<RefreshOccupation>(OnRefreshOccupation);
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
        #endregion

        #region Button Event
        #endregion

        #region Server Event
        //private void OnRefreshOccupation(IProtocol protocol)
        //{
        //    RefreshOccupation occupation = protocol.GetProtobuf<RefreshOccupation>(typeof(RefreshOccupation));
        //    lhMemoryData.Character.occupation = (OccupationType)occupation.curoccupation;
        //    lhUIManager.EnterUI("UI_PK", (uiBase) =>
        //    {
        //        uiBase.Open(null, null);
        //    });
        //}
        #endregion

        #region private methods

        #endregion


        #region ListView
        #endregion
    }
}