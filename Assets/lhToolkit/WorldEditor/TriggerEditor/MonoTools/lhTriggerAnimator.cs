using UnityEngine;
using System.Collections;
using LaoHan.Battle;
using LaoHan.Network;
using LaoHan.Infrastruture;
using System;


namespace LaoHan.Tools.WorldEditor
{
    public class lhTriggerAnimator : lhTriggerBase
    {
        public Animator animator;
        void Start()
        {
            //lhSyncManager.playAnimateHandler[id] = OnTrigger;
        }
        //void OnTrigger(PlayAnimate protocol)
        //{
        //    if (protocol.type.ToLower().Equals("trigger"))
        //    {
        //        animator.SetTrigger(protocol.name);
        //    }
        //    else if (protocol.type.ToLower().Equals("int"))
        //    {
        //        animator.SetInteger(protocol.name, Convert.ToInt32(protocol.value));
        //    }
        //    else if (protocol.type.ToLower().Equals("float"))
        //    {
        //        animator.SetFloat(protocol.name, Convert.ToSingle(protocol.value));
        //    }
        //    else if (protocol.type.ToLower().Equals("bool"))
        //    {
        //        animator.SetBool(protocol.name, Convert.ToBoolean(protocol.value));
        //    }
        //    else
        //    {
        //        Debug.LogError("laohan: dont has this type:  " + protocol.type);
        //    }
        //    Debug.Log("OnTrigger Animator " + id+"   type:"+protocol.type+"    name:"+protocol.name+"    value:"+protocol.value);
        //}
        void OnDrawGizmos()
        {
            if (animator == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(animator.transform.position, 0.5f);
        }
    }
}