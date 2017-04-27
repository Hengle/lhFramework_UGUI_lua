using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
//using WellFired;
using LaoHan.Battle;
using LaoHan.Network;

namespace LaoHan.Tools.WorldEditor
{
    public class lhTriggerCutScene : lhTriggerBase
    {
        public bool stopGame;
        void Start()
        {
            //lhSyncManager.cutSceneHandler[id] = OnTrigger;
        }
        void OnDestroy()
        { 
            //lhSyncManager.cutSceneHandler[id] =null;
        }
        //void OnTrigger(CutScene protocol)
        //{
        //}
    }
}