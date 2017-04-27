using UnityEngine;
using System;
using System.Collections.Generic;

namespace LaoHan.Infrastruture.Tools
{
    public class lhParticleReset:lhMonoBehaviour
    {

        private ParticleSystem[] m_arr;
        void OnEnable()
        {
            if (m_arr == null)
                m_arr = gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (var item in m_arr)
            {
                ParticleSystem.EmissionModule psemit = item.emission;
                psemit.enabled = true;
                item.Simulate(0.0f, true, true);
                item.Clear();
                item.Play();
                item.enableEmission = true;
            }
        }
        void OnDisable()
        {
            if (m_arr == null)
                m_arr = gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (var item in m_arr)
            {
                ParticleSystem.EmissionModule psemit = item.emission;

                psemit.enabled = false;
                item.time = 0;
                item.Stop();
                item.Clear();
                item.enableEmission = false;
            }

        }
    }
}
