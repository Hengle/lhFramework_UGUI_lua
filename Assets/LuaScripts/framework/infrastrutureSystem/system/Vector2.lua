--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

local sqrt = Mathf.Sqrt
local setmetatable = setmetatable
local rawset = rawset
local rawget = rawget

Vector2 = 
{
-----------------------------------------------member
    x=0,
    y=0,
-----------------------------------------------construture
    Ctor=function(self,x,y)
        self.x=x or 0
        self.y=y or 0
    end,
-----------------------------------------------public methods
    Set=function(self,x,y)
	    self.x = x or 0
	    self.y = y or 0	
    end,
    Get=function(self)
	    return self.x, self.y
    end,
    SqrMagnitude=function(self)
	    return self.x * self.x + self.y * self.y
    end,
    Clone=function(self)
	    return Vector2.New(self.x, self.y)
    end,
    Normalize=function(self)
	    local v = self:Clone()
	    return v:SetNormalize()	
    end,
    SetNormalize=function(self)
	    local num = self:magnitude()	
	
	    if num == 1 then
		    return self
        elseif num > 1e-05 then    
            self:Div(num)
        else    
            self:Set(0,0)
	    end 

	    return self
    end,
    Div=function(self,d)
	    self.x = self.x / d
	    self.y = self.y / d	
	
	    return self
    end,
    Mul=function(self,d)
	    self.x = self.x * d
	    self.y = self.y * d
	
	    return self
    end,
    Add=function(self,b)
	    self.x = self.x + b.x
	    self.y = self.y + b.y
	
	    return self
    end,
    Sub=function(self,b)
	    self.x = self.x - b.x
	    self.y = self.y - b.y
	
	    return
    end,
-----------------------------------------------static methods
    New=function(x, y)
	    return Class:New(Vector2,x,y)
    end,
    up=function()
        return Vector2.New(0,1)
    end,
    right=function()
        return Vector2.New(1,0)
    end,
    zero=function()
        return Vector2.New(0,0)
    end,
    one=function()
        return Vector2.New(1,1)
    end,
    Dot=function(lhs, rhs)
	    return lhs.x * rhs.x + lhs.y * rhs.y
    end,
    Angle=function(from, to)
	    return acos(clamp(Vector2.Dot(from:Normalize(), to:Normalize()), -1, 1)) * 57.29578
    end,
    magnitude=function(v2)
	    return sqrt(v2.x * v2.x + v2.y * v2.y)
    end,
-----------------------------------------------element function
--    __index = function(t,k)
--	    local var = rawget(Vector2, k)

--	    if var == nil then							
--		    var = rawget(Get, k)

--		    if var ~= nil then
--			    return var(t)
--		    end
--	    end

--	    return var
--    end,
    __call = function(t, x, y)
	    return Vector2.New(x, y)
    end,
    __tostring = function(self)
	    return string.format("[%f,%f]", self.x, self.y)
    end,
    __div = function(va, d)
	    return Vector2.New(va.x / d, va.y / d)
    end,
    __mul = function(va, d)
	    return Vector2.New(va.x * d, va.y * d)
    end,
    __add = function(va, vb)
	    return Vector2.New(va.x + vb.x, va.y + vb.y)
    end,
    __sub = function(va, vb)
	    return Vector2.New(va.x - vb.x, va.y - vb.y)
    end,
    __unm = function(va)
	    return Vector2.New(-va.x, -va.y)
    end,
    __eq = function(va,vb)
	    return va.x == vb.x and va.y == vb.y
    end
}

--local Get = tolua.initget(Vector2)

--Get.up 		= function() return Vector2.New(0,1) end
--Get.right	= function() return Vector2.New(1,0) end
--Get.zero	= function() return Vector2.New(0,0) end
--Get.one		= function() return Vector2.New(1,1) end

--Get.magnitude 		= Vector2.magnitude
--Get.normalized 		= Vector2.Normalize
--Get.SqrMagnitude 	= Vector2.SqrMagnitude

--setmetatable(Vector2, Vector2)