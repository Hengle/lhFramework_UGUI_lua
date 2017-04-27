using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using LaoHan.Infrastruture;

namespace LaoHan.Tools.LuaCompile
{
    public class lhLuaCompile : Editor
    {
        private static string m_luaScriptsDir = Application.dataPath + "/LuaScripts/";
        private static string m_targetDir = Application.dataPath + "/Resources/LuaScripts/";
        private static string m_configPath = Application.dataPath + "/Resources/ConfigData/luaLoaderConfig.txt";
        [MenuItem("lhTools/LuaTool/Lua Compile %e")]
        public static  void Compile()
        {
            if (EditorApplication.isCompiling)return;
            if (!Directory.Exists(m_luaScriptsDir)) return;
            var json = new JsonObject();
            var entry = new JsonObject();
            entry["startFileName"] = new JsonString("framework/infrastrutureSystem/luaInfrastrutureManager");
            entry["startFunction"] = new JsonString("luaInfrastrutureManager.Ctor");
            json["entry"] = entry;
            var luaFiles = new JsonArray();
            var files = Directory.GetFiles(m_luaScriptsDir, "*.lua", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i].Replace(m_luaScriptsDir, "");
                string target = m_targetDir + file;
                target = target.Replace(".lua", ".txt");
                FileInfo targetFileInfo = new FileInfo(target);
                if (!targetFileInfo.Directory.Exists)
                {
                    targetFileInfo.Directory.Create();
                }
                File.Copy(files[i], target,true);

                FileInfo fileInfo = new FileInfo(file);
                var obj = new JsonObject();
                file = file.Replace(m_luaScriptsDir, "").Replace(fileInfo.Extension, "").Replace("\\", "/");
                obj["name"] = new JsonString(file);
                string sub = file.Substring(file.LastIndexOf("/"));
                string head = file.Substring(0, file.LastIndexOf("/"));
                string tar = sub.Replace("/", ",");
                file = head + tar;
                obj["path"] = new JsonString("LuaScripts/"+file.Replace(fileInfo.Extension, ""));
                luaFiles.Add(obj);
            }
            json["luaFiles"] = luaFiles;
            File.WriteAllText(m_configPath,json.ToString() /*lhJson.PrettyPrint()*/,System.Text.Encoding.UTF8);
            AssetDatabase.Refresh();
        }
    }
}