using UnityEngine;
using System.Collections;


namespace LaoHan.Tools.WorldEditor
{
    public class lhTriggerChildCollider : MonoBehaviour
    {
        public bool activate=true;
        private lhTriggerCollider parentCollider;
        void Start()
        {
            parentCollider = GetComponentInParent<lhTriggerCollider>();
        }
        void OnTriggerEnter(Collider collider)
        {
            if (activate)
            {
                parentCollider.OnTriggerEnter(collider);
            }
        }
        void OnTriggerStay(Collider collider)
        {
             parentCollider.OnTriggerStay(collider);
        }
        void OnTriggerExit(Collider collider)
        {
            parentCollider.OnTriggerExit(collider);
        }
    }
}