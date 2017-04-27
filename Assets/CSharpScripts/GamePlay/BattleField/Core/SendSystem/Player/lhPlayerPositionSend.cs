using UnityEngine;
using System.Collections;
//using comm.msg;
using Vector3 = UnityEngine.Vector3;
using System;
using LaoHan.Control;
using LaoHan.Infrastruture;

namespace LaoHan.Battle
{
    public class lhPlayerPositionSend:lhMonoBehaviour
    {
        //private float m_posDeviation = 0.375f;
        //private float m_angleDeviation = 10f;
        //private Transform m_target;
        //private MoveInfo m_moveInfo;
        //private Vector3 m_sendPosition;
        //private float m_sendAngle;
        //private uint m_sendState;
        //private float m_time;
        //private float m_interval=0.15f;
        //private uint m_moving;
        //public void SetTarget(Transform target,ulong charid)
        //{
        //    m_target = target;
        //    m_sendPosition = target.position;
        //    m_sendAngle = target.eulerAngles.y;
        //    m_moveInfo = new MoveInfo();
        //    m_moveInfo.charid = charid;
        //    m_moveInfo.location = new comm.msg.Vector3();
        //    if (m_moveInfo.location.x == null)
        //        m_moveInfo.location.x = new Float();
        //    if (m_moveInfo.location.y == null)
        //        m_moveInfo.location.y = new Float();
        //    if (m_moveInfo.location.z == null)
        //        m_moveInfo.location.z = new Float();
        //    if (m_moveInfo.speed == null)
        //        m_moveInfo.speed = new comm.msg.Vector3();
        //    if (m_moveInfo.speed.x == null)
        //        m_moveInfo.speed.x = new Float();
        //    if (m_moveInfo.speed.y == null)
        //        m_moveInfo.speed.y = new Float();
        //    if (m_moveInfo.speed.z == null)
        //        m_moveInfo.speed.z = new Float();
        //    if (m_moveInfo.acce == null)
        //        m_moveInfo.acce = new comm.msg.Vector3();
        //    if (m_moveInfo.acce.x == null)
        //        m_moveInfo.acce.x = new Float();
        //    if (m_moveInfo.acce.y == null)
        //        m_moveInfo.acce.y = new Float();
        //    if (m_moveInfo.acce.z == null)
        //        m_moveInfo.acce.z = new Float();
        //    if (m_moveInfo.pitch == null)
        //        m_moveInfo.pitch = new Float();
        //    if (m_moveInfo.yaw == null)
        //        m_moveInfo.yaw = new Float();

        //}
        //public void SetAcceleration(Vector3 acceleration)
        //{
        //    m_moveInfo.acce.x.value = Convert.ToInt64(acceleration.x * 1000);
        //    m_moveInfo.acce.y.value = Convert.ToInt64(acceleration.y * 1000);
        //    m_moveInfo.acce.z.value = Convert.ToInt64(acceleration.z * 1000);
        //}
        //public void SetVelocity(Vector3 velocity)
        //{
        //    m_moveInfo.speed.x.value = Convert.ToInt64(velocity.x * 1000);
        //    m_moveInfo.speed.y.value = Convert.ToInt64(velocity.y * 1000);
        //    m_moveInfo.speed.z.value = Convert.ToInt64(velocity.z * 1000);
        //}
        //public void SetMoving(bool moving)
        //{
        //    m_moveInfo.moving = (uint)(moving==true?1:0);
        //}
        //public void SetState(uint state)
        //{
        //    m_moveInfo.state = state;
        //}
        //public void SetPitch(float pitch)
        //{
        //    m_moveInfo.pitch.value = Convert.ToInt64(pitch * 1000);
        //}
        //public void SetYaw(float yaw)
        //{
        //    m_moveInfo.yaw.value = Convert.ToInt64(yaw * 1000);
        //}

        //void Update()
        //{
        //    if (m_target == null || m_moveInfo==null) return;
        //    if( m_sendState != m_moveInfo.state)
        //    {
        //        SendMoveInfo();
        //        m_time = Time.time;
        //    }
        //    if(m_moving!=m_moveInfo.moving)
        //    {
        //        SendMoveInfo();
        //        m_time = Time.time;
        //    }
        //    if (Time.time - m_time >= m_interval)
        //    {
        //        m_time = Time.time;
        //        float distance=Vector3.Distance(m_target.position, m_sendPosition);
        //        if (distance >= m_posDeviation)
        //        {
        //            SendMoveInfo();
        //        }
        //        else
        //        {
        //            if(Mathf.Abs(m_target.eulerAngles.y-m_sendAngle) > m_angleDeviation && Mathf.Abs(m_target.eulerAngles.y-m_sendAngle)<200)
        //            {
        //                SendSimpleEuler();
        //            }
        //        }
        //    }
        //}
        //private void SendMoveInfo()
        //{
        //    m_sendPosition = m_target.position;
        //    m_sendState = m_moveInfo.state;
        //    m_sendAngle = m_target.eulerAngles.y;
        //    m_moving = m_moveInfo.moving;
        //    m_moveInfo.location.x.value = Convert.ToInt64(m_target.position.x * 1000) ;
        //    m_moveInfo.location.y.value =Convert.ToInt64(m_target.position.y * 1000);
        //    m_moveInfo.location.z.value = Convert.ToInt64(m_target.position.z * 1000);
        //    lhSendManager.SendProtocol<MoveInfo>(m_moveInfo,true);
        //}
        //private void SendSimpleEuler()
        //{
        //    m_sendPosition = m_target.position;
        //    m_sendState = m_moveInfo.state;
        //    m_sendAngle = m_target.eulerAngles.y;
        //    m_moving = m_moveInfo.moving;
        //    m_moveInfo.location.x.value = 0;
        //    m_moveInfo.location.y.value = 0;
        //    m_moveInfo.location.z.value = 0;
        //    lhSendManager.SendProtocol<MoveInfo>(m_moveInfo, true);
        //}
    }
}
