using UnityEngine;
using System.Collections;

namespace LaoHan.Infrastruture
{
    public class lhConvert
    {
        public static string RectToString(Rect vec)
        {
            return vec.x + "," + vec.y + "," + vec.width + "," + vec.height;
        }
        public static Rect StringToRect(string str)
        {
            string[] ss = str.Split(',');
            Rect rect = new Rect();
            rect.x = float.Parse(ss[0]);
            rect.y = float.Parse(ss[1]);
            rect.width = float.Parse(ss[2]);
            rect.height = float.Parse(ss[3]);
            return rect;
        }
        public static string Vector3ToString(Vector3 vec)
        {
            return vec.x + "," + vec.y + "," + vec.z;
        }
        public static Vector3 StringToVector3(string str)
        {
            string[] ss = str.Split(',');
            Vector3 vec;
            vec.x = float.Parse(ss[0]);
            vec.y = float.Parse(ss[1]);
            vec.z = float.Parse(ss[2]);
            return vec;
        }
        public static string Vector2ToString(Vector2 vec)
        {
            return vec.x + "," + vec.y;
        }
        public static Vector2 StringToVector2(string str)
        {
            string[] ss = str.Split(',');
            Vector2 vec;
            vec.x = float.Parse(ss[0]);
            vec.y = float.Parse(ss[1]);
            return vec;
        }
        public static string Vector4ToString(Vector4 vec)
        {
            return vec.x + "," + vec.y + "," + vec.z + "," + vec.w;
        }
        public static Vector4 StringToVector4(string str)
        {
            string[] ss = str.Split(',');
            Vector4 vec;
            vec.x = float.Parse(ss[0]);
            vec.y = float.Parse(ss[1]);
            vec.z = float.Parse(ss[2]);
            vec.w = float.Parse(ss[3]);
            return vec;
        }
        public static string ColorToString(Color c)
        {
            return (byte)(c.r * 255) + "," + (byte)(c.g * 255) + "," + (byte)(c.b * 255) + "," + (byte)(c.a * 255);
        }
        public static Color StringToColor(string str)
        {
            string[] ss = str.Split(',');
            Color c;
            c.r = ((float)byte.Parse(ss[0])) / 255.0f;
            c.g = ((float)byte.Parse(ss[1])) / 255.0f;
            c.b = ((float)byte.Parse(ss[2])) / 255.0f;
            c.a = ((float)byte.Parse(ss[3])) / 255.0f;
            return c;
        }
    }
}