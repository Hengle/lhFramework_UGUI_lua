using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

namespace LaoHan.Infrastruture
{
    public class lhMonoBehaviour : MonoBehaviour
    {
        protected GameObject lhInstantiate(GameObject obj, bool objectPool = false)
        {
            if (objectPool)
                return (GameObject)lhObjectManager.GetObject(obj);
            else
                return Instantiate(obj) as GameObject;
        }
        protected GameObject lhInstantiate(GameObject obj, Vector3 position, Quaternion rotation, bool objectPool = false,Transform parent=null,bool activate=true,EPoolType pool=EPoolType.None)
        {
            if (objectPool)
                return (GameObject)lhObjectManager.GetObject(obj, position, rotation);
            else
                return Instantiate(obj, position, rotation) as GameObject;
        }
        protected void lhFreeInstantiate(GameObject obj, GameObject waitFree)
        {
            lhObjectManager.FreeObject(obj, waitFree);
        }
        protected Component lhGetComponent(Type type)
        {
            return lhComponent.GetComponent(gameObject, type);
        }
        protected IEnumerator EWaitForSeconds(float time, Action onTimeOver)
        {
            yield return new lhWaitForSeconds(time);
            onTimeOver();
        }
        protected IEnumerator EWaitForSeconds(int frameCount, Action onCountOver)
        {
            yield return new lhWaitForCount(frameCount);
            onCountOver();
        }
        protected void SetChildLayer(Transform trans,int layer)
        {
            foreach (Transform item in trans)
            {
                item.gameObject.layer = layer;
            }
        }
        protected void SetChildLayer(Transform trans,string layerName)
        {
            SetChildLayer(trans, LayerMask.NameToLayer(layerName));
        }
        protected float GetParticleLength(Transform parentTransform)
        {
            ParticleSystem[] particleSystems = parentTransform.GetComponentsInChildren<ParticleSystem>();
            float maxDuration = 0;
            foreach (ParticleSystem ps in particleSystems)
            {
                if (ps.enableEmission)
                {
                    if (ps.loop)
                    {
                        return -1f;
                    }
                    float dunration = 0f;
                    if (ps.emissionRate <= 0)
                    {
                        dunration = ps.startDelay + ps.startLifetime;
                    }
                    else
                    {
                        dunration = ps.startDelay + Mathf.Max(ps.duration, ps.startLifetime);
                    }
                    if (dunration > maxDuration)
                    {
                        maxDuration = dunration;
                    }
                }
            }
            return maxDuration;
        }
    }
}
