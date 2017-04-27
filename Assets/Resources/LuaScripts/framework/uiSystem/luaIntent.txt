
local extras={}
luaIntent={
    PutExtras=function(key,value)
        extras[key]=value
    end,
    HasExtras=function(key)
        return extras.key!=nil
    end,
    GetExtras=function(key)
        return extras[key]
    end

}
