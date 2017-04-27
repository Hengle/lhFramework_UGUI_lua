using System.Collections.Generic;
using UnityEngine;

namespace LaoHan.Infrastruture
{
    public class lhWaitForSeconds : lhYieldInstruction
    {
        public float time;
        public lhWaitForSeconds(float time)
        {
            this.time = time;
        }
    }
}