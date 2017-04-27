using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LaoHan.UGUI;

namespace LaoHan.Data
{
    public partial class lhMemoryData
    {
        public class StageData
        {
            /// <summary>
            /// 通关的副本
            /// </summary>
            //public RefreshPassCopyList passStageList;
            ///// <summary>
            ///// 已领取的章节奖励列表
            ///// </summary>
            //public RefreshCopyRewardList stageRewardList;
        }

        private StageData stageData = new StageData();
        public static StageData Stage { get { return m_instance.stageData; } }
    }
}
