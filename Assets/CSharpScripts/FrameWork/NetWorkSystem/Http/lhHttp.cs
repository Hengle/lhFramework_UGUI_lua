using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LaoHan.Infrastruture;
using LaoHan.Data;

namespace LaoHan.Network
{
    public class lhHttp
    {
        public static Action<string> httpErrorHandler;
        
        private Dictionary<string, string> m_protocalGetDic;
        private Dictionary<string, string> m_protocalPostDic;

        private static lhHttp m_instance;
        public static lhHttp GetInstance()
        {
            if (m_instance != null)
                return null;
            return m_instance = new lhHttp();
        }
        lhHttp()
        {
            m_protocalGetDic = new Dictionary<string, string>();
            m_protocalPostDic = new Dictionary<string, string>();
            if (lhConfigData.httpProtocalConfig!=null)
            {
                foreach (var item in lhConfigData.httpProtocalConfig.get)
                {
                    m_protocalGetDic.Add(item.cmd, item.url);
                }
                foreach (var item in lhConfigData.httpProtocalConfig.post)
                {
                    m_protocalPostDic.Add(item.cmd, item.url);
                }
            }
        }
        public void Dispose()
        {
            m_instance = null;
        }
        public static void ProtocalGet(string cmd, Action<byte[]> onReceive)
        {
            if (m_instance.m_protocalGetDic == null)
            {
                lhDebug.LogError((object)"dont initialProtocal");
                return;
            }
            if (!m_instance.m_protocalGetDic.ContainsKey(cmd))
            {
                lhDebug.LogError((object)("m_protocalGetDic dont containsthis key:" + cmd));
                return;
            }
            var url = m_instance.m_protocalGetDic[cmd];
            Get(url, (www) =>
            {
                onReceive(www.bytes);
            });
        }
        public static void ProtocalPost(string cmd, string data, Action<IJsonNode> onReceive)
        {
            if (m_instance.m_protocalPostDic == null)
            {
                lhDebug.LogError((object)"dont initialProtocal");
                return;
            }
            if (!m_instance.m_protocalPostDic.ContainsKey(cmd))
            {
                lhDebug.LogError((object)("m_protocalPostDic dont containsthis key:" + cmd));
                return;
            }
            Post(m_instance.m_protocalPostDic[cmd], data.ToString(),
                (www) =>
                {
                    onReceive(lhJson.Parse(System.Text.Encoding.UTF8.GetString(www.bytes)));
                });
        }
        /// <summary>
        /// http Get
        /// please use httpProtocal
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onReceive"></param>
        public static void Get(string url, Action<WWW> onReceive, int wait = 0)
        {
            lhCoroutine.StartCoroutine(m_instance.EHttpGet(url, onReceive, wait));
        }
        /// <summary>
        /// http post
        /// please use httpProtocal
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="onReceive"></param>
        public static void Post(string url, string data, Action<WWW> onReceive, int wait = 0)
        {
            lhCoroutine.StartCoroutine(m_instance.EHttpPost(url, data, onReceive, wait));
        }
        /// <summary>
        /// http post
        /// please use httpProtocal
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bytes"></param>
        /// <param name="onReceive"></param>
        public static void Post(string url, byte[] bytes, Action<WWW> onReceive, int wait = 0)
        {
            lhCoroutine.StartCoroutine(m_instance.EHttpPost(url, bytes, onReceive, wait));
        }
        /// <summary>
        /// http post
        /// please use httpProtocal
        /// </summary>
        /// <param name="url"></param>
        /// <param name="wwwForm"></param>
        /// <param name="onReceive"></param>
        public static void Post(string url, WWWForm wwwForm, Action<WWW> onReceive, int wait = 0)
        {
            lhCoroutine.StartCoroutine(m_instance.EHttpPost(url, wwwForm, onReceive, wait));
        }
        IEnumerator EHttpGet(string url, Action<WWW> onReceive, int wait = 0)
        {
            yield return new lhWaitForSeconds(wait);
            using (WWW www = new WWW(url))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    if (httpErrorHandler != null)
                        httpErrorHandler(www.error);
                    lhDebug.LogError((object)("LaoHan: url: " + www.url + "  error: " + www.error));
                    //yield return lhWaitForSeconds.Return();
                }
                onReceive(www);
            }
        }
        IEnumerator EHttpPost(string url, string data, Action<WWW> onReceive, int wait)
        {
            yield return new lhWaitForSeconds(wait);
            using (WWW www = new WWW(url, System.Text.Encoding.UTF8.GetBytes(data)))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    if (httpErrorHandler != null)
                        httpErrorHandler(www.error);
                    lhDebug.LogError((object)("LaoHan: url: " + www.url + "  error: " + www.error));
                    //yield return lhWaitForSeconds.Return();
                }
                onReceive(www);
            }
        }
        IEnumerator EHttpPost(string url, byte[] bytes, Action<WWW> onReceive, int wait)
        {
            yield return new lhWaitForSeconds(wait);
            using (WWW www = new WWW(url, bytes))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    if (httpErrorHandler != null)
                        httpErrorHandler(www.error);
                    lhDebug.LogError((object)("LaoHan: url: " + www.url + "  error: " + www.error));
                    // yield return lhWaitForSeconds.Return();
                }
                onReceive(www);
            }
        }
        IEnumerator EHttpPost(string url, WWWForm wwwForm, Action<WWW> onReceive, int wait)
        {
            yield return new lhWaitForSeconds(wait);
            using (WWW www = new WWW(url, wwwForm))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    if (httpErrorHandler != null)
                        httpErrorHandler(www.error);
                    lhDebug.LogError((object)("LaoHan: url: " + www.url + "  error: " + www.error));
                    //yield return lhWaitForSeconds.Return();
                }
                onReceive(www);
            }

        }
    }
}