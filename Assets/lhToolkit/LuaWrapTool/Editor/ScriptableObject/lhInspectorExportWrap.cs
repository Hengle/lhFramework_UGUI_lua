using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using LaoHan.Infrastruture.ulua;

namespace LaoHan.Tools.AutoWrap
{
    public class lhInspectorExportWrap : ScriptableObject
    {
        public lhAutoWrap.BindType bindType;
    }
}