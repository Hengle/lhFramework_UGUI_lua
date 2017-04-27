using UnityEngine;
using System.Collections;
using System.Text;

public class StringBuilderCache  {

// Fields
    private static StringBuilder _cache = new StringBuilder();
    private const int MAX_BUILDER_SIZE = 0x200;

    // Methods
    public static StringBuilder Acquire(int capacity = 0x100)
    {
        StringBuilder sb = _cache;
        if ((sb != null) && (sb.Capacity >= capacity))
        {
            _cache = null;
            sb.Length=0;
            return sb;
        }
        return new StringBuilder(capacity);
    }

    public static string GetStringAndRelease(StringBuilder sb)
    {
        string str = sb.ToString();
        Release(sb);
        return str;
    }

    public static void Release(StringBuilder sb)
    {
        if (sb.Capacity <= 0x200)
        {
            _cache = sb;
        }
    }

}
