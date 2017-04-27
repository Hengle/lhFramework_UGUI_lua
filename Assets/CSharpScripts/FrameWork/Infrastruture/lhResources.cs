/*LaoHan:
 * This role is mainly to achieve the management of resources, the structure is divided into three kinds of "develop", "debug", "release", 
 * that is, the development stage, the commissioning phase, release stage, the three stage in terms of resources are different, 
 * the main difference lies in the "load" (string externalPath, string fileName, string sourceName, Action<UnityEngine.Object> onLoadOver) "function of the internal implementation,
 *The main reading "Resources folder resources" on the stage of development, debugging stage mainly read "StreamingAssets" folder inside resources, release as the core functions of the function, in the 'string remoteVersionUrl, 
 *string launched the "platform, Action onFinishedInitial, bool cacheVerification = false" this function background reads: "Version.xml" version control file,.
 *This version of the control file exists in three places: "StreamingAssets", caching, remote server. First of all to cache the file "Version.xml" exists, if there is, 
 *the contrast of remote file "Version.xml" and "Version.xml" version of the file cache to, to update, then use the distance "Version.xml" in "DownLoadUrl" path through the HTTP Get from server update coverage of resources,
 *if the cache does not exist "Version.xml" the file, then contrast the local "StreamingsAssets" in the file "Version.xml" files and remote file "Version.xml"
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using LaoHan.Network;
using Object = UnityEngine.Object;

namespace LaoHan.Infrastruture
{
    public class lhResources
    {
        //private System.Security.Cryptography.SHA1Managed m_sha1;
        private static lhResources m_instance;
        public static lhResources GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhResources();
        }
        private lhResources()
        {
            //        m_localurl =
            //#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            // "file://" + Application.dataPath + "/StreamingAssets/";
            //        //Application.streamingAssetsPath;
            //#elif   UNITY_IPHONE
            //         "file://"+Application.dataPath + "/Raw//";  
            //#elif   UNITY_ANDROID
            //        "jar:file://" + Application.dataPath + "!/assets//";
            //#endif
            //initialize local URL
//#if UNITY_ANDROID && !UNITY_EDITOR
//            //Android is special
//            m_localurl = Application.streamingAssetsPath;
//#else
//            //this url and windows and WP IOS  can use 
//            if (lhDefine.projectType == ProjectType.release)
//                m_localurl = "file://" + Application.streamingAssetsPath + "/";
//            else
//                m_localurl = Application.streamingAssetsPath + "/";
//#endif
//            m_cacheurl = System.IO.Path.Combine(Application.persistentDataPath, "lhVersion") + "/";
//            m_sourceurl = Application.dataPath + "/Resources/";
            //m_sha1 = new System.Security.Cryptography.SHA1Managed();
            //m_sha1.com
        }
        public void Dispose()
        {
            m_instance = null;
        }
        public static byte[] LoadBytes(string filePath)
        {
            return m_instance.LoadDirect(Path.Combine(lhDefine.sourceUrl, filePath));
        } 
        public static string LoadText(string filePath,Encoding coding)
        {
            return coding.GetString(LoadBytes(filePath));
        }
        public static string LoadText(string filePath)
        {
            return LoadText(filePath, System.Text.Encoding.UTF8);
        }
        public static Stream LoadStream(string filePath, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read)
        {
            return new FileStream(lhDefine.sourceUrl + filePath, fileMode, fileAccess);
        }
        public static void CopyDirectory(string sourceDirectory, string destDirectory)
        {
            sourceDirectory = lhDefine.sourceUrl + sourceDirectory;
            destDirectory = lhDefine.sourceUrl + destDirectory;
            m_instance.CopyFolder(sourceDirectory, destDirectory);
        }
        public static void DeleteDirectory(string folderPath)
        {
            m_instance.DeleteFolder(lhDefine.sourceUrl+ folderPath);
        }
        public static void CreateDirectory(string folderPath)
        {
            string path = lhDefine.sourceUrl + folderPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static bool DirectoryExits(string directory)
        {
            return Directory.Exists(lhDefine.sourceUrl + directory);
        }
        public static void DeleteFile(string filePath)
        {
            string path= lhDefine.sourceUrl + filePath;
            if (!File.Exists(path)) return;
            File.Delete(path);
        }
        public static void CopyFile(string filePath, string targetPath)
        {
            filePath = lhDefine.sourceUrl + filePath;
            targetPath = lhDefine.sourceUrl + targetPath;
            if (!File.Exists(filePath))
            {
                Debug.LogError("LaoHan: this file:  <" + filePath + ">  is nont exist");
                return;
            }
            FileInfo tartFileInfo = new FileInfo(targetPath);
            if (tartFileInfo.Exists) tartFileInfo.Delete();
            if (!tartFileInfo.Directory.Exists) tartFileInfo.Directory.Create();
            File.Copy(filePath, targetPath);
        }
        public static bool FileExists(string filePath)
        {
            return File.Exists(lhDefine.sourceUrl + filePath);
        }
        public static void WriteAllBytes(string filePath, byte[] bytes)
        {
            string path = lhDefine.sourceUrl + filePath;
            FileInfo file = new FileInfo(path);
            if (File.Exists(path))
                File.Delete(path);
            if (!file.Directory.Exists)
                file.Directory.Create();
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
        public static void WriteAllText(string filePath, string contents)
        {
            WriteAllBytes(filePath, System.Text.Encoding.UTF8.GetBytes(contents));
        }
        public static void Load(string[] pathArr,Action<int,string,Object> onLoadOver,Action onAllOver,bool destroyReference=true)
        {
            int count=pathArr.Length;
            for (int i = 0; i < pathArr.Length; i++)
            {
                int index = i;
                string path = pathArr[i];
                Load(path, o=> {
                    count--;
                    if (onLoadOver!=null)
                        onLoadOver(index,path,o);
                    if (count<=0)
                    {
                        if (onAllOver!=null)
                            onAllOver();
                    }
                }, destroyReference);
            }
        }
        public static void Load(string splitPath, Action<Object> onLoadOver, bool destroyReference = true)
        {
            Load(splitPath,onLoadOver, ',', destroyReference);
        }
        public static void Load(string splitPath,Action<Object> onLoadOver, char split, bool destroyReference = true)
        {
            string[] splitArr=splitPath.Split(split);
            if (splitArr.Length==2)
                Load(splitArr[0], splitArr[1], splitArr[1], onLoadOver, destroyReference);
            else if (splitArr.Length==3)
                Load(splitArr[0], splitArr[1], splitArr[2], onLoadOver, destroyReference);
            else
                Debug.LogError("LaoHan: load split is error " + splitPath);
        }
        public static void DestroyReference(UnityEngine.Object obj)
        {
            if (lhDefine.projectType != ProjectType.develop)
            {
                lhBundleManager.DestroyReference(obj);
            }
        }
        public static void Load(string externalPath, string fileName, string sourceName, Action<UnityEngine.Object> onLoadOver, bool destroyReference = true)
        {
            if (lhDefine.projectType == ProjectType.develop)
            {
                var obj = Resources.Load(m_instance.PathCombine(externalPath, sourceName), typeof(UnityEngine.Object));
                if (obj == null)
                    lhDebug.LogWarning("LaoHan: obj is null  \nexternalPath:" + externalPath + "\nfileName:" + fileName + "\nsourceName:" + sourceName);
                else
                    lhDebug.Log("LaoHan: obj  \nexternalPath:" + externalPath + "\nfileName:" + fileName + "\nsourceName:" + sourceName);
                onLoadOver(obj);
            }
            else
            {
                //Debug.Log(lhDefine.sourceUrl + lhDefine.bundleFolder + "/" + lhDefine.platform + "/" + externalPath +"/"+ fileName +"-"+ sourceName);
                lhBundleManager.Load(lhDefine.sourceUrl + lhDefine.bundleFolder + "/" + lhDefine.platform + "/", externalPath, fileName, sourceName, onLoadOver, destroyReference);
            }
        }
        public static string GetFileRealPath(string relativePath)
        {
            string filePath = lhDefine.sourceUrl + relativePath;
            if (!File.Exists(filePath))
            {
                lhDebug.LogError((object)("LaoHan: file is nont exist:" + filePath));
            }
            return filePath;
        }
        private byte[] LoadDirect(string filePath)
        {
            using (FileStream s = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                return b;
            }
        }
        private void DeleteFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                foreach (string childName in Directory.GetFileSystemEntries(folderPath))//获取子文件和子文件夹
                {
                    if (File.Exists(childName))
                    {
                        FileInfo fi = new FileInfo(childName);
                        if (fi.IsReadOnly)
                        {
                            fi.IsReadOnly = false;
                        }
                        File.Delete(childName);
                    }
                    else
                    {
                        DeleteFolder(childName);
                        if (Directory.Exists(childName))
                            Directory.Delete(childName, true);
                    }
                }
            }
        }
        private void CopyFolder(string sourceDirectory, string destDirectory)
        {
            if (!Directory.Exists(sourceDirectory))
            {
                Directory.CreateDirectory(sourceDirectory);
            }
            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }
            CopyTopDirectoryFile(sourceDirectory, destDirectory);
            string[] directionName = Directory.GetDirectories(sourceDirectory);
            foreach (string directionPath in directionName)
            {
                string directionPathTemp = destDirectory + "/" + directionPath.Substring(sourceDirectory.Length + 1);

                CopyFolder(directionPath, directionPathTemp);
            }
        }
        private void CopyTopDirectoryFile(string sourceFile, string destFile)
        {
            string[] fileName = Directory.GetFiles(sourceFile, "*", SearchOption.TopDirectoryOnly);
            foreach (string filePath in fileName)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                string filePathTemp = destFile + "/" + fileInfo.Name;

                if (File.Exists(filePathTemp))
                {
                    File.Copy(filePath, filePathTemp, true);
                }
                else
                {
                    File.Copy(filePath, filePathTemp);
                }
            }
        }
        private string PathCombine(params string[] param)
        {
            string path = "";
            foreach (var str in param)
            {
                path = System.IO.Path.Combine(path, str);
            }
            path = path.Replace("\\", "/");
            return path;
        }
        IEnumerator ELoadBytes(string fileUrl, Action<byte[]> onLoadOver)
        {
            using (WWW www = new WWW(fileUrl))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    lhDebug.LogError((object)string.Format("LaoHan: ELoadBytes({0})is error:{1}", fileUrl, www.error));
                    yield return new lhWaitForReturn();
                }
                onLoadOver(www.bytes);
            }
        }
        IEnumerator ELoadBytes(string fileUrl, int waitFrameCount, Action<byte[]> onLoadOver)
        {
            yield return new lhWaitForSeconds(waitFrameCount);
            using (WWW www = new WWW(fileUrl))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    lhDebug.LogError((object)string.Format("LaoHan: ELoadBytes({0})is error:{1}", fileUrl, www.error));
                    yield return new lhWaitForReturn();
                }
                onLoadOver(www.bytes);
            }
        }
    }
}