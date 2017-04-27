Plane =
{
	normal = nil,
	distance = 0,
    Ctor=function(self,normal, d)
        self.normal=normal.normalize()
        self.distance=d or 0
    end,
    New=function(normal, d)
	    return class:New(Plane,normal,d)
    end,
    Raycast=function(self,ray)
	    local a = Vector3.dot(ray.direction, self.normal)
        local num2 = -Vector3.dot(ray.origin, self.normal) - self.distance
	
        if Mathf.Approximately(a, 0) then                   
		    return false, 0        
	    end
	
        local enter = num2 / a    
	    return enter > 0, enter
    end,
    SetNormalAndPosition=function(self,inNormal, inPoint)    
        self.normal = inNormal:normalize()
        self.distance = -Vector3.dot(inNormal, inPoint)
    end,
    Set3Points=function(self,a, b, c)    
        self.normal = Vector3.normalize(Vector3.cross(b - a, c - a))
        self.distance = -Vector3.dot(self.normal, a)
    end	,
    GetDistanceToPoint=function(self,inPt)    
	    return Vector3.dot(self.normal, inPt) + self.distance
    end,
    GetSide=function(self,inPt)    
	    return ((Vector3.dot(self.normal, inPt) + self.distance) > 0)
    end,
    SameSide=function(self,inPt0, inPt1)    
	    local distanceToPoint = self:GetDistanceToPoint(inPt0)
	    local num2 = self:GetDistanceToPoint(inPt1)
	    return (((distanceToPoint > 0) and (num2 > 0)) or ((distanceToPoint <= 0) and (num2 <= 0)))
    end    
}


--local mt = {}
--mt.__index = Plane

