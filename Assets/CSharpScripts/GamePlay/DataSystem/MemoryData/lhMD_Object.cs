using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LaoHan.Data
{
    public partial class lhMemoryData
    {
        public ObjectData obj = new ObjectData();
        public List<ObjectData> objs = new List<ObjectData>();

        public class ObjectData
        {
            public uint baseid;//配置表id
            public uint thisid;//流水id
            public uint num;//数量
            public uint owerid;//拥有者id
            public bool inuse;//武器是否在使用
        }

        public static ObjectData objData  { get { return m_instance != null ? m_instance.obj : null; } }
        public static List<ObjectData> objDatas { get { return m_instance != null ? m_instance.objs : null; } }
       
    }

    public enum ObjectType
    {
        Type_None               = 0,    //一般道具
	    Type_Weapon             = 1,    //武器
	    Type_Avatar             = 2,    //形象
	    Type_PeiJian            = 3,    //配件
	    Type_Object             = 4    //道具
    }
}
