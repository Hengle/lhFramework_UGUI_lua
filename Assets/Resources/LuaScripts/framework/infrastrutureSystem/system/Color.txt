local rawget = rawget
local setmetatable = setmetatable
local type = type

local Color = 
{		
-----------------------------------------------member
    r=1,
    g=1,
    b=1,
    a=1,
-----------------------------------------------construture
    Ctor=function(self,r,g,b,a)
        self.r=r or 1
        self.g=g or 1
        self.b=b or 1
        self.a=a or 1
    end,
-----------------------------------------------public methods
    Set=function(self,r, g, b, a)
	    self.r = r
	    self.g = g
	    self.b = b
	    self.a = a or 1 
    end,
    Get=function(self)
	    return self.r, self.g, self.b, self.a
    end,
    Equals=function(self,other)
	    return self.r == other.r and self.g == other.g and self.b == other.b and self.a == other.a
    end,
-----------------------------------------------static methods
    New=function(r, g, b, a)
        return class:New(Color,r,g,b,a)
    end,
    Red=function()
        return Color.New(1,0,0,1)
    end,
    Green=function()
        return Color.New(0,1,0,1)
    end,
    Blue=function()
        return Color.New(1,0,1,1)
    end,
    White=function()
        return Color.New(1,1,1,1)
    end,
    Black=function()
        return Color.New(0,0,0,1)
    end,
    Yellow=function()
        return Color.New(1, 0.9215686, 0.01568628, 1)
    end,
    Cyan=function()
        return Color.New(0,1,1,1)
    end,
    Magenta=function()
        return Color.New(1,0,1,1)
    end,
    Gray=function()
        return Color.New(0.5,0.5,0.5,1)
    end,
    Clear=function()
        return Color.New(0,0,0,0)
    end,
    Gamma=function(c)
        return Color.New(Mathf.LinearToGammaSpace(c.r), Mathf.LinearToGammaSpace(c.g), Mathf.LinearToGammaSpace(c.b), c.a)  
    end,
    Linear=function(c)
        return Color.New(Mathf.GammaToLinearSpace(c.r), Mathf.GammaToLinearSpace(c.g), Mathf.GammaToLinearSpace(c.b), c.a)
    end,
    MaxColorComponent=function(c)
        return Mathf.Max(Mathf.Max(c.r, c.g), c.b)
    end,
    Lerp=function(a, b, t)
	    t = Mathf.Clamp01(t)
	    return Color.New(a.r + t * (b.r - a.r), a.g + t * (b.g - a.g), a.b + t * (b.b - a.b), a.a + t * (b.a - a.a))
    end,
    LerpUnclamped=function(a, b, t)
      return Color.New(a.r + t * (b.r - a.r), a.g + t * (b.g - a.g), a.b + t * (b.b - a.b), a.a + t * (b.a - a.a))
    end,
    GrayScale=function(a)
	    return 0.299 * a.r + 0.587 * a.g + 0.114 * a.b
    end,
-----------------------------------------------element function
    __index = function(t,k)
	    local var = rawget(Color, k)
		
	    if var == nil then							
		    var = rawget(Get, k)
		
		    if var ~= nil then
			    return var(t)	
		    end
	    end
	
	    return var
    end,
    __call = function(t, r, g, b, a)
	    return Color.New(r, g, b, a)
    end,
    __tostring = function(self)
	    return string.format("RGBA(%f,%f,%f,%f)", self.r, self.g, self.b, self.a)
    end,
    __add = function(a, b)
	    return Color.New(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a)
    end,
    __sub = function(a, b)	
	    return Color.New(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a)
    end,
    __mul = function(a, b)
	    if type(b) == "number" then
		    return Color.New(a.r * b, a.g * b, a.b * b, a.a * b)
	    elseif getmetatable(b) == Color then
		    return Color.New(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a)
	    end
    end,
    __div = function(a, d)
	    return Color.New(a.r / d, a.g / d, a.b / d, a.a / d)
    end,
    __eq = function(a,b)
	    return a.r == b.r and a.g == b.g and a.b == b.b and a.a == b.a
    end
}

--local Get = tolua.initget(Color)


--Get.Red 	= function() return Color.New(1,0,0,1) end
--Get.Green	= function() return Color.New(0,1,0,1) end
--Get.Blue	= function() return Color.New(0,0,1,1) end
--Get.White	= function() return Color.New(1,1,1,1) end
--Get.Black	= function() return Color.New(0,0,0,1) end
--Get.Yellow	= function() return Color.New(1, 0.9215686, 0.01568628, 1) end
--Get.Cyan	= function() return Color.New(0,1,1,1) end
--Get.Magenta	= function() return Color.New(1,0,1,1) end
--Get.Gray	= function() return Color.New(0.5,0.5,0.5,1) end
--Get.Clear	= function() return Color.New(0,0,0,0) end

--Get.Gamma = function(c) 
--  return Color.New(Mathf.LinearToGammaSpace(c.r), Mathf.LinearToGammaSpace(c.g), Mathf.LinearToGammaSpace(c.b), c.a)  
--end

--Get.Linear = function(c)
--  return Color.New(Mathf.GammaToLinearSpace(c.r), Mathf.GammaToLinearSpace(c.g), Mathf.GammaToLinearSpace(c.b), c.a)
--end

--Get.MaxColorComponent = function(c)    
--  return Mathf.Max(Mathf.Max(c.r, c.g), c.b)
--end

--Get.grayscale = Color.GrayScale

--setmetatable(Color, Color)
return Color



