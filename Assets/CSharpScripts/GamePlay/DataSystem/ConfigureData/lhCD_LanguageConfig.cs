using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
using System.Collections.Generic;

namespace LaoHan.Data
{
    public partial class lhConfigData
    {
        [lhConfigData("ConfigData,LanguageConfig",ConfigType.Json)]
        public static List<Dictionary<string, string>> languageConfig { get; private set; }
    }
}
