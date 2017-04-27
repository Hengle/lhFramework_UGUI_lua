using System;
using System.Collections.Generic;

namespace LaoHan.Infrastruture
{
    public class lhWaitForCount: lhYieldInstruction
    {
        public int count;
        public lhWaitForCount(int count)
        {
            this.count = count;
        }
    }
}
