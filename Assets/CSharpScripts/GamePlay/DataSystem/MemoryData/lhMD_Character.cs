using UnityEngine;
using System.Collections;

namespace LaoHan.Data
{
    public partial class lhMemoryData
    {
        public CharacterData character = new CharacterData();
        public class CharacterData
        {
            public ulong charId;
            /// <summary>
            /// 角色名称
            /// </summary>
            public string name;
            /// <summary>
            /// 职业
            /// </summary>
            public OccupationType occupation;
            /// <summary>
            /// 等级
            /// </summary>
            public uint level;
            /// <summary>
            /// 经验
            /// </summary>
            public ulong exp;
            /// <summary>
            /// 游戏货币
            /// </summary>
            public ulong money;
            /// <summary>
            /// 重置货币
            /// </summary>
            public ulong gold;
            /// <summary>
            /// 体力值
            /// </summary>
            public uint tilizhi;

            public void SetOccupation(int occupation)
            {
                this.occupation = (OccupationType)occupation;
            }
        }
        public static CharacterData Character { get { return m_instance != null ? m_instance.character : null; } }
    }

    public enum OccupationType
    {
        /// <summary>
        /// 突击兵
        /// </summary>
        Assault = 1,
        /// <summary>
        /// 火箭筒兵
        /// </summary>
        RocketLauncher = 2,
        /// <summary>
        /// 胖子
        /// </summary>
        TheFat = 3,
        /// <summary>
        /// 医生
        /// </summary>
        Doctor = 4,
        /// <summary>
        /// 喷火兵
        /// </summary>
        Flaming = 5,
        /// <summary>
        /// 狙击手
        /// </summary>
        Sniper = 6
    }
}