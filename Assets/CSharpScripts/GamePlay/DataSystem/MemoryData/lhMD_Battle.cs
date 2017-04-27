using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LaoHan.UGUI;

namespace LaoHan.Data
{
    public partial class lhMemoryData
    {
        public BattleData battle = new BattleData();
        public class BattleData
        {
            public uint curRoadId;
            public ulong curRoomId;
            public ulong curCooperateRoomId;
            public uint localPlayerTeam;

            public BattleData()
            {
                
            }
        }
        public static BattleData Battle { get { return m_instance.battle; } }
    }
}
