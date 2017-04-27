using UnityEngine;
using System.Collections;
using LaoHan.Battle;
using System;
using System.Collections.Generic;
using LaoHan.Infrastruture;

namespace LaoHan.Tools.WorldEditor
{
    public class lhTriggerCollider : lhTriggerBase
    {
        public ETriggerType triggerType;
        public LayerMask layer;
        public List<string> tagList;
        public int count = 999999999;

        [HideInInspector]
        public bool activate=true;
        [HideInInspector]
        public float interval = 1;

        private List<int> layerList;
        private Color wireColor=Color.blue;
        private lhTriggerChildCollider[] childBoxColliderArr;
        private float m_time;
        void Start()
        {
            layerList = MaskValueToEnumList(layer);
            wireColor = Color.blue;
            childBoxColliderArr = GetComponentsInChildren<lhTriggerChildCollider>();
        }
        public void OnTriggerEnter(Collider collider)
        {
            if (triggerType == ETriggerType.Enter && activate)
            {
                Trigger(collider);
			}
			activate = false;
			SetChildActivate(false);
        }
        public void OnTriggerStay(Collider collider)
        {
            if (triggerType == ETriggerType.Stay)
            {
                if (Time.time-m_time>=interval)
                {
                    m_time = Time.time;
                    Trigger(collider);
                }
            }
        }
        public void OnTriggerExit(Collider collider)
        {
            if (triggerType == ETriggerType.Exit)
            {
                Trigger(collider);
            }
            activate = true;
            SetChildActivate(true);
        }
        void Trigger(Collider collider)
        {
            if (layerList.Contains(collider.gameObject.layer) && tagList.Contains(collider.gameObject.tag) && count > 0)
            {
                Debug.Log("lhTriggerCollider: " + id);
                count--;
                //EnterCollide value = new EnterCollide();
                //value.rectid = id;
                //lhSendManager.SendProtocol<EnterCollide>(value);
                wireColor = Color.cyan;
                lhInvoke.Invoke(() => { wireColor = Color.blue; }, 0.5f);
            }
        }
        void SetChildActivate(bool activate)
        {
            foreach (var item in childBoxColliderArr)
            {
                item.activate = activate;
            }
        }
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (UnityEditor.EditorApplication.isPlaying)
                Gizmos.color = wireColor;
            else if (count <= 0)
                Gizmos.color = Color.grey;
            else
                Gizmos.color = Color.blue;

            BoxCollider[] boxColliders=GetComponentsInChildren<BoxCollider>();
            foreach (var item in boxColliders)
            {
                Gizmos.DrawWireCube(item.transform.position + item.center, item.size - Vector3.one * 0.01f);
            }
        }
#endif
    }
}