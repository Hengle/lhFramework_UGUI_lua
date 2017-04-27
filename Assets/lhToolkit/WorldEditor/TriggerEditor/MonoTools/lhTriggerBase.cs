using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LaoHan.Infrastruture;


namespace LaoHan.Tools.WorldEditor
{
    public class lhTriggerBase : lhMonoBehaviour
    {

        public enum ETriggerType
        {
            Enter,
            Stay,
            Exit
        }
        public string id;

        public List<int> MaskValueToEnumList(int value)
        {
            List<int> list = new List<int>();
            int length = DecimalToBinary(value).Length;
            for (int i = 0; i < length; i++)
            {
                int v = value >> i;
                if (v % 2 != 0)
                {
                    list.Add(i);
                }
            }
            return list;
        }
        public string DecimalToBinary(int num)
        {
            num = Mathf.Abs(num);
            if ((int)num / 2 == 0)
            {
                return (num % 2).ToString();
            }
            else
            {
                string result = DecimalToBinary((int)num / 2) + (num % 2).ToString();
                return result;
            }
        }
    }
}