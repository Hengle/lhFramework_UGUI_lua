using UnityEngine;
using System.Collections;

namespace LaoHan.Infrastruture
{
    [ExecuteInEditMode]
    public class lhNodeLink : MonoBehaviour
    {
#if UNITY_EDITOR
        public GameObject start;
        public GameObject end;
        public GameObject handler;
        public float v0 = 0;
        //public float step = 2;
        public float g = 9.8f;

        private float m_interval;
        void Start()
        {
            if (start==null)
            {
                start = new GameObject("start");
                start.transform.parent = transform;
                start.transform.localPosition = Vector3.zero;
            }
            if (end == null)
            {
                end = new GameObject("end");
                end.transform.parent = transform;
                end.transform.localPosition = Vector3.zero;
            }
            if (handler == null)
            {
                handler = new GameObject("handler");
                handler.transform.parent = transform;
                handler.transform.localPosition = Vector3.zero;
            }
        }
        void OnDrawGizmos()
        {
            if (start == null || end == null || handler==null) return;

            float height =Mathf.Abs( end.transform.position.y - start.transform.position.y);
            float sqrt = v0 * v0 - 4 * (-0.5f * g) * (-height);
            float dir = sqrt > 0 ? 1 : -1;
            float totalTime = (-v0 + dir*Mathf.Sqrt(Mathf.Abs(sqrt))) / (2 * (-0.5f * g));
            Vector2 directionhor = new Vector2(end.transform.position.x - start.transform.position.x, end.transform.position.z - start.transform.position.z);
            float speed = directionhor.magnitude / totalTime;
            Debug.Log(totalTime + "  " + speed);

            Vector2 normalized =directionhor.normalized;
            Vector3 startVec = start.transform.position;
            int i = 0;
            while(true)
			{
                float t = 0.01f*i;
                float y =v0*t- 0.5f * g * t * t;
                float nextT=0.01f*(i+1);
                float nextY=v0*nextT- 0.5f * g * nextT * nextT;
                Vector2 hor = new Vector2(startVec.x, startVec.z) + normalized *  (speed*t);
                Vector2 nexthor = new Vector2(startVec.x, startVec.z) + normalized * (speed*nextT);
                Vector3 drawStart = new Vector3(hor.x, startVec.y + y, hor.y);
                Vector3 drawEnd = new Vector3(nexthor.x, startVec.y + nextY, nexthor.y);
                Gizmos.DrawLine(drawStart, drawEnd);
                i++;
                if (y >= height) break;
                //Debug.Log(drawStart + "   " + drawEnd + "    " + currentVec);
			}
        }
#endif
    }
}