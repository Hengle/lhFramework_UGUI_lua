using UnityEngine;
using System.Collections;
using System;
using LaoHan.Network;

namespace LaoHan.Infrastruture
{
    public class lhSourceUpdate
    {
        
        private static lhSourceUpdate m_instance;
        public static lhSourceUpdate GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhSourceUpdate();
        }
        lhSourceUpdate(){}
        public void Dispose()
        {
            m_instance = null;
        }
        public void Updating(Action onUpdateOver)
        {
            if (lhDefine.projectType == ProjectType.develop)
            {
                onUpdateOver();
            }
            else
            {

                onUpdateOver();
            }
        }

        public static void VersionChanged(string url, Action<bool> onResult)
        {
            if (lhDefine.projectType == ProjectType.develop || lhDefine.projectType == ProjectType.debug)
                onResult(false);
            else
            {
                lhHttp.Get(url, (www) =>
                {
                    if (!string.IsNullOrEmpty(www.error))
                        onResult(false);
                    else
                    {
                        var remoteJson = lhJson.Parse(www.text) as JsonObject;
                        string remoteVersion = remoteJson["Version"].AsString();
                        if (lhResources.FileExists("Version.json"))
                        {
                            var localJson = lhJson.Parse(lhResources.LoadText("Version.json")) as JsonObject;
                            string localVersion = localJson["Version"].AsString();
                            if (string.Equals(remoteVersion, localVersion))
                                onResult(false);
                            else
                                onResult(true);
                        }
                        else
                            onResult(true);
                    }
                });
            }
        }
        public static void VersionUpdate(string url, Action<int> progress)
        {
            if (lhDefine.projectType == ProjectType.develop || lhDefine.projectType == ProjectType.debug)
                progress(100);
            else
                progress(100);
        }
    }
}