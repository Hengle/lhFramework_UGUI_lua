using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace LaoHan.Tools.WorldEditor
{
    public class lhDragHandler
    {
        public void MouseDrag(string title)
        {
            DragAndDrop.StartDrag(title);
            DragAndDrop.visualMode = DragAndDropVisualMode.Move;
            Event.current.Use();
        }
        public void PrepareStartDrag(string[] paths, UnityEngine.Object[] objectReferences)
        {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.paths=paths;
            DragAndDrop.objectReferences=objectReferences;
        }
        public void SetGenericData(string type,object data)
        {
            DragAndDrop.SetGenericData(type, data);
        }
        public object GetGenericData(string type)
        {
            object data=DragAndDrop.GetGenericData(type);
            return data;
        }
        public bool HasObject()
        {
            return DragAndDrop.paths.Length == 0 ? false : true;
        }
        public string[] GetAssetPaths()
        {
            return DragAndDrop.paths;
        }
        public void SetVisualMode(DragAndDropVisualMode mode)
        {
            DragAndDrop.visualMode = mode;
        }

        public void AcceptDrag()
        {
            DragAndDrop.AcceptDrag();
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        }
        public bool CanReceive()
        {
            return true;
        }
    }
}
