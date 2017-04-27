using UnityEngine;
using System.Collections;
using LaoHan.Network;
//using comm.msg;
using LaoHan.Infrastruture;
using System.Collections.Generic;

namespace LaoHan.Battle
{
    public class lhPlayerControlSend:lhMonoBehaviour
    {
        ulong charID;

        public void Initialize(ulong charid)
        {
            charID = charid;
        }
        
    }
}
