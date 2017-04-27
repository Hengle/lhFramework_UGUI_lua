using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
using System.Collections.Generic;

namespace LaoHan.Data
{
    public partial class lhConfigData
    {
        [lhConfigData("ConfigData,SceneConfig",ConfigType.Json)]
        public static Dictionary<string,string> sceneConfig { get; private set; }
    }
}
