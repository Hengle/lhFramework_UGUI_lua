using UnityEngine;
using System;
using System.Collections.Generic;
using LaoHan.Infrastruture;

namespace LaoHan.FairyGUI
{
    public class Intent
    {
        private Dictionary<object, object> extras = new Dictionary<object, object>();
        public Intent()
        {

        }
        public void PutExtras(object key,object value)
        {
            if (extras.ContainsKey(key))
            {
                extras.Add(key, value);
            }
            else
            {
                lhDebug.LogError("LaoHan: extras is dont has this key => " + key);
            }
        }
        public bool HasExtras(object key)
        {
            return extras.ContainsKey(key);
        }
        public object GetExtras(object key)
        {
            if (extras.ContainsKey(key))
            {
                return extras[key];
            }
            else
            {
                lhDebug.LogError("LaoHan: extras is dont has this key => " + key);
                return null;
            }
        }
    }

}