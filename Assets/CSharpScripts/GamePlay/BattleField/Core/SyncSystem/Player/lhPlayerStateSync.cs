using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
using LaoHan.Network;
//using comm.msg;
using System;
namespace LaoHan.Battle
{
    public class lhPlayerStateSync : lhMonoBehaviour,ISync
    {
        ulong charID;

        public void Initialize(ulong charid)
        {
            charID = charid;  
        }

        //public void AddFireInfoHandler(Action<FireInfo> fireInfoHandler)
        //{
        //    lhSyncManager.fireInfoHandler[charID] = fireInfoHandler;
        //}

        //public void AddFanFireInfoHandler(Action<FanFireInfo> fanFireInfoHandler)
        //{
        //    lhSyncManager.fanFireInfoHandler[charID] = fanFireInfoHandler;
        //}

        //public void AddReloadInfoHandler(Action<ReloadInfo> reloadInfoHandler)
        //{
        //    lhSyncManager.reloadInfoHandler[charID] = reloadInfoHandler;
        //}

        //public void AddShowReloadInfoHandler(Action<ShowReloadInfo> showReloadInfoHandler)
        //{
        //    lhSyncManager.showReloadInfoHandler[charID] = showReloadInfoHandler;
        //}

        //public void AddChangeEquipInfoHandler(Action<ChangeEquipInfo> changeEquipInfoHandler)
        //{
        //    lhSyncManager.changeEquipInfoHandler[charID] = changeEquipInfoHandler;
        //}

        //public void AddChangeHpHandler(Action<ChangeHp> changeHpHandler)
        //{
        //    lhSyncManager.changeHpHandler[charID] = changeHpHandler;
        //}

        //public void AddUserDeathHandler(Action<UserDeath> userDeathHandler)
        //{
        //    lhSyncManager.userDeathHandler[charID] = userDeathHandler;
        //}

        //public void AddCharacterRespawnHandler(Action<CharacterRespawn> characterRespawnpHandler)
        //{
        //    lhSyncManager.characterRespawnHandler[charID] = characterRespawnpHandler;
        //}

        //public void AddStartBulletHandler(Action<StartBullet> startBulletHandler)
        //{
        //    lhSyncManager.startBulletHandler[charID] = startBulletHandler;
        //}

        //public void AddRangeFireInfoHandler(Action<RangeFireInfo> rangeFireInfoHandler)
        //{
        //    lhSyncManager.rangeFireInfoHandler[charID] = rangeFireInfoHandler;
        //}
    }
}