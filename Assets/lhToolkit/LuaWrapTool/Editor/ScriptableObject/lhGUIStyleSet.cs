using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace LaoHan.Tools.AutoWrap
{
    internal class lhGUIStyleSet : ScriptableObject
    {
        public List<Texture2D> icons = new List<Texture2D>();
        public List<GUIStyle> styles = new List<GUIStyle>();
        public List<GUIStyle> freeStyles = new List<GUIStyle>();

        //[MenuItem("Tasd/CreateSet")]
        //public static void CreateSet()
        //{
        //    var set=ScriptableObject.CreateInstance<lhGUIStyleSet>();
        //    AssetDatabase.CreateAsset(set, "Assets/CustomStyles.asset");
        //}
    }
}