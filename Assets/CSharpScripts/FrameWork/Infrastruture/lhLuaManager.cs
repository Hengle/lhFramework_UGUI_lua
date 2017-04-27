using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LaoHan.Infrastruture.ulua;
using LaoHan.Data;

namespace LaoHan.Infrastruture.ulua
{
    public class lhLuaManager
    {

        private Dictionary<string, byte[]> m_luaDic = new Dictionary<string, byte[]>();
        private bool m_openLuaSocket;
        private LuaState luaState;
        private static lhLuaManager m_instance;
        public static lhLuaManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhLuaManager();
        }
        lhLuaManager()
        {
        }
        public void Initialize(Action OnInitalOver)
        {
            string[] fileNameArr = new string[lhConfigData.luaLoaderConfig.luaFiles.Count];
            string[] pathArr = new string[lhConfigData.luaLoaderConfig.luaFiles.Count];
            for (int i = 0; i < lhConfigData.luaLoaderConfig.luaFiles.Count; i++)
            {
                fileNameArr[i] = lhConfigData.luaLoaderConfig.luaFiles[i].name;
                pathArr[i] = lhConfigData.luaLoaderConfig.luaFiles[i].path;
            }
            lhResources.Load(pathArr,
                (i,s,o) =>
                {
                    if (o==null)
                    {
                        lhDebug.LogError("LaoHan: load is null  ->" + s);
                    }
                    byte[] bytes = (o as TextAsset).bytes;
                    m_luaDic.Add(fileNameArr[i], bytes);
                },
                ()=> {
                    
                    luaState = new LuaState();
                    luaState.OpenLibs(LuaDLL.luaopen_pb);
                    luaState.OpenLibs(LuaDLL.luaopen_struct);
                    luaState.OpenLibs(LuaDLL.luaopen_lpeg);
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                    luaState.OpenLibs(LuaDLL.luaopen_bit);
#endif
                    if (m_openLuaSocket)
                    {
                        luaState.OpenLibs(LuaDLL.luaopen_socket_core);
                        luaState.OpenLibs(LuaDLL.luaopen_luasocket_scripts);
                    }
                    //-------------------cjson
                    luaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
                    luaState.OpenLibs(LuaDLL.luaopen_cjson);
                    luaState.LuaSetField(-2, "cjson");

                    luaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
                    luaState.LuaSetField(-2, "cjson.safe");
                    //---------------

                    luaState.LuaSetTop(0);

                    LuaBinder.Bind(luaState);

                    luaState.Start();
                    if (!string.IsNullOrEmpty(lhConfigData.luaLoaderConfig.entry.startFileName))
                    {
                        luaState.DoFile(lhConfigData.luaLoaderConfig.entry.startFileName);
                    }
                    if (!string.IsNullOrEmpty(lhConfigData.luaLoaderConfig.entry.startFunction))
                    {
                        LuaFunction main = luaState.GetFunction(lhConfigData.luaLoaderConfig.entry.startFunction);
                        main.Call();
                        main.Dispose();
                        main = null;
                    }
                    OnInitalOver();
                }
            );

        }
        public void Dispose()
        {
            if (luaState != null)
            {
                if (luaState != null)
                {
                    luaState.Dispose();
                    luaState = null;
                }
            }
            m_instance = null;
        }

        #region Static methods
        public static byte[] GetLuaBytes(string fileName)
        {
            if (m_instance.m_luaDic.ContainsKey(fileName))
            {
                return m_instance.m_luaDic[fileName];
            }
            else
            {
                lhDebug.LogError("LaoHan: luabytes is dont exit -> " + fileName);
                return null;
            }
        }
        #endregion

        #region private methods
        #endregion
    }
}