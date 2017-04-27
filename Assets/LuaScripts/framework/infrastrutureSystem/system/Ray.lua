local rawget = rawget
local setmetatable = setmetatable

local Ray = 
{	
    direction, 
    origin,
    Ctor=function(self,direction, origin)
        self.direction=direction or Vector3.zero
        self.origin=origin
    end,
    New=function(direction, origin)
	    return class:New(Ray,direction,origin)
    end,
    GetPoint=function(self,distance)
	    local dir = self.direction * distance
	    dir:Add(self.origin)
	    return dir
    end,
    Get=function(self)		
	    return self.origin, self.direction
    end,
    __index = function(t,k)
	    local var = rawget(Ray, k)
		
	    if var == nil then							
		    var = rawget(Get, k)
		
		    if var ~= nil then
			    return var(t)	
		    end
	    end
	
	    return var
    end,
    __call = function(t, direction, origin)
	    return Ray.New(direction, origin)
    end,
    _tostring = function(self)
	    return string.format("Origin:(%f,%f,%f),Dir:(%f,%f, %f)", self.origin.x, self.origin.y, self.origin.z, self.direction.x, self.direction.y, self.direction.z)
    end
}

--local Get = tolua.initget(Ray)


--setmetatable(Ray, Ray)
return Ray