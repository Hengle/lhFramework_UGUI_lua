--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

local sin 	= math.sin
local cos 	= math.cos
local acos 	= math.acos
local asin 	= math.asin
local sqrt 	= math.sqrt
local min	= math.min
local max 	= math.max
local sign	= math.sign
local atan2 = math.atan2
local clamp = Mathf.Clamp
local abs	= math.abs
local setmetatable = setmetatable
local getmetatable = getmetatable
local rawget = rawget
local rawset = rawset

local rad2Deg = Mathf.Rad2Deg
local halfDegToRad = 0.5 * Mathf.Deg2Rad
local _forward = Vector3.forward
local _up = Vector3.up
local _next = { 2, 3, 1 }

local pi = Mathf.PI
local half_pi = pi * 0.5
local two_pi = 2 * pi
local negativeFlip = -0.0001
local positiveFlip = two_pi - 0.0001
	

local function MatrixToQuaternion(rot, quat)
	local trace = rot[1][1] + rot[2][2] + rot[3][3]
	
	if trace > 0 then		
		local s = sqrt(trace + 1)
		quat.w = 0.5 * s
		s = 0.5 / s
		quat.x = (rot[3][2] - rot[2][3]) * s
		quat.y = (rot[1][3] - rot[3][1]) * s
		quat.z = (rot[2][1] - rot[1][2]) * s--]]
		quat:SetNormalize()
	else
		local i = 1		
		local q = {0, 0, 0}
		
		if rot[2][2] > rot[1][1] then			
			i = 2			
		end
		
		if rot[3][3] > rot[i][i] then
			i = 3			
		end
		
		local j = _next[i]
		local k = _next[j]
		
		local t = rot[i][i] - rot[j][j] - rot[k][k] + 1
		local s = 0.5 / sqrt(t)
		q[i] = s * t
		local w = (rot[k][j] - rot[j][k]) * s
		q[j] = (rot[j][i] + rot[i][j]) * s
		q[k] = (rot[k][i] + rot[i][k]) * s
		
		quat:Set(q[1], q[2], q[3], w)			
		quat:SetNormalize()		
	end
end
local function UnclampedSlerp(from, to, t)		
	local cosAngle = Quaternion.Dot(from, to)
	
    if cosAngle < 0 then    
        cosAngle = -cosAngle
        to = Quaternion.New(-to.x, -to.y, -to.z, -to.w)
    end
    
    local t1, t2
    
    if cosAngle < 0.95 then    
	    local Angle 	= acos(cosAngle)
		local sinAngle 	= sin(Angle)
        local invSinAngle = 1 / sinAngle
        t1 = sin((1 - t) * Angle) * invSinAngle
        t2 = sin(t * Angle) * invSinAngle    
		local quat = Quaternion.New(from.x * t1 + to.x * t2, from.y * t1 + to.y * t2, from.z * t1 + to.z * t2, from.w * t1 + to.w * t2)
		return quat
    else    
		return Quaternion.Lerp(from, to, t)
    end   	
end
local function Approximately(f0, f1)
	return abs(f0 - f1) < 1e-6	
end
local function SanitizeEuler(Euler)	
	if Euler.x < negativeFlip then
		Euler.x = Euler.x + two_pi
	elseif Euler.x > positiveFlip then
		Euler.x = Euler.x - two_pi
	end

	if Euler.y < negativeFlip then
		Euler.y = Euler.y + two_pi
	elseif Euler.y > positiveFlip then
		Euler.y = Euler.y - two_pi
	end

	if Euler.z < negativeFlip then
		Euler.z = Euler.z + two_pi
	elseif Euler.z > positiveFlip then
		Euler.z = Euler.z + two_pi
	end
end

--from http://www.geometrictools.com/Documentation/EulerAngles.pdf
--Order of rotations: YXZ, 原来的ZXY顺序

Quaternion = 
{

-----------------------------------------------member
    x=0,
    y=0,
    z=0,
    w=0,
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
    Cone=function(self)
	    return Quaternion.New(self.x, self.y, self.z, self.w)
    end,
    Get=function(self)
	    return self.x, self.y, self.z, self.w
    end,
    SetEuler=function(self,x, y, z)		
	    if y == nil and z == nil then		
		    y = x.y
		    z = x.z	
		    x = x.x
	    end
		
	    x = x * halfDegToRad
        y = y * halfDegToRad
        z = z * halfDegToRad
	
	    local sinX = sin(x)
        local cosX = cos(x)
        local sinY = sin(y)
        local cosY = cos(y)
        local sinZ = sin(z)
        local cosZ = cos(z)
    
        self.w = cosY * cosX * cosZ + sinY * sinX * sinZ
        self.x = cosY * sinX * cosZ + sinY * cosX * sinZ
        self.y = sinY * cosX * cosZ - cosY * sinX * sinZ
        self.z = cosY * cosX * sinZ - sinY * sinX * cosZ
	
	    return self
    end,
    Normalize=function(self)
	    local quat = self:Cone()
	    quat:SetNormalize()
	    return quat
    end,
    SetNormalize=function(self)
	    local n = self.x * self.x + self.y * self.y + self.z * self.z + self.w * self.w
	
	    if n ~= 1 and n > 0 then
		    n = 1 / sqrt(n)
		    self.x = self.x * n
		    self.y = self.y * n
		    self.z = self.z * n
		    self.w = self.w * n		
	    end
    end,
    --设置当前四元数为 from 到 to的旋转, 注意from和to同 Forward平行会同unity不一致
    SetFromToRotation1=function(self,from, to)
	    local v0 = from:Normalize()
	    local v1 = to:Normalize()
	    local d = Vector3.Dot(v0, v1)

	    if d > -1 + 1e-6 then	
		    local s = sqrt((1+d) * 2)
		    local invs = 1 / s
		    local c = Vector3.Cross(v0, v1) * invs
		    self:Set(c.x, c.y, c.z, s * 0.5)	
	    elseif d > 1 - 1e-6 then
		    return Quaternion.New(0, 0, 0, 1)
	    else
		    local axis = Vector3.Cross(Vector3.right, v0)
		
		    if axis:SqrMagnitude() < 1e-6 then
			    axis = Vector3.Cross(Vector3.Forward, v0)
		    end

		    self:Set(axis.x, axis.y, axis.z, 0)		
		    return self
	    end
	
	    return self
    end,
    SetFromToRotation=function(self,from, to)
	    from = from:Normalize()
	    to = to:Normalize()
	
	    local e = Vector3.Dot(from, to)
	
	    if e > 1 - 1e-6 then
		    self:Set(0, 0, 0, 1)
	    elseif e < -1 + 1e-6 then		
		    local left = {0, from.z, from.y}	
		    local mag = left[2] * left[2] + left[3] * left[3]  --+ left[1] * left[1] = 0
		
		    if mag < 1e-6 then		
			    left[1] = -from.z
			    left[2] = 0
			    left[3] = from.x
			    mag = left[1] * left[1] + left[3] * left[3]
		    end
				
		    local invlen = 1/sqrt(mag)
		    left[1] = left[1] * invlen
		    left[2] = left[2] * invlen
		    left[3] = left[3] * invlen
		
		    local up = {0, 0, 0}
		    up[1] = left[2] * from.z - left[3] * from.y
		    up[2] = left[3] * from.x - left[1] * from.z
		    up[3] = left[1] * from.y - left[2] * from.x
				

		    local fxx = -from.x * from.x
		    local fyy = -from.y * from.y
		    local fzz = -from.z * from.z
		
		    local fxy = -from.x * from.y
		    local fxz = -from.x * from.z
		    local fyz = -from.y * from.z

		    local uxx = up[1] * up[1]
		    local uyy = up[2] * up[2]
		    local uzz = up[3] * up[3]
		    local uxy = up[1] * up[2]
		    local uxz = up[1] * up[3]
		    local uyz = up[2] * up[3]

		    local lxx = -left[1] * left[1]
		    local lyy = -left[2] * left[2]
		    local lzz = -left[3] * left[3]
		    local lxy = -left[1] * left[2]
		    local lxz = -left[1] * left[3]
		    local lyz = -left[2] * left[3]
		
		    local rot = 
		    {
			    {fxx + uxx + lxx, fxy + uxy + lxy, fxz + uxz + lxz},
			    {fxy + uxy + lxy, fyy + uyy + lyy, fyz + uyz + lyz},
			    {fxz + uxz + lxz, fyz + uyz + lyz, fzz + uzz + lzz},
		    }
		
		    MatrixToQuaternion(rot, self)		
	    else
		    local v = Vector3.Cross(from, to)
		    local h = (1 - e) / Vector3.Dot(v, v) 
		
		    local hx = h * v.x
		    local hz = h * v.z
		    local hxy = hx * v.y
		    local hxz = hx * v.z
		    local hyz = hz * v.y
		
		    local rot = 
		    { 					
			    {e + hx*v.x, 	hxy - v.z, 		hxz + v.y},
			    {hxy + v.z,  	e + h*v.y*v.y, 	hyz-v.x},
			    {hxz - v.y,  	hyz + v.x,    	e + hz*v.z},
		    }
		
		    MatrixToQuaternion(rot, self)
	    end
    end,
    Inverse=function(self)
	    local quat = Quaternion.New()
		
	    quat.x = -self.x
	    quat.y = -self.y
	    quat.z = -self.z
	    quat.w = self.w
	
	    return quat
    end,
    SetIdentity=function (self)
	    self.x = 0
	    self.y = 0
	    self.z = 0
	    self.w = 1
    end,
    ToAngleAxis=function (self)		
	    local Angle = 2 * acos(self.w)
	
	    if Approximately(Angle, 0) then
		    return Angle * 57.29578, Vector3.New(1, 0, 0)
	    end
	
	    local div = 1 / sqrt(1 - sqrt(self.w))
	    return Angle * 57.29578, Vector3.New(self.x * div, self.y * div, self.z * div)
    end,
    ToEulerAngles=function(self)
	    local x = self.x
	    local y = self.y
	    local z = self.z
	    local w = self.w
		
	    local check = 2 * (y * z - w * x)
	
	    if check < 0.999 then
		    if check > -0.999 then
			    local v = Vector3.New( -asin(check), 
						    atan2(2 * (x * z + w * y), 1 - 2 * (x * x + y * y)), 
						    atan2(2 * (x * y + w * z), 1 - 2 * (x * x + z * z)))
			    SanitizeEuler(v)
			    v:Mul(rad2Deg)
			    return v
		    else
			    local v = Vector3.New(half_pi, atan2(2 * (x * y - w * z), 1 - 2 * (y * y + z * z)), 0)
			    SanitizeEuler(v)
			    v:Mul(rad2Deg)
			    return v
		    end
	    else
		    local v = Vector3.New(-half_pi, atan2(-2 * (x * y - w * z), 1 - 2 * (y * y + z * z)), 0)
		    SanitizeEuler(v)
		    v:Mul(rad2Deg)
		    return v		
	    end
    end,
    Forward=function(self)
	    return self:MulVec3(_forward)
    end,
    MulVec3=function(self, point)
	    local vec = Vector3.New()
    
	    local num 	= self.x * 2
	    local num2 	= self.y * 2
	    local num3 	= self.z * 2
	    local num4 	= self.x * num
	    local num5 	= self.y * num2
	    local num6 	= self.z * num3
	    local num7 	= self.x * num2
	    local num8 	= self.x * num3
	    local num9 	= self.y * num3
	    local num10 = self.w * num
	    local num11 = self.w * num2
	    local num12 = self.w * num3
	
	    vec.x = (((1 - (num5 + num6)) * point.x) + ((num7 - num12) * point.y)) + ((num8 + num11) * point.z)
	    vec.y = (((num7 + num12) * point.x) + ((1 - (num4 + num6)) * point.y)) + ((num9 - num10) * point.z)
	    vec.z = (((num8 - num11) * point.x) + ((num9 + num10) * point.y)) + ((1 - (num4 + num5)) * point.z)
	
	    return vec
    end,
-----------------------------------------------static methods
    New=function(x, y, z, w)
	    return Class:New(Quaternion,x,y,z,w)
    end,
    Dot=function(a, b)
	    return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w
    end,
    Angle=function (a, b)
	    local Dot = Quaternion.Dot(a, b)
	    if Dot < 0 then Dot = -Dot end
	    return acos(min(Dot, 1)) * 2 * 57.29578	
    end,
    AngleAxis=function(Angle, axis)
	    local normAxis = axis:Normalize()
        Angle = Angle * halfDegToRad
        local s = sin(Angle)    
    
        local w = cos(Angle)
        local x = normAxis.x * s
        local y = normAxis.y * s
        local z = normAxis.z * s
	
	    return Quaternion.New(x,y,z,w)
    end,
    Equals=function(a, b)
	    return a.x == b.x and a.y == b.y and a.z == b.z and a.w == b.w
    end,
    Euler=function(x, y, z)		
	    local quat = Quaternion.New()	
	    quat:SetEuler(x,y,z)
	    return quat
    end,
    --产生一个新的从from到to的四元数
    FromToRotation=function(from, to)
	    local quat = Quaternion.New()
	    quat:SetFromToRotation(from, to)
	    return quat
    end,
    Lerp=function(q1, q2, t)
	    t = clamp(t, 0, 1)
	    local q = Quaternion.New()	
	
	    if Quaternion.Dot(q1, q2) < 0 then
		    q.x = q1.x + t * (-q2.x -q1.x)
		    q.y = q1.y + t * (-q2.y -q1.y)
		    q.z = q1.z + t * (-q2.z -q1.z)
		    q.w = q1.w + t * (-q2.w -q1.w)
	    else
		    q.x = q1.x + (q2.x - q1.x) * t
		    q.y = q1.y + (q2.y - q1.y) * t
		    q.z = q1.z + (q2.z - q1.z) * t
		    q.w = q1.w + (q2.w - q1.w) * t
	    end	
	
	    q:SetNormalize()
	    return q
    end,
    LookRotation=function(forward, up)
	    local mag = forward:Magnitude()
	    if mag < 1e-6 then
		    error("error input Forward to Quaternion.LookRotation" + tostring(Forward))
		    return nil
	    end
	
	    forward = forward / mag
	    up = up or _up				
	    local right = Vector3.Cross(up, forward)
	    right:SetNormalize()    
        up = Vector3.Cross(forward, right)
        right = Vector3.Cross(up, forward)	
	
    --[[	local quat = Quaternion.New(0,0,0,1)
	    local rot = 
	    { 					
		    {right.x, up.x, Forward.x},
		    {right.y, up.y, Forward.y},
		    {right.z, up.z, Forward.z},
	    }
	
	    MatrixToQuaternion(rot, quat)
	    return quat--]]
		
	    local t = right.x + up.y + Forward.z
    
	    if t > 0 then		
		    local x, y, z, w
		    t = t + 1
		    local s = 0.5 / sqrt(t)		
		    w = s * t
		    x = (up.z - Forward.y) * s		
		    y = (Forward.x - right.z) * s
		    z = (right.y - up.x) * s
		
		    local ret = Quaternion.New(x, y, z, w)	
		    ret:SetNormalize()
		    return ret
	    else
		    local rot = 
		    { 					
			    {right.x, up.x, Forward.x},
			    {right.y, up.y, Forward.y},
			    {right.z, up.z, Forward.z},
		    }
	
		    local q = {0, 0, 0}
		    local i = 1		
		
		    if up.y > right.x then			
			    i = 2			
		    end
		
		    if Forward.z > rot[i][i] then
			    i = 3			
		    end
		
		    local j = _next[i]
		    local k = _next[j]
		
		    local t = rot[i][i] - rot[j][j] - rot[k][k] + 1
		    local s = 0.5 / sqrt(t)
		    q[i] = s * t
		    local w = (rot[k][j] - rot[j][k]) * s
		    q[j] = (rot[j][i] + rot[i][j]) * s
		    q[k] = (rot[k][i] + rot[i][k]) * s
		
		    local ret = Quaternion.New(q[1], q[2], q[3], w)			
		    ret:SetNormalize()
		    return ret
	    end
    end,
    Slerp=function(from, to, t)	
	    t = clamp(t, 0, 1)
	    return UnclampedSlerp(from, to, t)
    end,
    RotateTowards=function(from, to, maxDegreesDelta)   	
	    local Angle = Quaternion.Angle(from, to)
	
	    if Angle == 0 then
		    return to
	    end
	
	    local t = min(1, maxDegreesDelta / Angle)
	    return UnclampedSlerp(from, to, t)
    end,
-----------------------------------------------element function
    __index = function(t, k)		
	    local var = rawget(Quaternion, k)
	
	    if var == nil then							
		    var = rawget(Get, k)
		
		    if var ~= nil then
			    return var(t)	
		    end
	    end

	    return var
    end,
--    __newindex = function(t, name, k)	
--	    if name == "eulerAngles" then
--		    t:SetEuler(k)
--	    else
--		    rawset(t, name, k)
--	    end	
--    end,
    __call = function(t, x, y, z, w)
	    return Quaternion.New(x, y, z, w)
    end,
    __mul = function(lhs, rhs)
	    if Quaternion == getmetatable(rhs) then
		    return Quaternion.New((((lhs.w * rhs.x) + (lhs.x * rhs.w)) + (lhs.y * rhs.z)) - (lhs.z * rhs.y), (((lhs.w * rhs.y) + (lhs.y * rhs.w)) + (lhs.z * rhs.x)) - (lhs.x * rhs.z), (((lhs.w * rhs.z) + (lhs.z * rhs.w)) + (lhs.x * rhs.y)) - (lhs.y * rhs.x), (((lhs.w * rhs.w) - (lhs.x * rhs.x)) - (lhs.y * rhs.y)) - (lhs.z * rhs.z))	
	    elseif Vector3 == getmetatable(rhs) then
		    return lhs:MulVec3(rhs)
	    end
    end,
    __unm = function(q)
	    return Quaternion.New(-q.x, -q.y, -q.z, -q.w)
    end,
    __eq = function(lhs,rhs)
	    return Quaternion.Dot(lhs, rhs) > 0.999999
    end,
    __tostring = function(self)
	    return "["..self.x..","..self.y..","..self.z..","..self.w.."]"
    end
}

--local Get = tolua.initget(Quaternion)
--Get.identity = function() return Quaternion.New(0, 0, 0, 1) end
--Get.eulerAngles = Quaternion.ToEulerAngles
