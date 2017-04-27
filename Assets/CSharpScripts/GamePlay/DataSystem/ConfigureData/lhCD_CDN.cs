using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
using System.Collections.Generic;

namespace LaoHan.Data
{
    public partial class lhConfigData
    {
        [lhConfigData("ConfigData,CDN",ConfigType.Json)]
        public static CDN cdn { get; private set; }
        public class CDN
        {
            public List<Zone> zone { get; private set; }
            public List<Map> map { get; private set; }

            public class Zone
            {
                public ushort id { get; private set; }
                public string name { get; private set; }
            }
            public class Map
            {
                public ushort id { get; private set; }
                public string ip { get; private set; }
                public int port { get; private set; }
            }
        }
    }
}
