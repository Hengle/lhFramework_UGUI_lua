//#define LOG_DEBUG
//#define LOG_File
//#define LOG_GUI
//#define LOG_UPLOAD

using UnityEngine;
using System.IO;
using System.Collections;
using System;

namespace LaoHan.Infrastruture
{
    public class lhDebug
    {
        public static Action<bool> debugShowHandler;

        private static lhDebug m_instance;
        private string m_log;
        private string m_warning;
        private string m_error;
        private string m_curLog;
        private FileStream m_fileStream;
        private string filePath = Application.persistentDataPath + "/lhLog.txt";
        private Vector2 m_debugScrollPosition;
#if LOG_GUI
        private bool m_openLog = true;
        private string[] m_toolbarText = new string[4] { "All", "Log", "Warning", "Error" };
        private int m_toolbar;
        private GUIStyle m_guiStyle=new GUIStyle();
#endif
        public static lhDebug GetInstance()
        {
            if (m_instance != null)
            {
                Debug.LogWarning("LaoHan: lhDebug is instantiation");
                return null;
            }
            return m_instance = new lhDebug();
        }
        lhDebug()
        {
            //Application.RegisterLogCallback (Application_logMessageReceived);
            Application.logMessageReceivedThreaded+=Application_logMessageReceivedThreaded;
#if LOG_GUI
            if (debugShowHandler!=null)
            {
                debugShowHandler(m_instance.m_openLog);
            }
#endif
        }

        void Application_logMessageReceivedThreaded(string condition, string stackTrace, LogType type)
        {
#if LOG_File
            if (type == LogType.Error || type == LogType.Assert || type==LogType.Exception)
            {
                m_instance.WriteToLog( "\nError->" + condition + ":" + stackTrace );
            }
            else if (type==LogType.Log)
            {
                m_instance.WriteToLog("\nlog->" + condition + ":" + stackTrace );
            }
            else if (type==LogType.Warning)
            {
                m_instance.WriteToLog("\nWarning->" + condition + ":" + stackTrace );
            }
#endif
#if LOG_GUI
            if (type == LogType.Error || type == LogType.Assert || type==LogType.Exception)
            {
                m_instance.m_error += "\nError->" + condition + ":" + stackTrace ;
            }
            else if (type==LogType.Log)
            {
                m_instance.m_log += "\nlog->" + condition + ":" + stackTrace ;
            }
            else if (type==LogType.Warning)
            {
                m_instance.m_warning += "\nWarning->" + condition + ":" + stackTrace ;
            }
#endif
        }

        void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
#if LOG_File
            if (type == LogType.Error || type == LogType.Assert || type==LogType.Exception)
            {
                m_instance.WriteToLog( "\nError->" + condition + ":" + stackTrace );
            }
            else if (type==LogType.Log)
            {
                m_instance.WriteToLog("\nlog->" + condition + ":" + stackTrace );
            }
            else if (type==LogType.Warning)
            {
                m_instance.WriteToLog("\nWarning->" + condition + ":" + stackTrace );
            }
#endif
#if LOG_GUI
            if (type == LogType.Error || type == LogType.Assert || type == LogType.Exception)
            {
                m_instance.m_error += "\nError->" + condition + ":" + stackTrace + ":" + type;
            }
            else if (type == LogType.Log)
            {
                m_instance.m_guiStyle.normal.textColor = Color.white;
                m_instance.m_log += "\nlog->" + condition + ":" + stackTrace + ":" + type;
            }
            else if (type == LogType.Warning)
            {
                m_instance.m_warning += "\nWarning->" + condition + ":" + stackTrace + ":" + type;
            }
#endif
        }
        public static void Log(object log)
        {
#if LOG_DEBUG
            Debug.Log(log);
#endif
        }
        public static void Log(string str, params object[] args)
        {
            str = string.Format(str, args);
            Log((object)str);
        }
        public static void LogWarning(object log)
        {
#if LOG_DEBUG
            Debug.LogWarning(log);
#endif
        }
        public static void LogWarning(string str, params object[] args)
        {
            str = string.Format(str, args);
            LogWarning((object)str);
        }
        public static void LogError(object log)
        {
#if LOG_DEBUG
            Debug.LogError(log);
#endif
        }
        public static void LogError(string str, params object[] args)
        {
            str = string.Format(str, args);
            LogError((object)str);
        }
        public static void LogException(System.Exception exception)
        {
#if LOG_DEBUG
            Debug.LogException(exception);
#endif
        }
        private void WriteToLog(object log)
        {
            string logstr = log.ToString();
            if (m_fileStream == null)
            {
                //if (!File.Exists(filePath))
                //    File.Create(filePath);
                m_fileStream = new FileStream(filePath, FileMode.Append);
                logstr = "\n--------------------->LaoHan: QQ_369016334     CreateTime:" + System.DateTime.Now.ToString("yyyy:MM:dd-hh:mm") + "\n" + logstr;
            }
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(logstr);
            m_fileStream.Write(bytes, 0, bytes.Length);
            bytes = null;
        }
        public void Dispose()
        {
            if (m_fileStream != null)
                m_fileStream.Dispose();
        }
        public void OnGUI()
        {
#if LOG_GUI
        if (!m_openLog)
        {
            if (GUILayout.Button("Open"))
            {
                m_openLog = true;
                if (debugShowHandler != null)
                {
                    debugShowHandler(m_instance.m_openLog);
                }
            }
        }
        else
        {
            GUILayout.BeginVertical();
            m_debugScrollPosition = GUILayout.BeginScrollView(m_debugScrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(9.3f*Screen.height/10));
            m_guiStyle.normal.background=null;
            m_guiStyle.fontSize = 32;
            m_instance.m_guiStyle.normal.textColor = Color.white;
            GUILayout.Label(m_curLog,m_guiStyle);
            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal();
            m_toolbar = GUILayout.Toolbar(m_toolbar, m_toolbarText, GUILayout.Height((0.7f * Screen.height / 10)));
            switch (m_toolbar)
            {
                case 0:
                    m_curLog = m_log + m_warning + m_error;
                    break;
                case 1:
                    m_curLog = m_log;
                    break;
                case 2:
                    m_curLog = m_warning;
                    break;
                case 3:
                    m_curLog = m_error;
                    break;
            }
            if (GUILayout.Button("Clear", GUILayout.Height((0.7f*Screen.height / 10))))
            {
                m_log = "";
                m_warning = "";
                m_error = "";
            }
            if (GUILayout.Button("Close", GUILayout.Height((0.7f * Screen.height / 10))))
            {
                m_openLog = false;
                if (debugShowHandler != null)
                {
                    debugShowHandler(m_instance.m_openLog);
                }
            }
#if LOG_UPLOAD
            if (GUILayout.Button("UpLoad", GUILayout.Height((0.7f * Screen.height / 10))))
            {
                /*lhHttp.Post("http://192.168.1.38/public/Log/" + lhMemoryData.Character.name + System.DateTime.Now.ToString("yy_MM_dd_hh_mm_ss") + ".txt",
                    System.Text.Encoding.UTF8.GetBytes(m_curLog), 
                    (www) => { 
                        
                });*/

                //[CY ADD: upload log]
                lhCoroutine.StartCoroutine(PostLog(lhMemoryData.Character.name + "__" + System.DateTime.Now.ToString("MM_dd_HH_mm_ss"), m_curLog));
            }
#endif
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
#endif
        }

        IEnumerator PostLog(string fileName, string logData)
        {
            string post_url = @"http://192.168.1.38/save_log.php?" + "name=" + WWW.EscapeURL(fileName) + "&data=" + WWW.EscapeURL(logData);

            WWW data_post = new WWW(post_url);
            yield return data_post;

            if (data_post.error != null)
                lhDebug.LogWarning((object)("PostLog: There was an error saving data: " + data_post.error));
        }
    }
}