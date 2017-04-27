using UnityEngine;
using System;

namespace LaoHan.Infrastruture
{
    public class lhTransform
    {
        public Action<Vector3> positionHandler;
        public Action<Quaternion> rotationHandler;
        private Vector3 m_position;
        private Quaternion m_rotation;
        public lhTransform(Vector3 position, Quaternion rotation)
        {
            m_position = position;
            m_rotation = rotation;
        }
        public Vector3 eulerAngles
        {
            get
            {
                return this.m_rotation.eulerAngles;
            }
            set
            {
                this.m_rotation = Quaternion.Euler(value);
            }
        }

        public Vector3 forward
        {
            get
            {
                return this.rotation * Vector3.forward;
            }
            set
            {
                this.rotation = Quaternion.LookRotation(value);
            }
        }
        public Vector3 position
        {
            get
            {
                return m_position;
            }
            set
            {
                m_position = value;
                if (positionHandler != null) positionHandler(m_position);
            }
        }

        public Vector3 right
        {
            get
            {
                return this.rotation * Vector3.right;
            }
            set
            {
                this.rotation = Quaternion.FromToRotation(Vector3.right, value);
            }
        }
        public Quaternion rotation
        {
            get
            {
                return m_rotation;
            }
            set
            {
                m_rotation = value;
                if (rotationHandler != null) rotationHandler(m_rotation);
            }
        }
        public Vector3 up
        {
            get
            {
                return this.rotation * Vector3.up;
            }
            set
            {
                this.rotation = Quaternion.FromToRotation(Vector3.up, value);
            }
        }
        /*public Vector3 InverseTransformDirection (Vector3 direction)
        {
            return Transform.INTERNAL_CALL_InverseTransformDirection (this, ref direction);
        }

        public Vector3 InverseTransformDirection (float x, float y, float z)
        {
            return this.InverseTransformDirection (new Vector3 (x, y, z));
        }

        public Vector3 InverseTransformPoint (Vector3 position)
        {
            return Transform.INTERNAL_CALL_InverseTransformPoint (this, ref position);
        }

        public Vector3 InverseTransformPoint (float x, float y, float z)
        {
            return this.InverseTransformPoint (new Vector3 (x, y, z));
        }

        public Vector3 InverseTransformVector (float x, float y, float z)
        {
            return this.InverseTransformVector (new Vector3 (x, y, z));
        }

        public Vector3 InverseTransformVector (Vector3 vector)
        {
            return Transform.INTERNAL_CALL_InverseTransformVector (this, ref vector);
        }
        */
        public void LookAt(Vector3 worldPosition)
        {
            this.rotation = Quaternion.LookRotation((worldPosition - this.m_position).normalized);
        }
        public void LookAt(lhTransform target)
        {
            this.LookAt(target.position);
        }
        public void Rotate(Vector3 eulerAngles)
        {
            Quaternion rhs = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
            this.rotation *= Quaternion.Inverse(this.m_rotation) * rhs * this.m_rotation;
        }
        public void Rotate(Vector3 axis, float angle)
        {
            this.eulerAngles += axis * angle;
        }
        public void RotateAround(Vector3 axis, float angle)
        {
            this.eulerAngles += axis * angle;
        }
        public void RotateAround(Vector3 point, Vector3 axis, float angle)
        {
            Vector3 vector = this.position;
            Quaternion rotation = Quaternion.AngleAxis(angle, axis);
            Vector3 vector2 = vector - point;
            vector2 = rotation * vector2;
            vector = point + vector2;
            this.position = vector;
            this.RotateAround(axis, angle * 0.0174532924f);
        }

        /*public Vector3 TransformDirection (Vector3 direction)
        {
            return Transform.INTERNAL_CALL_TransformDirection (this, ref direction);
        }

        public Vector3 TransformDirection (float x, float y, float z)
        {
            return this.TransformDirection (new Vector3 (x, y, z));
        }

        public Vector3 TransformPoint (float x, float y, float z)
        {
            return this.TransformPoint (new Vector3 (x, y, z));
        }

        public Vector3 TransformPoint (Vector3 position)
        {
            return Transform.INTERNAL_CALL_TransformPoint (this, ref position);
        }

        public Vector3 TransformVector (Vector3 vector)
        {
            return Transform.INTERNAL_CALL_TransformVector (this, ref vector);
        }

        public Vector3 TransformVector (float x, float y, float z)
        {
            return this.TransformVector (new Vector3 (x, y, z));
        }*/

        public void Translate(Vector3 translation)
        {
            this.position += translation;
        }

    }
}