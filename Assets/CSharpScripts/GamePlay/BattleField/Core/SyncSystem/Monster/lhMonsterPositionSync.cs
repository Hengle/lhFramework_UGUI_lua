using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
//using comm.msg;

using Vector3 = UnityEngine.Vector3;
using System.Collections.Generic;
using System;

namespace LaoHan.Battle
{
    public class lhMonsterPositionSync : MonoBehaviour, ISync
    {
        public Action reachedCallback;
        public Action<float> moveInfoHandler;

        private ulong m_charid;
        private lhNavMeshAgent m_navMeshAgent;
        private Transform m_transform;
        public void Initialize(ulong charid)
        {
            this.m_charid = charid;
            m_navMeshAgent = new lhNavMeshAgent(transform);
            m_navMeshAgent.reachedCallback = reachedCallback;
            //lhSyncManager.monsterMoveInfoHandler[charid] = OnMonsterMoveInfo;
            //lhSyncManager.monsterLocationHandler[charid] = OnMonsterLocation;
            m_transform = transform;
        }
        public void Dispose()
        {
            //if (lhSyncManager.monsterMoveInfoHandler.ContainsKey(m_charid) && lhSyncManager.monsterMoveInfoHandler[m_charid] != null)
            //    lhSyncManager.monsterMoveInfoHandler.Remove(m_charid);
        }
        void Update()
        {
            m_navMeshAgent.Update();
        }
        void OnDestroy()
        {
            //if (lhSyncManager.monsterMoveInfoHandler.ContainsKey(m_charid) && lhSyncManager.monsterMoveInfoHandler[m_charid] != null)
            //    lhSyncManager.monsterMoveInfoHandler.Remove(m_charid);
        }
        void OnDrawGizmos()
        {
            m_navMeshAgent.OnDrawGizmos();
        }
        //void OnMonsterLocation(MonsterLocation location)
        //{
        //    //m_navMeshAgent.Stop();
        //    m_transform.position = new Vector3((float)location.location.x.value / 1000, (float)location.location.y.value / 1000, (float)location.location.z.value / 1000);
        //}
        //void OnMonsterMoveInfo(MonsterMoveInfo moveInfo)
        //{
        //    var list = new List<Vector3>();
        //    for (int i = 0; i < moveInfo.locations.Count; i++)
        //    {
        //        list.Add(new Vector3((float)moveInfo.locations[i].x.value / 1000, (float)moveInfo.locations[i].y.value / 1000, (float)moveInfo.locations[i].z.value / 1000));
        //    }
        //    m_navMeshAgent.speed = (float)moveInfo.speed.value / 1000;
        //    m_navMeshAgent.SetDestination(list);
        //    if (moveInfoHandler!=null)
        //    {
        //        moveInfoHandler(m_navMeshAgent.speed);
        //    }
        //}
    }
}