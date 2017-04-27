using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LaoHan.Infrastruture
{
    public class lhNavMeshAgent
    {
        public Action reachedCallback;
        public Action<Vector3> nextDirectionHandler;
        Transform m_transform;
        public float nextStep
        {
            get
            {
                return m_nextStep;
            }
        }
        public Vector3 nextPosition
        {
            get
            {
                return m_transform.position + m_nextDirection * m_nextStep;
            }
        }
        public bool targetReached
        {
            get
            {
                return m_targetReached;
            }
        }
        public float speed
        {
            get
            {
                return m_speed;
            }
            set
            {
                m_maxSpeed = value;
            }
        }
        public float angularSpeed
        {
            get
            {
                return m_angularSpeed;
            }
            set
            {
                m_angularSpeed = value;
            }
        }
        public float stoppingDistance
        {
            get
            {
                return m_stoppingDistance;
            }
            set
            {
                m_stoppingDistance = value;
            }
        }
        public float accerate
        {
            get
            {
                return m_accerate;
            }
            set
            {
                m_accerate = value;
            }
        }
        public float jumpAccerate
        {
            get { return m_jumpAccerate; }
            set { m_jumpAccerate = value; }
        }
        public enum EMoveType
        {
            Direct,
            Jump
        }
        float m_accerate=1;
        float m_jumpAccerate = 9.8f;
        float m_speed = 0f;
        float m_maxSpeed = 5.5f;
        float m_angularSpeed = 20;
        float m_stoppingDistance = 0.5f;
        bool m_canMove;
        bool m_targetReached;
        List<Vector3> m_path;
        float m_nextStep;
        int m_nextCornerIndex;
        Vector3 m_nextDirection;
        EMoveType m_moveType;
#if lhSyncGizmos
        GameObject m_objParent;
        List<Vector3> m_sourcePathList = new List<Vector3>();
        List<Vector3> m_pathList = new List<Vector3>();
#endif
        public lhNavMeshAgent(Transform transform)
        {
#if lhSyncGizmos
            m_objParent = new GameObject("Path-Monster:->" + transform.name);
#endif
            this.m_transform = transform;
        }
        public void Update()
        {
            if (!m_canMove || m_targetReached) return;
            Move();
        }
        public void Stop()
        {
            m_canMove = false;
        }
        public void Resume()
        {
            m_canMove = true;
        }
        public void OnDrawGizmos()
        {
#if lhSyncGizmos
            if (m_sourcePathList.Count < 1) return;
            for (int i = 0; i < m_sourcePathList.Count-1; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(m_sourcePathList[i], m_sourcePathList[i + 1]);
                Gizmos.color = Color.white;
            }
            if (m_pathList.Count < 1) return;
            for (int i = 0; i < m_pathList.Count - 1; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(m_pathList[i], m_pathList[i + 1]);
                Gizmos.color = Color.white;
            }
#endif
        }
        public void SetDestination(List<Vector3> path, EMoveType moveType = EMoveType.Direct)
        {
            if (path.Count <= 1)
            {
                lhDebug.LogError((object)("Path count is: " + path.Count + "   " + path[0]));
                return;
            }
#if lhSyncGizmos
            foreach (Transform item in m_objParent.transform)
            {
                if (item == m_objParent.transform) continue;
                UnityEngine.Object.Destroy(item.gameObject);
            }
            m_pathList.Clear();
            m_sourcePathList.Clear();
            for (int i = 0; i < path.Count; i++)
            {
                m_sourcePathList.Add(path[i]);
                var obj = new GameObject(i.ToString());
                obj.transform.position = path[i];
                obj.transform.parent = m_objParent.transform;
            }
#endif
            m_path = path;
            //float totalDistance = CalculateTotalDistance(path.vectorPath);
            this.m_moveType = moveType;
            this.m_speed = Mathf.Lerp(m_speed, m_maxSpeed, Time.deltaTime * m_accerate);
            this.m_nextStep =m_speed * Time.deltaTime;
            this.m_nextCornerIndex = 1;
            if (m_moveType == EMoveType.Direct)
                m_nextDirection = GetSegementDirection().normalized;
            else
                m_nextDirection = GetSegementJump().normalized;
            m_targetReached = false;
            m_canMove = true;
        }
        private void Move()
        {
            if (m_path == null) return;
            RotateTowards(this.m_nextDirection);
            if(nextDirectionHandler!=null)
                nextDirectionHandler(this.m_nextDirection);
            m_transform.Translate(this.m_nextDirection * this.m_nextStep,Space.World);
#if lhSyncGizmos
            m_pathList.Add(m_transform.position);
#endif
            this.m_speed = Mathf.Lerp(m_speed, m_maxSpeed, Time.deltaTime * m_accerate);
            this.m_nextStep = m_speed * Time.deltaTime;
            if (m_nextCornerIndex == m_path.Count - 1)
                if (Mathf.RoundToInt(Vector3.Distance(m_transform.position, m_path[m_path.Count - 1])) <= m_stoppingDistance)
                {
                    m_targetReached = true;
                    if (reachedCallback != null)
                        reachedCallback();
                    m_speed = 0;
                }
            if (m_moveType == EMoveType.Direct)
                m_nextDirection = GetSegementDirection().normalized;
            else
                m_nextDirection = GetSegementJump().normalized;
        }
        private Vector3 GetSegementDirection()
        {
            Vector3 curPosition = m_transform.position;
            Vector3 nextCorner = this.m_path[this.m_nextCornerIndex];
            //If the last node, it is returned directly the direction of the current segment
            if (this.m_nextCornerIndex == this.m_path.Count - 1)
                return nextCorner - curPosition;

            //Current role in the corner where the next target distance is greater 
            //than the current position and the distance between the corner point,
            //causes the role more than we set the path to the track
            //If the next mobile distance is greater than the current position 
            //and the distance between the nodes, they directly in the form of vector addition to jump down
            float distance = Vector3.Distance(curPosition, nextCorner);
            if (distance > this.m_nextStep)
                return nextCorner - curPosition;

            else if (distance < this.m_nextStep)
            {
                Vector3 velocity = nextCorner - curPosition;
                float remainingDistance = this.m_nextStep - distance;
                float previousSegmentDistance = 0;
                for (int i = this.m_nextCornerIndex; i < this.m_path.Count; i++)
                {
                    remainingDistance -= previousSegmentDistance;
                    float nextSegmentDistance = Vector3.Distance(this.m_path[i + 1], this.m_path[i]);
                    previousSegmentDistance = nextSegmentDistance;
                    Vector3 nextSegmentDirection = (this.m_path[i + 1] - this.m_path[i]).normalized;
                    if (remainingDistance > nextSegmentDistance)
                    {
                        velocity += nextSegmentDirection * nextSegmentDistance;
                        if (i + 1 == this.m_path.Count - 1)
                            break;
                        continue;
                    }
                    else
                    {
                        velocity += nextSegmentDirection * remainingDistance;
                        break;
                    }
                }
                this.m_nextStep = velocity.magnitude;
                this.m_nextCornerIndex++;
                return velocity;
            }
            else
            {
                this.m_nextCornerIndex++;
                return nextCorner - curPosition;
            }
        }
        private Vector3 GetSegementJump()
        {
            return Vector3.zero;
        }
        private float CalculateTotalDistance(List<Vector3> corners)
        {
            float distance = 0;
            for (int i = 1; i < corners.Count; i++)
                distance += Vector3.Distance(corners[i], corners[i - 1]);
            return distance;
        }
        private void RotateTowards(Vector3 dir)
        {
            if (dir == Vector3.zero) return;
            Quaternion rot = m_transform.rotation;
            Quaternion toTarget = Quaternion.LookRotation(dir);
            rot = Quaternion.Slerp(rot, toTarget, m_angularSpeed * Time.deltaTime);
            Vector3 euler = rot.eulerAngles;
            euler.z = 0;
            euler.x = 0;
            rot = Quaternion.Euler(euler);
            m_transform.rotation = rot;
        }
    }

}