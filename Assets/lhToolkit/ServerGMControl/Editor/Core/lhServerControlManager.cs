using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
using UnityEditor;
using System;
using LaoHan.Network;
using System.Collections.Generic;

namespace LaoHan.Tools.ServerControl
{
    public class lhServerControlManager  {

        public class ServerData
        {
            public string uri;
            public ServerData(string uri)
            {
                this.uri = uri;
            }
            public ServerData(JsonObject json)
            {
                uri = json["uri"].AsString();
            }
            public void Send(Action<WWW> onSendResult)
            {
                lhHttp.Get(uri, onSendResult);
            }
            public void Show()
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Uri:");
                    uri = EditorGUILayout.TextField(uri);
                    GUILayout.FlexibleSpace();
                    if(GUILayout.Button("Send"))
                    {
                        EditorUtility.DisplayProgressBar("Server ", "Server Executing", 0.5f);
                        Send((www) =>
                        {
                            if(string.IsNullOrEmpty(www.error))
                                EditorUtility.DisplayDialog("Information", "Success", "Ok");
                            else
                                EditorUtility.DisplayDialog("Information", www.text, "Ok");
                            EditorUtility.ClearProgressBar();
                        });
                    }
                    if(GUILayout.Button("X"))
                    {
                        DeleteData(this);
                    }
                } EditorGUILayout.EndHorizontal();
            }
            public IJsonNode ToJson()
            {
                var json = new JsonObject();
                json["uri"] = new JsonString(uri);
                return json;
            }
        }
        private lhCoroutine m_coroutine;
        private lhHttp m_http;
        private List<ServerData> m_dataList;
        private string m_filePath=Application.dataPath + "/lhToolkit/ServerControl/Sources/lhServerControl.txt";
        private static lhServerControlManager m_instance;
        public static lhServerControlManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhServerControlManager();
        }
        lhServerControlManager()
        {
            m_dataList = new List<ServerData>();
            m_coroutine = lhCoroutine.GetInstance();
            m_http = lhHttp.GetInstance();
            if (System.IO.File.Exists(m_filePath))
            {
                string defineStr = System.IO.File.ReadAllText(m_filePath, System.Text.Encoding.UTF8);
                if (!string.IsNullOrEmpty(defineStr))
                {
                    var json = lhJson.Parse(defineStr) as JsonArray;
                    foreach (var j in json)
                    {
                        m_dataList.Add(new ServerData((JsonObject)j));
                    }
                }
            }
        }
        public void Update()
        {
            m_coroutine.LateUpdate();
        }
        public void Dispose()
        {
            m_coroutine.Dispose();
            m_coroutine = null;
            m_http.Dispose();
            m_http = null;
            m_instance = null;
        }
        public void Create()
        {
            m_dataList.Add(new ServerData("http://"));
        }
        public void ShowServerData()
        {
            for (int i = 0; i < m_dataList.Count; i++)
            {
                m_dataList[i].Show();
            }
        }
        public void Save()
        {
            JsonArray json = new JsonArray();
            foreach (var d in m_instance.m_dataList)
            {
                json.Add(d.ToJson());
            }
            System.IO.File.WriteAllText(m_instance.m_filePath, json.ToString(), System.Text.Encoding.UTF8);
            AssetDatabase.Refresh();
        }
        private static void DeleteData(ServerData data)
        {
            m_instance.m_dataList.Remove(data);
        }
    }
}