using System;
using LaoHan.Infrastruture.ulua;

public class LuaInterface_LuaOutWrap
{
    public static void Register(LuaState L)
    {
        L.BeginClass(typeof(LuaOutMetatable), null);        
        L.EndClass();
    }
}
