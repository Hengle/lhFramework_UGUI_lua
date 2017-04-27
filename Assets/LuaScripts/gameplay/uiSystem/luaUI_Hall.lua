
--Button　Event------------------------------------------
local function OnBtnSettingClick()
end

local function OnBtnMailClick()
end

local function OnBtnActivityClick()
end

local function OnBtnFirstPunchClick()
end

local luaUI_Hall={
    tag="luaUI_Hall",
	extends=luaUIBase,
    Ctor=function(self,s)

    end,

    Initialize=function(self,obj,onInitialOver)
	    self.super:Initialize(obj,onInitialOver)
	    if onInitialOver then
		    onInitialOver()
	    end
    end,

    Open=function(self,param,onOpenOver)
	    self.super:Open(param,onOpenOver)
	    lhDebug.Log("luaUI_Hall:Open")
	    self.super.gameObject:SetActive(true)
	    if onOpenOver then
		    onOpenOver(nil)
	    end
    end,

    Close=function(self,param,onCloseOver)
	    self.super:Close(param,onCloseOver)
	    lhDebug.Log("luaUI_Hall:Close")
	    self.super.gameObject:SetActive(false)
	    if onCloseOver then
		    onCloseOver(nil)
	    end
    end,

    Destroy=function(self,onDestroyOver)
	    self.super.Destroy(onDestroyOver)
	    lhDebug.Log("luaUI_Hall:Destroy")
	    if onDestroyOver then
		    onDestroyOver(nil)
	    end
    end

}return luaUI_Hall