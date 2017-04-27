--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
--if jit then		
--	if jit.opt then
--		jit.opt.start(3)	
--		print("jit opt to 3")
--	end
--	print("jit", jit.status())
--	print(string.format("os: %s, arch: %s", jit.os, jit.arch))
--end

require "framework/infrastrutureSystem/system/Class"

require "framework/infrastrutureSystem/system/Mathf"
require "framework/infrastrutureSystem/system/Vector3"
require "framework/infrastrutureSystem/system/Quaternion"
require "framework/infrastrutureSystem/system/Vector2"
require "framework/infrastrutureSystem/system/Vector4"
Color		= require "framework/infrastrutureSystem/system/Color"
Ray			= require "framework/infrastrutureSystem/system/Ray"
Bounds		= require "framework/infrastrutureSystem/system/Bounds"
RaycastHit	= require "framework/infrastrutureSystem/system/RaycastHit"
Touch		= require "framework/infrastrutureSystem/system/Touch"
list		= require "framework/infrastrutureSystem/system/list"
Time		= require "framework/infrastrutureSystem/system/Time"
LayerMask	= require "framework/infrastrutureSystem/system/LayerMask"
utf8		= require "framework/infrastrutureSystem/system/utf8"

require "framework/infrastrutureSystem/system/slot"
require "framework/infrastrutureSystem/system/typeof"
require "framework/infrastrutureSystem/system/event"
require "framework/infrastrutureSystem/system/Timer"
require "framework/infrastrutureSystem/system/coroutine"
require "framework/infrastrutureSystem/system/Plane"
require "framework/infrastrutureSystem/system/ValueType"

--require "misc/strict"