--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

local clamp	= Mathf.Clamp
local sqrt	= Mathf.Sqrt
local Min	= Mathf.Min
local Max 	= Mathf.Max
local setmetatable = setmetatable
local rawget = rawget

Vector4 = 
{
-----------------------------------------------member
    x,
    y,
    z,
    w,
-----------------------------------------------construture
    Ctor=function(self,x,y,z,w)
        self.x=x or 0
        self.y=y or 0
        self.z=z or 0
        self.w=w or 0
    end,
-----------------------------------------------public methods
    Set=function(self,x,y,z,w)
	    self.x = x or 0
	    self.y = y or 0	
	    self.z = z or 0
	    self.w = w or 0
    end,
    Get=function(self)
	    return self.x, self.y, self.z, self.w
    end,
    SetScale=function(self,Scale)
	    self.x = self.x * Scale.x
	    self.y = self.y * Scale.y
	    self.z = self.z * Scale.z
	    self.w = self.w * Scale.w
    end,
    normalize=function(self)
	    local v = vector4.New(self.x, self.y, self.z, self.w)
	    return v:SetNormalize()
    end,
    SetNormalize=function(self)
	    local num = self:Magnitude()	
	
	    if num == 1 then
		    return self
        elseif num > 1e-05 then    
            self:Div(num)
        else    
            self:Set(0,0,0,0)
	    end 

	    return self
    end,
    Div=function(self,d)
	    self.x = self.x / d
	    self.y = self.y / d	
	    self.z = self.z / d
	    self.w = self.w / d
	
	    return self
    end,
    Mul=function(self,d)
	    self.x = self.x * d
	    self.y = self.y * d
	    self.z = self.z * d
	    self.w = self.w * d	
	
	    return self
    end,
    Add=function(self,b)
	    self.x = self.x + b.x
	    self.y = self.y + b.y
	    self.z = self.z + b.z
	    self.w = self.w + b.w
	
	    return self
    end,
    Sub=function(self,b)
	    self.x = self.x - b.x
	    self.y = self.y - b.y
	    self.z = self.z - b.z
	    self.w = self.w - b.w
	
	    return self
    end,
-----------------------------------------------static methods
    New=function(x, y, z, w)
	    return Class:New(Vector4,x,y,z,w)
    end,
    zero=function()
        return Vector4.New(0, 0, 0, 0)
    end,
    one=function()
        return Vector4.New(1,1,1, 1)
    end,
    Lerp=function(from, to, t)    
        t = clamp(t, 0, 1)
        return Vector4.New(from.x + ((to.x - from.x) * t), from.y + ((to.y - from.y) * t), from.z + ((to.z - from.z) * t), from.w + ((to.w - from.w) * t))
    end ,
    MoveTowards=function(current, target, maxDistanceDelta)    
	    local vector = target - current
	    local Magnitude = vector:Magnitude()	
	
	    if Magnitude > maxDistanceDelta and Magnitude ~= 0 then     
		    maxDistanceDelta = maxDistanceDelta / Magnitude
		    vector:Mul(maxDistanceDelta)   
		    vector:Add(current)
		    return vector
	    end
	
	    return target
    end,
    Scale=function(a, b)    
        return Vector4.New(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w)
    end ,
    Dot=function(a, b)
	    return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w
    end,
    Project=function(a, b)
	    local s = Vector4.Dot(a, b) / Vector4.Dot(b, b)
	    return b * s
    end,
    Distance=function(a, b)
	    local v = a - b
	    return Vector4.Magnitude(v)
    end,
    Magnitude=function(a)
	    return sqrt(a.x * a.x + a.y * a.y + a.z * a.z + a.w * a.w)
    end,
    SqrMagnitude=function(a)
	    return a.x * a.x + a.y * a.y + a.z * a.z + a.w * a.w
    end,
    Min=function(lhs, rhs)
	    return Vector4.New(Max(lhs.x, rhs.x), Max(lhs.y, rhs.y), Max(lhs.z, rhs.z), Max(lhs.w, rhs.w))
    end,
    Max=function(lhs, rhs)
	    return Vector4.New(Min(lhs.x, rhs.x), Min(lhs.y, rhs.y), Min(lhs.z, rhs.z), Min(lhs.w, rhs.w))
    end,
-----------------------------------------------element function
    __index = function(t,k)
	    local var = rawget(Vector4, k)
	
	    if var == nil then							
		    var = rawget(Get, k)
		
		    if var ~= nil then
			    return var(t)	
		    end
	    end
	
	    return var
    end,
    __call = function(t, x, y, z, w)
	    return Vector4.New(x, y, z, w)
    end,
    __tostring = function(self)
	    return string.format("[%f,%f,%f,%f]", self.x, self.y, self.z, self.w)
    end,
    __div = function(va, d)
	    return Vector4.New(va.x / d, va.y / d, va.z / d, va.w / d)
    end,
    __mul = function(va, d)
	    return Vector4.New(va.x * d, va.y * d, va.z * d, va.w * d)
    end,
    __add = function(va, vb)
	    return Vector4.New(va.x + vb.x, va.y + vb.y, va.z + vb.z, va.w + vb.w)
    end,
    __sub = function(va, vb)
	    return Vector4.New(va.x - vb.x, va.y - vb.y, va.z - vb.z, va.w - vb.w)
    end,
    __unm = function(va)
	    return Vector4.New(-va.x, -va.y, -va.z, -va.w)
    end,
    __eq = function(va,vb)
	    local v = va - vb
	    local delta = Vector4.SqrMagnitude(v)	
	    return delta < 1e-10
    end
}

local Get = tolua.initget(Vector4)


--Get.zero = function() return Vector4.New(0, 0, 0, 0) end
--Get.one	 = function() return Vector4.New(1, 1, 1, 1) end

--Get.Magnitude 	 = Vector4.Magnitude
--Get.normalized 	 = Vector4.normalize
--Get.SqrMagnitude = Vector4.SqrMagnitude

--setmetatable(Vector4, Vector4)