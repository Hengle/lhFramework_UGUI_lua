Class={
    New=function(self,classTable,...)
         local cls = {}
         if classTable then
            for key,value in pairs(classTable) do
                cls[key]=value
            end
            if classTable.extends then
                cls.super=self:New(classTable.extends,...)
            end
         end
        cls.__index = cls
        
        setmetatable(cls,classTable)
        if cls.Ctor then
            cls:Ctor(...)
        end
        return cls
    end,
    Singlton=function(self,classTable,...)
        if classTable.extends then
	        classTable.super=self:New(classTable.extends)
        end
        classTable.__index=classTable
        local instance = setmetatable({}, classTable)
        if instance.Ctor then
            instance:Ctor(...)
        end
	    return instance
    end
}