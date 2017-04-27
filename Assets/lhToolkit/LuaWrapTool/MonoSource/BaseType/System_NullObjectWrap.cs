using System;
using LaoHan.Infrastruture.ulua;

public class System_NullObjectWrap
{
    public static void Register(LuaState L)
    {
        L.BeginClass(typeof(NullObject), null);        
        L.EndClass();
    }
}
