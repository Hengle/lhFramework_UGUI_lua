using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
using System.Collections.Generic;

namespace LaoHan.Data
{
    public partial class lhConfigData
    {
        [lhConfigData("ConfigData,AudioConfig", ConfigType.Json)]
        public static AudioConfig audioConfig { get; private set; }
        public class AudioConfig
        {
            public List<MixerData> mixers { get; private set; }
            public List<AudioData> sound2D { get; private set; }
            public List<AudioData> sound3D { get; private set; }
            public List<AudioData> backMusic { get; private set; }
            public class AudioData
            {
                public string name { get; private set; }
                public string clipPath { get; private set; }
                public string prefabPath { get; private set; }
                public string mixerName { get; private set; }
                public string mixerGroup { get; private set; }
            }
            public class MixerData
            {
                public string name { get;private set; }
                public string path { get;private set; }
            }
        }
    }
}
