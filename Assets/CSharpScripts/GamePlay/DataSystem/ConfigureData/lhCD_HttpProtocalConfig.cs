using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
using System.Collections.Generic;

namespace LaoHan.Data
{
    public partial class lhConfigData
    {
        [lhConfigData("ConfigData,HttpProtocalConfig", ConfigType.Json)]
        public static HttpProtocalConfig httpProtocalConfig { get; private set; }
        public class HttpProtocalConfig
        {
            public List<ProtocalData> get { get;private set; }
            public List<ProtocalData> post { get;private set; }
            public class ProtocalData
            {
                public string cmd { get;private set; }
                public string url { get;private set; }
            }
        }
    }
}
