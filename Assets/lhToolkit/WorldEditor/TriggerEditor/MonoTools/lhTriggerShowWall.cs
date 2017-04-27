using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LaoHan.Battle;
using System;

namespace LaoHan.Tools.WorldEditor
{
    public class lhTriggerShowWall : lhTriggerBase
    {
        void Start()
        {
            //lhSyncManager.showWallHandler[id] = OnTrigger;
        }
        void OnDestroy()
        {
            //lhSyncManager.showWallHandler[id] = null;
        }
        //void OnTrigger(ShowWall protocol)
        //{
        //    gameObject.SetActive(Convert.ToBoolean(protocol.state));
        //}
    }
}