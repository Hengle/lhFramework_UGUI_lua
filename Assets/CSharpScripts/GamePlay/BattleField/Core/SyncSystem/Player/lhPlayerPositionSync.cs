using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
//using comm.msg;
using Vector3 = UnityEngine.Vector3;
using System;
using System.Collections.Generic;
//using ProtoBuf;

namespace LaoHan.Battle
{
    public class lhPlayerPositionSync : lhMonoBehaviour,ISync
    {
//        public Action<bool> isMovingHanlder;
//        public Action<Vector3> velocityHandler;//角色速度
//        public Action<Vector3> accelerationHandler;//角色加速度
//        public Action<float> yawHandler;// 角色方向
//        public Action<float> pitchHandler;// 枪的倾斜角度
//        public Action<uint> stateHandler;// 当前状态

//        private ulong m_charId;
//        private Transform m_transform;
//        private MoveInfo m_moveInfo;
//        private IEnumerator m_enumerator;

//        private bool m_smooth=false;
//        private float m_preInterval=0.100f;
//        private float m_time;
//        private Vector3 m_newPos;
//        private Vector3 m_oldPos;
//        private Vector3 m_oldVel;
//        private Vector3 m_oldAcc;
//        private uint m_oldState;
//        private float m_fallHeight;
//        private bool m_moving;
//#if lhSyncGizmos
//        private List<GizmosTest> m_packageList = new List<GizmosTest>();
//        private List<GizmosTest> m_smoothList = new List<GizmosTest>();
//        private GameObject m_objParent;
//        private class GizmosTest
//        {
//            public Vector3 position;
//            public Color color;
//        }
//#endif
//        public void Initialize(ulong charId)
//        {
//#if lhSyncGizmos
//            m_objParent = new GameObject("Path-Player:->" + name);
//#endif
//            this.m_charId = charId;
//            m_transform = transform;
//            m_fallHeight = m_transform.position.y;
//            m_oldPos = m_transform.position;
//            if (lhSyncManager.moveInfoHandler != null)
//                lhSyncManager.moveInfoHandler[charId] = OnPositionSync;
//        }
//        public void Dispose()
//        {
//            if (lhSyncManager.moveInfoHandler.ContainsKey(m_charId) && lhSyncManager.moveInfoHandler[m_charId] != null)
//                lhSyncManager.moveInfoHandler.Remove(m_charId);
//        }
//        void OnDestroy()
//        {
//            if (lhSyncManager.moveInfoHandler.ContainsKey(m_charId) && lhSyncManager.moveInfoHandler[m_charId] != null)
//                lhSyncManager.moveInfoHandler.Remove(m_charId);
//        }
//#if lhSyncGizmos
//        void OnDrawGizmos()
//        {
//            for (int i = 0; i < m_packageList.Count-1; i++)
//            {
//                Gizmos.color = m_packageList[i].color;
//                Gizmos.DrawLine(m_packageList[i].position, m_packageList[i + 1].position);
//                Gizmos.color = Color.white;
//            }
//            for (int i = 0; i < m_smoothList.Count - 1; i++)
//            {
//                Gizmos.color = m_smoothList[i].color;
//                Gizmos.DrawLine(m_smoothList[i].position, m_smoothList[i + 1].position);
//                Gizmos.color = Color.white;
//            }
//            if (m_packageList.Count > 999)
//            {
//                UnityEngine.Object.Destroy(m_objParent);
//                m_objParent = new GameObject("Debug:->" + name);
//                m_packageList.Clear();
//                m_packageList.Capacity = 999;
//            }
            
//        }
//#endif

//        public void OnCollisionEnter(Collision collision)
//        {
//            for (int i = 0; i < collision.contacts.Length; i++)
//            {
//                if (m_oldState == 0)
//                {
//                    if (Mathf.Abs(collision.contacts[i].point.y - m_transform.position.y) < 0.1f)
//                    {
//                        if (m_enumerator != null)
//                        {
//                            StopCoroutine(m_enumerator);
//                            m_enumerator = null;
//                            return;
//                        }
//                    }
//                }
//                else
//                {
//                    if (Mathf.Abs(collision.contacts[i].point.y - m_transform.position.y) > 0.1f)
//                    {
//                        if (m_enumerator != null)
//                        {
//                            StopCoroutine(m_enumerator);
//                            m_enumerator = null;
//                            return;
//                        }
//                    }
//                }
//            }
//        }

//        private void OnPositionSync(IExtensible extensible)
//        {
//            var info = (MoveInfo)extensible;
//            m_moveInfo = info;
//            if (info.yaw != null && yawHandler!=null)
//                yawHandler((float)info.yaw.value / 1000);
//            if (info.pitch != null && pitchHandler!=null)
//                pitchHandler((float)info.pitch.value / 1000);
//            if (stateHandler != null)
//                stateHandler(info.state);
//            m_moving = info.moving == 1 ? true : false;
//            if (m_moving && isMovingHanlder!=null)
//                isMovingHanlder(true);
//            var newVel = new Vector3((float)m_moveInfo.speed.x.value / 1000, (float)m_moveInfo.speed.y.value / 1000, (float)m_moveInfo.speed.z.value / 1000);
//            m_newPos = new Vector3((float)m_moveInfo.location.x.value / 1000, (float)m_moveInfo.location.y.value / 1000, (float)m_moveInfo.location.z.value / 1000);
//            var newAcc = new Vector3((float)m_moveInfo.acce.x.value / 1000, (float)m_moveInfo.acce.y.value / 1000, (float)m_moveInfo.acce.z.value / 1000);
//            if (velocityHandler != null)
//                velocityHandler(newVel);
//            if (accelerationHandler!=null)
//                accelerationHandler(newAcc);
//            newAcc = Vector3.zero;
//            if (m_newPos.Equals(Vector3.zero)) return;
//#if lhSyncGizmos
//            m_packageList.Add(new GizmosTest() { position = m_newPos+Vector3.up*0.2f, color = Color.cyan });
//            var obj = new GameObject();
//            obj.transform.position = m_newPos;
//            obj.transform.parent = m_objParent.transform;
//#endif
//            Vector3 pos1 = Vector3.zero;
//            Vector3 pos2 = Vector3.zero;
//            Vector3 pos3 = Vector3.zero;
//            Vector3 pos4 = Vector3.zero;
//            if (info.state == (int)0 && m_oldState == (int)1)//fall
//            {
//                m_fallHeight = m_newPos.y;
//            }
//            else if (m_oldState == 0 && info.state == 2)//jump
//                m_fallHeight = m_oldPos.y;
//            else
//            {
//                if (m_smooth)
//                    m_fallHeight = -99999;
//                else
//                    m_fallHeight = m_newPos.y;
//            }
//            if (m_smooth)
//            {
//                pos1 = m_transform.position;
//                pos2 = pos1 + m_oldVel * m_preInterval;
//                if (m_moving)
//                {
//                    pos3 = m_newPos + newVel * m_preInterval * (1 - m_time) + 0.5f * newAcc * Mathf.Pow((1 - m_time), 2);
//                    pos4 = pos3 + (newVel * m_preInterval + newAcc * (1 - m_time));
//                }
//                else
//                {
//                    pos4 = m_newPos;
//                    pos3 = pos4 - newVel * m_preInterval * (1 - m_time) + 0.5f * newAcc * Mathf.Pow((1 - m_time), 2);
//                }
//            }
//            else
//            {
//                pos1 = m_transform.position;
//                pos2 = pos1 + m_oldVel * m_preInterval;
//                pos4 = m_newPos;
//                pos3 = pos4 - newVel * m_preInterval;
//            }
//            m_oldPos = m_newPos;
//            m_oldAcc = newAcc;
//            m_oldVel = newVel;
//            m_oldState = info.state;
//            m_moveInfo = null;
//            if (m_enumerator!=null)
//            {
//                StopCoroutine(m_enumerator);
//                m_enumerator = null;
//            }
//            m_enumerator = EModifyVelocity(pos1, pos2, pos3, pos4);
//            StartCoroutine(m_enumerator);

//        }
//        IEnumerator EModifyVelocity(Vector3 pos1,Vector3 pos2,Vector3 pos3,Vector3 pos4)
//        {
//            float x0 = pos1.x;
//            float x1 = pos2.x;
//            float x2 = pos3.x;
//            float x3 = pos4.x;

//            float y0 = pos1.y;
//            float y1 = pos2.y;
//            float y2 = pos3.y;
//            float y3 = pos4.y;

//            float z0 = pos1.z;
//            float z1 = pos2.z;
//            float z2 = pos3.z;
//            float z3 = pos4.z;

//            float a = x3 - 3 * x2 + 3 * x1 - x0;
//            float b = 3 * x2 - 6 * x1 + 3 * x0;
//            float c = 3 * x1 - 3 * x0;
//            float d = x0;

//            float e = y3 - 3 * y2 + 3 * y1 - y0;
//            float f = 3 * y2 - 6 * y1 + 3 * y0;
//            float g = 3 * y1 - 3 * y0;
//            float h = y0;

//            float i = z3 - 3 * z2 + 3 * z1 - z0;
//            float j = 3 * z2 - 6 * z1 + 3 * z0;
//            float k = 3 * z1 - 3 * z0;
//            float l = z0;
//            m_smooth = true;
//            float time = 0;
//            while (true)
//            {
//                time+=Time.deltaTime*3;
//                float x = a * Mathf.Pow(time, 3) + b * Mathf.Pow(time, 2) + c * time + d;
//                float y = e * Mathf.Pow(time, 3) + f * Mathf.Pow(time, 2) + g * time + h;
//                float z = i * Mathf.Pow(time, 3) +j * Mathf.Pow(time, 2) + k * time + l;
//                m_time = time;
//#if lhSyncGizmos
//                m_smoothList.Add(new GizmosTest() { position = new Vector3(x, y, z), color = Color.red });
//#endif
//                if (y < m_fallHeight && m_fallHeight != -99999)
//                    y = m_fallHeight;
//                m_transform.position = new Vector3(x, y, z);
//                if (time >= 1)
//                {
//#if lhSyncGizmos
//                    Debug.DrawLine(pos1, pos2, Color.green, 1000);
//                    Debug.DrawLine(pos2, pos3, Color.blue, 1000);
//                    Debug.DrawLine(pos3, pos4, Color.yellow, 1000);
//#endif
//                    if (!m_moving)
//                    {
//                        velocityHandler(Vector3.zero);
//                        if (isMovingHanlder!=null)
//                            isMovingHanlder(false);
//                    }
//                    m_smooth = false;
//                    break;
//                }
//                yield return 1;
//            }
//        }

    }

}