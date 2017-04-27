local lhDebug=LaoHan.Infrastruture.lhDebug
local lhResources=LaoHan.Infrastruture.lhResources
local  cjson=require "cjson"
local configArr={
    uiType="ConfigData,LuaUIConfig"
}

luaConfigData={
    tag="luaConfigData",
    Ctor=function(self,onLoadOver)
        local count=table.getn(configArr)
        for key,value in pairs(configArr) do
            local path=value;
            lhResources.Load(path,
                function(o)
                    count=count-1
                    luaConfigData[key]=cjson.decode(o.text)
                    if count <=0 then
                        onLoadOver()
                        self=luaConfigData
                        return self
                    end
                end
            )
        end
    end
}