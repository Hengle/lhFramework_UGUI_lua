RaycastBits = 
{
	Collider = 1,
    Normal = 2,
    Point = 4,
    Rigidbody = 8,
    Transform = 16,
    ALL = 31,
}
	
local RaycastHit = 
{
	Collider,
    Normal,
    Point,
    Rigidbody,
    Transform,
    Ctor=function(self,collider, distance, normal, point, rigidbody, transform)
	    self.collider 	= collider
	    self.distance 	= distance
	    self.normal 	= normal
	    self.point 		= point
	    self.rigidbody 	= rigidbody
	    self.transform 	= transform
    end,
    New=function(collider, distance, normal, point, rigidbody, transform)
	    return class:New(RaycastHit,collider, distance, normal, point, rigidbody, transform)
    end,
    Get=function(self)
	    return self.collider, self.distance, self.normal, self.point, self.rigidbody, self.transform
    end,
    Destroy=function(self)				
	    self.collider 	= nil			
	    self.rigidbody 	= nil					
	    self.transform 	= nil		
    end,
    GetMask=function(...)
	    local arg = {...}
	    local value = 0	

	    for i = 1, #arg do		
		    local n = RaycastBits[arg[i]] or 0
		
		    if n ~= 0 then
			    value = value + n				
		    end
	    end	
		
	    if value == 0 then value = RaycastBits["all"] end
	    return value
    end,
    __index = function(t,k)
	    local var = rawget(RaycastHit, k)
		
	    if var == nil then							
		    var = rawget(Get, k)
		
		    if var ~= nil then
			    return var(t)	
		    end
	    end
	    return var
    end,
	
}

--local Get = tolua.initget(RaycastHit)


--setmetatable(RaycastHit, RaycastHit)
return RaycastHit