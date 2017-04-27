using UnityEngine;
using System.Collections;
using System.Text;

public class MyExtensionMethods {
    // Fields
    public static Vector3 vec3 = Vector3.zero;

    // Methods
    public static void AppendLineEx(StringBuilder sb, string str = "")
    {
        sb.Append(str + "\r\n");
    }

    public static void Clear(StringBuilder sb)
    {
        sb.Length = 0;
    }

    public static void Identity(Transform trans)
    {
        trans.localScale=Vector3.one;
        trans.localPosition=Vector3.zero;
        trans.localRotation=Quaternion.identity;
    }

}
