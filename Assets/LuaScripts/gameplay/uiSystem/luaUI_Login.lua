local lhDebug=LaoHan.Infrastruture.lhDebug
local luaUI_Login={
    tag="luaUI_Login",
	extends=luaUIBase,
    Ctor=function(self)

    end,
    Initialize=function(self,obj,onInitialOver)
	    self.super:Initialize(obj,onInitialOver)
	
	    if onInitialOver then
		    onInitialOver()
	    end
    end,
    Open=function (self,param,onOpenOver)
	    lhDebug.Log("luaUI_Login:Open")
	    self.super.gameObject:SetActive(true)
        LaoHan.Infrastruture.lhSceneManager.Load("1",
            function(p)
                lhDebug.LogError(p)
            end,
            function ()
                lhDebug.Log("Ok");
            end,
            UnityEngine.SceneManagement.LoadSceneMode.Single
       )
	    if onOpenOver then
		    onOpenOver(nil)
        end
	end,
    Close=function(self,param,onCloseOver)
	    self.super:Close(param,onCloseOver)
	    lhDebug.Log("luaUI_Login:Close")
	    self.super.gameObject:SetActive(false)
	    if onCloseOver then
		    onCloseOver(nil)
	    end
    end,
    Destroy=function(self,onDestroyOver)
	    self.super:Destroy(onDestroyOver)
	    lhDebug.Log("luaUI_Login:Destroy")
        if(onDestroyOver) then
	        onDestroyOver(nil)
        end
    end
}return luaUI_Login