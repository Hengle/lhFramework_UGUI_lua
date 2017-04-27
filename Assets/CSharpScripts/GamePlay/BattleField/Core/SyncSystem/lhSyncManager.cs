using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


using LaoHan.Network;
using LaoHan.Infrastruture;
using LaoHan.Control;

using Vector3 = UnityEngine.Vector3;

namespace LaoHan.Battle
{
    public class lhSyncManager
    {

        #region public Action
        //public Action<> battleInitialHandler;
        //public Action<> playerEnterHandler;
        #endregion

        #region private member
        #endregion

        #region public static member
        //public static Dictionary<ulong, Action<MoveInfo>> moveInfoHandler=new Dictionary<ulong,Action<MoveInfo>>();
        //public static Dictionary<ulong, Action<FireInfo>> fireInfoHandler=new Dictionary<ulong,Action<FireInfo>>();
        //public static Dictionary<ulong, Action<FanFireInfo>> fanFireInfoHandler = new Dictionary<ulong, Action<FanFireInfo>>();
        //public static Dictionary<ulong, Action<ReloadInfo>> reloadInfoHandler = new Dictionary<ulong, Action<ReloadInfo>>();
        //public static Dictionary<ulong, Action<ShowReloadInfo>> showReloadInfoHandler = new Dictionary<ulong, Action<ShowReloadInfo>>();
        //public static Dictionary<ulong, Action<StateInfos>> buffInfoHandler = new Dictionary<ulong, Action<StateInfos>>();
        //public static Dictionary<ulong, Action<ChangeEquipInfo>> changeEquipInfoHandler = new Dictionary<ulong, Action<ChangeEquipInfo>>();
        //public static Dictionary<ulong, Action<ChangeHp>> changeHpHandler = new Dictionary<ulong, Action<ChangeHp>>();
        //public static Dictionary<ulong, Action<UserDeath>> userDeathHandler = new Dictionary<ulong, Action<UserDeath>>();
        //public static Dictionary<ulong, Action<CharacterRespawn>> characterRespawnHandler = new Dictionary<ulong, Action<CharacterRespawn>>();
        //public static Dictionary<ulong, Action<StartBullet>> startBulletHandler = new Dictionary<ulong, Action<StartBullet>>();
        //public static Dictionary<ulong, Action<RangeFireInfo>> rangeFireInfoHandler = new Dictionary<ulong, Action<RangeFireInfo>>();

        //public static Dictionary<ulong, Action<MonsterMoveInfo>> monsterMoveInfoHandler = new Dictionary<ulong, Action<MonsterMoveInfo>>();
        //public static Dictionary<ulong, Action<MonsterLocation>> monsterLocationHandler = new Dictionary<ulong, Action<MonsterLocation>>();
        //public static Dictionary<ulong, Action<MonsterFireInfo>> monsterFireInfoHandler = new Dictionary<ulong, Action<MonsterFireInfo>>();
        //public static Dictionary<ulong, Action<MonsterStopFire>> monsterStopFireHandler = new Dictionary<ulong, Action<MonsterStopFire>>();
        //public static Dictionary<ulong, Action<MonsterIdleAction>> monsterIdleActionHandler = new Dictionary<ulong, Action<MonsterIdleAction>>();
        //public static Dictionary<ulong, Action<StartMonsterBullet>> startMonsterBulletHandler = new Dictionary<ulong, Action<StartMonsterBullet>>();
        //public static Dictionary<ulong, Action<MonsterSuicide>> monsterSuicideHandler = new Dictionary<ulong, Action<MonsterSuicide>>();

        //public static Dictionary<string, Action<PlayAnimate>> playAnimateHandler = new Dictionary<string, Action<PlayAnimate>>();
        //public static Dictionary<string, Action<CutScene>> cutSceneHandler=new Dictionary<string,Action<CutScene>>();
        //public static Dictionary<string, Action<ShowWall>> showWallHandler = new Dictionary<string, Action<ShowWall>>();
        //public static Dictionary<string, Action<PlayMusic>> playMusicHandler = new Dictionary<string, Action<PlayMusic>>();

        #endregion

        #region Contruture
        private static lhSyncManager m_instance;
        public static lhSyncManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhSyncManager();
        }
        lhSyncManager()
        {
            //lhNetwork.RegisterProtobuf<MoveInfo>(OnMoveInfo);
            //lhNetwork.RegisterProtobuf<FireInfo>(OnFireInfo);
            //lhNetwork.RegisterProtobuf<FanFireInfo>(OnFanFireInfo);
            //lhNetwork.RegisterProtobuf<ReloadInfo>(OnReloadInfo);
            //lhNetwork.RegisterProtobuf<ShowReloadInfo>(OnShowReloadInfo);
            //lhNetwork.RegisterProtobuf<StateInfos>(OnBuffInfo);
            //lhNetwork.RegisterProtobuf<ChangeEquipInfo>(OnChangeEquipInfo);
            //lhNetwork.RegisterProtobuf<ChangeHp>(OnChangeHp);
            //lhNetwork.RegisterProtobuf<UserDeath>(OnUserDeath);
            //lhNetwork.RegisterProtobuf<CharacterRespawn>(OnCharacterRespawn);
            //lhNetwork.RegisterProtobuf<MonsterMoveInfo>(OnMonsterMoveInfo);
            //lhNetwork.RegisterProtobuf<MonsterLocation>(OnMonsterLocation);
            //lhNetwork.RegisterProtobuf<StartBullet>(OnStartBullet);
            //lhNetwork.RegisterProtobuf<RangeFireInfo>(OnRangeFireInfo);
            //lhNetwork.RegisterProtobuf<MonsterFireInfo>(OnMonsterFireInfo);
            //lhNetwork.RegisterProtobuf<MonsterStopFire>(OnMonsterStopFire);
            //lhNetwork.RegisterProtobuf<MonsterIdleAction>(OnMonsterIdleAction);
            //lhNetwork.RegisterProtobuf<StartMonsterBullet>(OnStartMonsterBullet);
            //lhNetwork.RegisterProtobuf<MonsterSuicide>(OnMonsterSuicide);

            //lhNetwork.RegisterProtobuf<PlayAnimate>(OnPlayAnimate);
            //lhNetwork.RegisterProtobuf<CutScene>(OnCutScene);
            //lhNetwork.RegisterProtobuf<ShowWall>(OnShowWall);
            //lhNetwork.RegisterProtobuf<PlayMusic>(OnPlayMusic);
        }
        #endregion

        #region Event callback
        //void OnMoveInfo(IProtocol protocol)
        //{
        //    MoveInfo info=protocol.GetProtobuf<MoveInfo>(typeof(MoveInfo));
        //    if (moveInfoHandler.ContainsKey(info.charid))
        //        moveInfoHandler[info.charid](info);
        //}
        //void OnFireInfo(IProtocol protocol)
        //{
        //    FireInfo info = protocol.GetProtobuf<FireInfo>(typeof(FireInfo));
        //    if (fireInfoHandler.ContainsKey(info.charid))
        //        fireInfoHandler[info.charid](info);
        //}
        //void OnFanFireInfo(IProtocol protocol)
        //{
        //    FanFireInfo info = protocol.GetProtobuf<FanFireInfo>(typeof(FanFireInfo));
        //    if (fanFireInfoHandler.ContainsKey(info.charid))
        //        fanFireInfoHandler[info.charid](info);
        //}
        //void OnReloadInfo(IProtocol protocol)
        //{
        //    ReloadInfo info = protocol.GetProtobuf<ReloadInfo>(typeof(ReloadInfo));
        //    if (reloadInfoHandler.ContainsKey(info.charid))
        //        reloadInfoHandler[info.charid](info);
        //}
        //void OnShowReloadInfo(IProtocol protocol)
        //{
        //    ShowReloadInfo info = protocol.GetProtobuf<ShowReloadInfo>(typeof(ShowReloadInfo));
        //    if (showReloadInfoHandler.ContainsKey(info.charid))
        //        showReloadInfoHandler[info.charid](info);
        //}
        //void OnBuffInfo(IProtocol protocol)
        //{
        //    StateInfos info = protocol.GetProtobuf<StateInfos>(typeof(StateInfos));
        //    if (buffInfoHandler.ContainsKey(info.charid))
        //        buffInfoHandler[info.charid](info);
        //}
        //void OnChangeEquipInfo(IProtocol protocol)
        //{
        //    ChangeEquipInfo info = protocol.GetProtobuf<ChangeEquipInfo>(typeof(ChangeEquipInfo));
        //    if (changeEquipInfoHandler.ContainsKey(info.charid))
        //        changeEquipInfoHandler[info.charid](info);
        //}
        //void OnChangeHp(IProtocol protocol)
        //{
        //    ChangeHp info = protocol.GetProtobuf<ChangeHp>(typeof(ChangeHp));
        //    if (changeHpHandler.ContainsKey(info.charid))
        //        changeHpHandler[info.charid](info);
        //}
        //void OnUserDeath(IProtocol protocol)
        //{
        //    UserDeath info = protocol.GetProtobuf<UserDeath>(typeof(UserDeath));
        //    if (userDeathHandler.ContainsKey(info.charid))
        //        userDeathHandler[info.charid](info);
        //}
        //void OnCharacterRespawn(IProtocol protocol)
        //{
        //    CharacterRespawn info = protocol.GetProtobuf<CharacterRespawn>(typeof(CharacterRespawn));
        //    if (characterRespawnHandler.ContainsKey(info.data.charid))
        //        characterRespawnHandler[info.data.charid](info);
        //}
        //void OnMonsterMoveInfo(IProtocol protocol)
        //{
        //    MonsterMoveInfo info = protocol.GetProtobuf<MonsterMoveInfo>(typeof(MonsterMoveInfo));
        //    if (monsterMoveInfoHandler.ContainsKey(info.tempid))
        //        monsterMoveInfoHandler[info.tempid](info);
        //}
        //void OnMonsterLocation(IProtocol protocol)
        //{
        //    MonsterLocation info = protocol.GetProtobuf<MonsterLocation>(typeof(MonsterLocation));
        //    if(monsterLocationHandler.ContainsKey(info.tempid))
        //    {
        //        monsterLocationHandler[info.tempid](info);
        //    }
        //}
        //void OnStartBullet(IProtocol protocol)
        //{
        //    StartBullet info = protocol.GetProtobuf<StartBullet>(typeof(StartBullet));
        //    if (startBulletHandler.ContainsKey(info.charid))
        //        startBulletHandler[info.charid](info);
        //}
        //void OnRangeFireInfo(IProtocol protocol)
        //{
        //    RangeFireInfo info = protocol.GetProtobuf<RangeFireInfo>(typeof(RangeFireInfo));
        //    if (rangeFireInfoHandler.ContainsKey(info.charid))
        //        rangeFireInfoHandler[info.charid](info);
        //}
        //void OnPlayAnimate(IProtocol protocol)
        //{
        //    PlayAnimate info = protocol.GetProtobuf<PlayAnimate>(typeof(PlayAnimate));
        //    if (playAnimateHandler.ContainsKey(info.id))
        //        playAnimateHandler[info.id](info);
        //}
        //void OnCutScene(IProtocol protocol)
        //{
        //    CutScene info = protocol.GetProtobuf<CutScene>(typeof(CutScene));
        //    if (cutSceneHandler.ContainsKey(info.id))
        //        cutSceneHandler[info.id](info);
        //}
        //void OnShowWall(IProtocol protocol)
        //{
        //    ShowWall info = protocol.GetProtobuf<ShowWall>(typeof(ShowWall));
        //    if (showWallHandler.ContainsKey(info.id))
        //        showWallHandler[info.id](info);
        //}
        //void OnPlayMusic(IProtocol protocol)
        //{
        //    PlayMusic info = protocol.GetProtobuf<PlayMusic>(typeof(PlayMusic));
        //    if (playMusicHandler.ContainsKey(info.id))
        //        playMusicHandler[info.id](info);
        //}
        //void OnMonsterFireInfo(IProtocol protocol)
        //{
        //    MonsterFireInfo info = protocol.GetProtobuf<MonsterFireInfo>(typeof(MonsterFireInfo));
        //    if (monsterFireInfoHandler.ContainsKey(info.monster_id))
        //        monsterFireInfoHandler[info.monster_id](info);
        //}
        //void OnMonsterStopFire(IProtocol protocol)
        //{
        //    MonsterStopFire info = protocol.GetProtobuf<MonsterStopFire>(typeof(MonsterStopFire));
        //    if (monsterStopFireHandler.ContainsKey(info.monster_id))
        //        monsterStopFireHandler[info.monster_id](info);
        //}

        //void OnMonsterIdleAction(IProtocol protocol)
        //{
        //    MonsterIdleAction info = protocol.GetProtobuf<MonsterIdleAction>(typeof(MonsterIdleAction));
        //    if (monsterIdleActionHandler.ContainsKey(info.tempid))
        //        monsterIdleActionHandler[info.tempid](info);
        //}
        //void OnStartMonsterBullet(IProtocol protocol)
        //{
        //    StartMonsterBullet info = protocol.GetProtobuf<StartMonsterBullet>(typeof(StartMonsterBullet));
        //    if (startMonsterBulletHandler.ContainsKey(info.monster_id))
        //        startMonsterBulletHandler[info.monster_id](info);
        //}
        //void OnMonsterSuicide(IProtocol protocol)
        //{
        //    MonsterSuicide info = protocol.GetProtobuf<MonsterSuicide>(typeof(MonsterSuicide));
        //    if (monsterSuicideHandler.ContainsKey(info.monster_id))
        //        monsterSuicideHandler[info.monster_id](info);
        //}
        #endregion

        #region public methods
        public void Dispose()
        {
            //lhNetwork.RemoveProtobuf<MoveInfo>(OnMoveInfo);
            //lhNetwork.RemoveProtobuf<FireInfo>(OnFireInfo);
            //lhNetwork.RemoveProtobuf<FanFireInfo>(OnFanFireInfo);
            //lhNetwork.RemoveProtobuf<ReloadInfo>(OnReloadInfo);
            //lhNetwork.RemoveProtobuf<ShowReloadInfo>(OnShowReloadInfo);
            //lhNetwork.RemoveProtobuf<StateInfos>(OnBuffInfo);
            //lhNetwork.RemoveProtobuf<ChangeEquipInfo>(OnChangeEquipInfo);
            //lhNetwork.RemoveProtobuf<ChangeHp>(OnChangeHp);
            //lhNetwork.RemoveProtobuf<UserDeath>(OnUserDeath);
            //lhNetwork.RemoveProtobuf<CharacterRespawn>(OnCharacterRespawn);
            //lhNetwork.RemoveProtobuf<StartBullet>(OnStartBullet);
            //lhNetwork.RemoveProtobuf<RangeFireInfo>(OnRangeFireInfo);

            //lhNetwork.RemoveProtobuf<MonsterMoveInfo>(OnMonsterMoveInfo);
            //lhNetwork.RemoveProtobuf<MonsterLocation>(OnMonsterLocation);
            //lhNetwork.RemoveProtobuf<MonsterFireInfo>(OnMonsterFireInfo);
            //lhNetwork.RemoveProtobuf<MonsterStopFire>(OnMonsterStopFire);
            //lhNetwork.RemoveProtobuf<MonsterIdleAction>(OnMonsterIdleAction);
            //lhNetwork.RemoveProtobuf<StartMonsterBullet>(OnStartMonsterBullet);
            //lhNetwork.RemoveProtobuf<MonsterSuicide>(OnMonsterSuicide);

            //lhNetwork.RemoveProtobuf<PlayAnimate>(OnPlayAnimate);
            //lhNetwork.RemoveProtobuf<CutScene>(OnCutScene);
            //lhNetwork.RemoveProtobuf<ShowWall>(OnShowWall);
            //lhNetwork.RemoveProtobuf<PlayMusic>(OnPlayMusic);

            //moveInfoHandler.Clear();
            //fireInfoHandler.Clear();
            //fanFireInfoHandler.Clear();
            //reloadInfoHandler.Clear();
            //showReloadInfoHandler.Clear();
            //buffInfoHandler.Clear();
            //changeEquipInfoHandler.Clear();
            //changeHpHandler.Clear();
            //userDeathHandler.Clear();
            //characterRespawnHandler.Clear();
            //monsterMoveInfoHandler.Clear();
            //monsterLocationHandler.Clear();
            //startBulletHandler.Clear();
            //rangeFireInfoHandler.Clear();
            //playAnimateHandler.Clear();
            //cutSceneHandler.Clear();
            //showWallHandler.Clear();
            //playMusicHandler.Clear();
            //monsterFireInfoHandler.Clear();
            //monsterStopFireHandler.Clear();
            //monsterIdleActionHandler.Clear();
            //startMonsterBulletHandler.Clear();
            //monsterSuicideHandler.Clear();
            //m_instance = null;
        }
        #endregion

    }
}
