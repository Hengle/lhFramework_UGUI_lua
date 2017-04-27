local Layer = Layer
local rawget = rawget
--local setmetatable = setmetatable

local LayerMask =
{
-----------------------------------------------member
	value = 0,
-----------------------------------------------construture
    Ctor=function ()
        
    end,
-----------------------------------------------public methods
    Get=function(self)
	    return self.value
    end,
-----------------------------------------------static methods
    GetMask=function(...)
	    local arg = {...}
	    local value = 0	

	    for i = 1, #arg do		
		    local n = LayerMask.NameToLayer(arg[i])
		
		    if n ~= 0 then
			    value = value + 2 ^ n				
		    end
	    end	
		
	    return value
    end,
    New=function(value)
        return class:New(LayerMask)
    end,
    NameToLayer=function(name)
	    return Layer[name]
    end,
-----------------------------------------------element function
    __index = function(t,k)
	    local var = rawget(LayerMask, k)
	
	    if var == nil then							
		    var = rawget(Get, k)
		
		    if var ~= nil then
			    return var(t)	
		    end
	    end
	
	    return var
    end,
    __call = function(t,v)
	    return LayerMask.New(v)
    end
}

--local Get = tolua.initget(LayerMask)

--setmetatable(LayerMask, LayerMask)
return LayerMask



