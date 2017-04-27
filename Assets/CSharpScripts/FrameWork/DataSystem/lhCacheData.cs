using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using LaoHan.Infrastruture;

namespace LaoHan.Data
{
    public class lhCacheData
    {
        private JsonObject m_cacheJson;
        private static lhCacheData m_instance;
        public static lhCacheData GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhCacheData();
        }
        lhCacheData()
        {
            if (!File.Exists(lhDefine.cacheFilePath))
            {
                InitializeCacheJsonStruct();
            }
            else
            {
                using (FileStream fileStream = new FileStream(lhDefine.cacheFilePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fileStream.Length];
                    int length = (int)fileStream.Length;
                    fileStream.Read(bytes, 0, length);
                    string str = System.Text.Encoding.UTF8.GetString(bytes);
                    m_cacheJson = (JsonObject)lhJson.Parse(str);
                }
            }
        }
        public static bool HasHistoryValue(string key)
        {
            return m_instance.m_cacheJson["HistoryData"].AsDict().ContainsKey(key);
        }
        public static string GetHistoryValue(string key)
        {
            return m_instance.m_cacheJson["HistoryData"].AsDict()[key].AsString();
        }
        public static void SetHistoryValue(string key, string value)
        {
            m_instance.m_cacheJson["HistoryData"].AsDict()[key] = new JsonString(value);
            m_instance.UpdateCache();
        }
        public static void RemoveHistoryValue(string key)
        {
            m_instance.m_cacheJson["HistoryData"].AsDict().Remove(key);
            m_instance.UpdateCache();
        }

        public void Dispose()
        {
            m_cacheJson = null;
            m_instance = null;
        }
        private void InitializeCacheJsonStruct()
        {
            m_cacheJson = new JsonObject();
            FileInfo info = new FileInfo(lhDefine.cacheFilePath);
            if (!info.Directory.Exists)
            {
                info.Directory.Create();
            }
            m_cacheJson["HistoryData"] = new JsonObject();
            UpdateCache();
        }
        private void UpdateCache()
        {
            using (FileStream fs = new FileStream(lhDefine.cacheFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(m_cacheJson.ToString());
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }
}