using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LaoHan.Infrastruture
{
    public class lhAddComponentLibrary : MonoBehaviour
    {
        public List<Component> library;
        void Awake()
        {
            foreach (Component mono in library)
            {
                lhComponent.AddComponent(gameObject, mono.GetType(), mono);
            }
        }
    }
}