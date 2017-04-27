using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
using System.Collections.Generic;

namespace LaoHan.Data
{
    public partial class lhConfigData
    {
        [lhConfigData("ConfigData,LocUIConfig",ConfigType.Json)]
        public static UIConfig uiConfig { get; private set; }
        public class UIConfig
        {
            public DefaultData defaultData { get; private set; }
            public List<Library> utilityLibrary { get; private set; }
            public List<BackMusicConnectionData> backMusicConnection{ get; private set; }
            public List<AudioData> uiAudio { get; private set; }
            public List<GuideData> guide { get; private set; }
            public List<Library> uiLibrary { get; private set; }

            public class DefaultData
            {
                public string uiRoot { get;private set; }
                public string uiClass { get; private set; }
                public string backMusicConnection { get; private set; }
            }
            public class Library
            {
                public string uiClass { get;private set; }
                public string uiPath { get;private set; }
            }
            public class BackMusicConnectionData
            {
                public string soundConnection { get; private set; }
                public List<string> audioList { get; private set; }
            }
            public class AudioData
            {
                public string audioType { get; private set; }
                public string audioPath { get; private set; }
            }
            public class GuideData
            {
                public string guidId { get; private set; }
                public string uiPath { get; private set; }
                public List<ProcessData> process { get; private set; }
                public class ProcessData
                {
                    public string processObj { get; private set; }
                    public string maskObj { get; private set; }
                }
            }
        }
    }
}
