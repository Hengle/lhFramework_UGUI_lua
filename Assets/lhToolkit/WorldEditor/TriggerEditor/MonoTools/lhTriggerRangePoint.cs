using UnityEngine;
using System.Collections;

namespace LaoHan.Tools.WorldEditor
{
    public class lhTriggerRangePoint : MonoBehaviour
    {
        public float radius = 1;
        
        void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}