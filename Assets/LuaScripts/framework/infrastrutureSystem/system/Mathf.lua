local math = math
Mathf={
    Deg2Rad = math.rad(1),
    Epsilon = 1.4013e-45,
    Infinity = math.huge,
    NegativeInfinity = -math.huge,
    PI = math.pi,
    Rad2Deg = math.deg(1),
		
    Abs = math.abs,
    Acos = math.acos,
    Asin = math.asin,
    Atan = math.atan,
    Atan2 = math.atan2,
    Ceil = math.ceil,
    Cos = math.cos,
    Exp = math.exp,
    Floor = math.floor,
    Log = math.log,
    Log10 = math.log10,
    Max = math.max,
    Min = math.min,
    Pow = math.pow,
    Sin = math.sin,
    Sqrt = math.sqrt,
    Tan = math.tan,
    Deg = math.deg,
    Rad = math.rad,
    Random = math.random,
    Approximately=function(self,a, b)
	    return self.Abs(b - a) < math.Max(1e-6 * math.Max(self.Abs(a), self.Abs(b)), 1.121039e-44)
    end,
    Clamp=function(value, Min, Max)
	    if value < Min then
		    value = Min
	    elseif value > Max then
		    value = Max    
	    end
	
	    return value
    end,
    Clamp01=function(value)
	    if value < 0 then
		    return 0
	    elseif value > 1 then
		    return 1   
	    end
	
	    return value
    end,
    DeltaAngle=function(self,current, target)    
	    local num = self.Repeating(target - current, 360)

	    if num > 180 then
		    num = num - 360
	    end

	    return num
    end ,
    Gamma=function(self,value, absmax, gamma) 
	    local flag = false
	
        if value < 0 then    
            flag = true
        end
	
        local num = self.Abs(value)
	
        if num > absmax then    
            return (not flag) and num or -num
        end
	
        local num2 = math.Pow(num / absmax, gamma) * absmax
        return (not flag) and num2 or -num2
    end,
    InverseLerp=function(from, to, value)
	    if from < to then      
		    if value < from then 
			    return 0
		    end

		    if value > to then      
			    return 1
		    end

		    value = value - from
		    value = value/(to - from)
		    return value
	    end

	    if from <= to then
		    return 0
	    end

	    if value < to then
		    return 1
	    end

	    if value > from then
            return 0
	    end

	    return 1 - ((value - to) / (from - to))
    end,
    Lerp=function(self,from, to, t)
	    return from + (to - from) * self.Clamp01(t)
    end,
    LerpAngle=function(self,a, b, t)
	    local num = self.Repeating(b - a, 360)

	    if num > 180 then
		    num = num - 360
	    end

	    return a + num * self.Clamp01(t)
    end,
    MoveTowards=function(self,current, target, maxDelta)
	    if self.Abs(target - current) <= maxDelta then
		    return target
	    end

	    return current + mathf.Sign(target - current) * maxDelta
    end,
    MoveTowardsAngle=function(self,current, target, maxDelta)
	    target = current + self.DeltaAngle(current, target)
	    return self.MoveTowards(current, target, maxDelta)
    end,
    PingPong=function(self,t, length)
        t = self.Repeating(t, length * 2)
        return length - self.Abs(t - length)
    end,
    Repeating=function(self,t, length)    
	    return t - (self.Floor(t / length) * length)
    end  ,
    Round=function(self,num)
	    return self.Floor(num + 0.5)
    end,
    Sign=function(num)  
	    if num > 0 then
		    num = 1
	    elseif num < 0 then
		    num = -1
	    else 
		    num = 0
	    end

	    return num
    end,
    SmoothDamp=function(self,current, target, currentVelocity, smoothTime, maxSpeed, deltaTime)
	    maxSpeed = maxSpeed or self.Infinity
	    deltaTime = deltaTime or Time.deltaTime
        smoothTime = self.Max(0.0001, smoothTime)
        local num = 2 / smoothTime
        local num2 = num * deltaTime
        local num3 = 1 / (1 + num2 + 0.48 * num2 * num2 + 0.235 * num2 * num2 * num2)
        local num4 = current - target
        local num5 = target
        local Max = maxSpeed * smoothTime
        num4 = self.Clamp(num4, -Max, Max)
        target = current - num4
        local num7 = (currentVelocity + (num * num4)) * deltaTime
        currentVelocity = (currentVelocity - num * num7) * num3
        local num8 = target + (num4 + num7) * num3
	
        if (num5 > current) == (num8 > num5)  then    
            num8 = num5
            currentVelocity = (num8 - num5) / deltaTime		
        end
	
        return num8
    end,
    SmoothDampAngle=function(self,current, target, currentVelocity, smoothTime, maxSpeed, deltaTime)
	    deltaTime = deltaTime or Time.deltaTime
	    maxSpeed = maxSpeed or self.Infinity	
	    target = current + self.DeltaAngle(current, target)
        return self.SmoothDamp(current, target, currentVelocity, smoothTime, maxSpeed, deltaTime)
    end,
    SmoothStep=function(self,from, to, t)
        t = self.Clamp01(t)
        t = -2 * t * t * t + 3 * t * t
        return to * t + from * (1 - t)
    end,
    HorizontalAngle=function(dir) 
	    return math.Deg(math.Atan2(dir.x, dir.z))
    end,
    IsNan=function(number)
	    return not (number == number)
    end
}
