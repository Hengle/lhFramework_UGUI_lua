--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are Subject to the "MIT License"
--------------------------------------------------------------------------------

local acos	= math.acos
local sqrt 	= math.sqrt
local Max 	= math.Max
local Min 	= math.Min
local clamp = Mathf.Clamp
local cos	= math.cos
local sin	= math.sin
local abs	= math.abs
local sign	= Mathf.Sign
local setmetatable = setmetatable
local rawset = rawset
local rawget = rawget

local rad2Deg = Mathf.Rad2Deg
local deg2Rad = Mathf.Deg2Rad
local overSqrt2 = 0.7071067811865475244008443621048490

local function orthoNormalVector(vec)
	local res = Vector3.New()
	
	if abs(vec.z) > overSqrt2 then			
		local a = vec.y * vec.y + vec.z * vec.z
		local k = 1 / sqrt (a)
		res.x = 0
		res.y = -vec.z * k
		res.z = vec.y * k
	else			
		local a = vec.x * vec.x + vec.y * vec.y
		local k = 1 / sqrt (a)
		res.x = -vec.y * k
		res.y = vec.x * k
		res.z = 0
	end
	
	return res
end


Vector3 = {
-----------------------------------------------member
    x=0,
    y=0,
    z=0,
-----------------------------------------------construture
    Ctor=function(self,x,y,z)
        self.x=x or 0
        self.y=y or 0
        self.z=z or 0
    end,
-----------------------------------------------public methods
    Set=function(self,x,y,z)	
	    self.x = x or 0
	    self.y = y or 0
	    self.z = z or 0
    end,
    Get=function(self)			
	    return self.x, self.y, self.z	
    end,
    Clone=function(self)
	    return self:New(self.x, self.y, self.z)
    end,
    magnitude=function (self)
	    return sqrt(self.x * self.x + self.y * self.y + self.z * self.z)
    end,
    SetNormalize=function(self)
	    local num = sqrt(self.x * self.x + self.y * self.y + self.z * self.z)
	
	    if num > 1e-5 then    
            self.x = self.x / num
		    self.y = self.y / num
		    self.z = self.z /num
        else    
		    self.x = 0
		    self.y = 0
		    self.z = 0
	    end 

	    return self
    end,
    SqrMagnitude=function(self)
	    return self.x * self.x + self.y * self.y + self.z * self.z
    end,
    ClampMagnitude=function(self,MaxLength)	
	    if self:SqrMagnitude() > (MaxLength * MaxLength) then    
		    self:SetNormalize()
		    self:Mul(MaxLength)        
        end
        return self
    end,
    Equals=function(self,other)
	    return self.x == other.x and self.y == other.y and self.z == other.z
    end,
    Mul=function (self,q)
	    if type(q) == "number" then
		    self.x = self.x * q
		    self.y = self.y * q
		    self.z = self.z * q
	    else
		    self:MulQuat(q)
	    end
	
	    return self
    end,
    Div=function(self,d)
	    self.x = self.x / d
	    self.y = self.y / d
	    self.z = self.z / d
	
	    return self
    end,
    Add=function(self,vb)
	    self.x = self.x + vb.x
	    self.y = self.y + vb.y
	    self.z = self.z + vb.z
	
	    return self
    end,
    Sub=function(self,vb)
	    self.x = self.x - vb.x
	    self.y = self.y - vb.y
	    self.z = self.z - vb.z
	
	    return self
    end,
    MulQuat=function(self,quat)	   
	    local num 	= quat.x * 2
	    local num2 	= quat.y * 2
	    local num3 	= quat.z * 2
	    local num4 	= quat.x * num
	    local num5 	= quat.y * num2
	    local num6 	= quat.z * num3
	    local num7 	= quat.x * num2
	    local num8 	= quat.x * num3
	    local num9 	= quat.y * num3
	    local num10 = quat.w * num
	    local num11 = quat.w * num2
	    local num12 = quat.w * num3
	
	    local x = (((1 - (num5 + num6)) * self.x) + ((num7 - num12) * self.y)) + ((num8 + num11) * self.z)
	    local y = (((num7 + num12) * self.x) + ((1 - (num4 + num6)) * self.y)) + ((num9 - num10) * self.z)
	    local z = (((num8 - num11) * self.x) + ((num9 + num10) * self.y)) + ((1 - (num4 + num5)) * self.z)
	
	    self:Set(x, y, z)	
	    return self
    end,
-----------------------------------------------static methods
    New=function(x, y, z)
	    return Class:New(Vector3,x,y,z)
    end,
    up=function()
        return Vector3.New(0,1,0)
    end,
    down=function()
        return Vector3.New(0,-1,0)
    end,
    right=function()
        return Vector3.New(0,1,0)
    end,
    left=function()
        return Vector3.New(0,1,0)
    end,
    forward=function()
        return Vector3.New(0,1,0)
    end,
    back=function()
        return Vector3.New(0,1,0)
    end,
    zero=function()
        return Vector3.New(0,1,0)
    end,
    one=function()
        return Vector3.New(0,1,0)
    end,
    Distance=function(va, vb)
	    return sqrt((va.x - vb.x)^2 + (va.y - vb.y)^2 + (va.z - vb.z)^2)
    end,
    Dot=function (lhs, rhs)
	    return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z
    end,
    Lerp=function(from, to, t)	
	    t = clamp(t, 0, 1)
	    return Vector3.New(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t)
    end,
    Max=function (lhs, rhs)
	    return Vector3.New(Max(lhs.x, rhs.x),Max(lhs.y, rhs.y), Max(lhs.z, rhs.z))
    end,
    Min=function(hs, rhs)
	    return Vector3.New(Min(lhs.x, rhs.x),Min(lhs.y, rhs.y), Min(lhs.z, rhs.z))
    end,
    Normalize=function(v)
	    local x,y,z = v.x, v.y, v.z		
	    local num = sqrt(x * x + y * y + z * z)	
	
	    if num > 1e-5 then		
		    return Vector3.New(x/num, y/num, z/num)   			
        end
	  
	    return Vector3.New(0, 0, 0)			
    end,
    Angle=function(from, to)
	    return acos(clamp(Vector3.Dot(from:Normalize(), to:Normalize()), -1, 1)) * rad2Deg
    end,
    OrthoNormalize=function (va, vb, vc)	
	    va:SetNormalize()
	    vb:Sub(vb:Project(va))
	    vb:SetNormalize()
	
	    if vc == nil then
		    return va, vb
	    end
	    vc:Sub(vc:Project(va))
	    vc:Sub(vc:Project(vb))
	    vc:SetNormalize()		
	    return va, vb, vc
    end,
    MoveTowards=function(current, target, MaxDistanceDelta)	
	    local delta = target - current	
        local sqrDelta = delta:SqrMagnitude()
	    local sqrDistance = MaxDistanceDelta * MaxDistanceDelta
	
        if sqrDelta > sqrDistance then    
		    local magnitude = sqrt(sqrDelta)
		
		    if magnitude > 1e-6 then
			    delta:Mul(MaxDistanceDelta / magnitude)
			    delta:Add(current)
			    return delta
		    else
			    return current:Cone()
		    end
        end
	
        return target:Cone()
    end,
    ClampedMove=function (lhs, rhs, clampedDelta)
	    local delta = rhs - lhs
	
	    if delta > 0 then
		    return lhs + Min(delta, clampedDelta)
	    else
		    return lhs - Min(-delta, clampedDelta)
	    end
    end,
    RotateTowards=function(current, target, MaxRadiansDelta, MaxMagnitudeDelta)
	    local len1 = current:magnitude()
	    local len2 = target:magnitude()
	
	    if len1 > 1e-6 and len2 > 1e-6 then	
		    local from = current / len1
		    local to = target / len2		
		    local cosom = Vector3.Dot(from, to)
				
		    if cosom > 1 - 1e-6 then		
			    return Vector3.MoveTowards (current, target, MaxMagnitudeDelta)		
		    elseif cosom < -1 + 1e-6 then		
			    local axis = orthoNormalVector(from)						
			    local q = Quaternion.AngleAxis(MaxRadiansDelta * rad2Deg, axis)	
			    local rotated = q:mulVec3(from)
			    local delta = Vector3.ClampedMove(len1, len2, MaxMagnitudeDelta)
			    rotated:Mul(delta)
			    return rotated
		    else		
			    local Angle = acos(cosom)
			    local axis = Vector3.Cross(from, to)
			    axis:SetNormalize ()
			    local q = Quaternion.AngleAxis(Min(MaxRadiansDelta, Angle) * rad2Deg, axis)			
			    local rotated = q:mulVec3(from)
			    local delta = Vector3.ClampedMove(len1, len2, MaxMagnitudeDelta)
			    rotated:Mul(delta)
			    return rotated
		    end
	    end
		
	    return Vector3.MoveTowards(current, target, MaxMagnitudeDelta)
    end,
    SmoothDamp=function(current, target, currentVelocity, smoothTime)
	    local MaxSpeed = Mathf.Infinity
	    local deltaTime = Time.deltaTime
        smoothTime = Max(0.0001, smoothTime)
        local num = 2 / smoothTime
        local num2 = num * deltaTime
        local num3 = 1 / (1 + num2 + 0.48 * num2 * num2 + 0.235 * num2 * num2 * num2)    
        local vector2 = target:Cone()
        local MaxLength = MaxSpeed * smoothTime
	    local vector = current - target
        vector:ClampMagnitude(MaxLength)
        target = current - vector
        local vec3 = (currentVelocity + (vector * num)) * deltaTime
        currentVelocity = (currentVelocity - (vec3 * num)) * num3
        local vector4 = target + (vector + vec3) * num3	
	
        if Vector3.Dot(vector2 - current, vector4 - vector2) > 0 then    
            vector4 = vector2
            currentVelocity:Set(0,0,0)
        end
	
        return vector4, currentVelocity
    end	,
    Scale=function(a, b)
	    local x = a.x * b.x
	    local y = a.y * b.y
	    local z = a.z * b.z	
	    return Vector3.New(x, y, z)
    end,
    Cross=function(lhs, rhs)
	    local x = lhs.y * rhs.z - lhs.z * rhs.y
	    local y = lhs.z * rhs.x - lhs.x * rhs.z
	    local z = lhs.x * rhs.y - lhs.y * rhs.x
	    return Vector3.New(x,y,z)	
    end,
    Reflect=function (inDirection, inNormal)
	    local num = -2 * Vector3.Dot(inNormal, inDirection)
	    inNormal = inNormal * num
	    inNormal:Add(inDirection)
	    return inNormal
    end,
    Project=function(vector, onNormal)
	    local num = onNormal:SqrMagnitude()
	
	    if num < 1.175494e-38 then	
		    return Vector3.New(0,0,0)
	    end
	
	    local num2 = Vector3.Dot(vector, onNormal)
	    local v3 = onNormal:Cone()
	    v3:Mul(num2/num)	
	    return v3
    end,
    ProjectOnPlane=function(vector, planeNormal)
	    local v3 = Vector3.Project(vector, planeNormal)
	    v3:Mul(-1)
	    v3:Add(vector)
	    return v3
    end,
    Slerp=function(from, to, t)
	    local omega, sinom, Scale0, Scale1

	    if t <= 0 then		
		    return from:Cone()
	    elseif t >= 1 then		
		    return to:Cone()
	    end
	
	    local v2 	= to:Cone()
	    local v1 	= from:Cone()
	    local len2 	= to:magnitude()
	    local len1 	= from:magnitude()	
	    v2:Div(len2)
	    v1:Div(len1)

	    local len 	= (len2 - len1) * t + len1
	    local cosom = Vector3.Dot(v1, v2)
	
	    if 1 - cosom > 1e-6 then
		    omega 	= acos(cosom)
		    sinom 	= sin(omega)
		    Scale0 	= sin((1 - t) * omega) / sinom
		    Scale1 	= sin(t * omega) / sinom
	    else 
		    Scale0 = 1 - t
		    Scale1 = t
	    end

	    v1:Mul(Scale0)
	    v2:Mul(Scale1)
	    v2:Add(v1)
	    v2:Mul(len)
	    return v2
    end,
    AngleAroundAxis=function(from, to, axis)	 	 
	    from = from - Vector3.Project(from, axis)
	    to = to - Vector3.Project(to, axis) 	    
	    local Angle = Vector3.Angle (from, to)	   	    
	    return Angle * (Vector3.Dot (axis, Vector3.Cross (from, to)) < 0 and -1 or 1)
    end,
-----------------------------------------------element function
    __tostring = function(self)
	    return "["..self.x..","..self.y..","..self.z.."]"
    end,
    __div = function(va, d)
	    return Vector3.New(va.x / d, va.y / d, va.z / d)
    end,
    __mul = function(va, d)
	    if type(d) == "number" then
		    return Vector3.New(va.x * d, va.y * d, va.z * d)
	    else
		    local vec = va:Cone()
		    vec:MulQuat(d)
		    return vec
	    end	
    end,
    __add = function(va, vb)
	    return Vector3.New(va.x + vb.x, va.y + vb.y, va.z + vb.z)
    end,
    __Sub = function(va, vb)
	    return Vector3.New(va.x - vb.x, va.y - vb.y, va.z - vb.z)
    end,
    __unm = function(va)
	    return Vector3.New(-va.x, -va.y, -va.z)
    end,
    __eq = function(a,b)
	    local v = a - b
	    local delta = v:SqrMagnitude()
	    return delta < 1e-10
    end,
--    __index = function(t,k)
--	    local var = rawget(Vector3, k)

--	    if var == nil then						
--		    var = rawget(Get, k)		

--		    if var ~= nil then
--			    return var(t)				
--		    end		
--	    end

--	    return var
--    end,

    __call = function(t,x,y,z)
	    return Vector3.New(x,y,z)
    end
}

--local Get = tolua.initget(Vector3)


--Get.up 		= function() return Vector3.New(0,1,0) end
--Get.down 	= function() return Vector3.New(0,-1,0) end
--Get.right	= function() return Vector3.New(1,0,0) end
--Get.left	= function() return Vector3.New(-1,0,0) end
--Get.forward = function() return Vector3.New(0,0,1) end
--Get.back	= function() return Vector3.New(0,0,-1) end
--Get.zero	= function() return Vector3.New(0,0,0) end
--Get.one		= function() return Vector3.New(1,1,1) end

--Get.magnitude	= Vector3.Magnitude
--Get.Normalized	= Vector3.Normalize
--Get.SqrMagnitude= Vector3.SqrMagnitude

--setmetatable(Vector3, Vector3)
--return Vector3
