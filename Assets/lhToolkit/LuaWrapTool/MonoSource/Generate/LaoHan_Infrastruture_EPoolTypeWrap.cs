﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LaoHan.Infrastruture.ulua;

public class LaoHan_Infrastruture_EPoolTypeWrap
{
	public static void Register(LuaState L)
	{
		L.BeginEnum(typeof(LaoHan.Infrastruture.EPoolType));
		L.RegVar("None", get_None, null);
		L.RegVar("UI", get_UI, null);
		L.RegVar("Effect", get_Effect, null);
		L.RegVar("Sound3D", get_Sound3D, null);
		L.RegFunction("IntToEnum", IntToEnum);
		L.EndEnum();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_None(IntPtr L)
	{
		ToLua.Push(L, LaoHan.Infrastruture.EPoolType.None);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UI(IntPtr L)
	{
		ToLua.Push(L, LaoHan.Infrastruture.EPoolType.UI);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Effect(IntPtr L)
	{
		ToLua.Push(L, LaoHan.Infrastruture.EPoolType.Effect);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Sound3D(IntPtr L)
	{
		ToLua.Push(L, LaoHan.Infrastruture.EPoolType.Sound3D);
		return 1;
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		LaoHan.Infrastruture.EPoolType o = (LaoHan.Infrastruture.EPoolType)arg0;
		ToLua.Push(L, o);
		return 1;
	}
}
