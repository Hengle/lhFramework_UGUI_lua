using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
using System.Collections.Generic;

namespace LaoHan.Data
{
    public partial class lhConfigData
    {
        [lhConfigData("ConfigData,LuaLoaderConfig", ConfigType.Json)]
        public static LuaLoaderConfig luaLoaderConfig { get; private set; }
        public class LuaLoaderConfig
        {
            public Entry entry { get; private set; }
            public List<LuaFiles> luaFiles { get; private set; }
            public class LuaFiles
            {
                public string name { get; private set; }
                public string path { get; private set; }
            }
            public class Entry
            {
                public string startFileName { get; private set; }
                public string startFunction { get; private set; }
            }
        }

    }
}
