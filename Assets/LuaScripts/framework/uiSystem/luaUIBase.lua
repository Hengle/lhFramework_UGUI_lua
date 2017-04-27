local lhDebug=LaoHan.Infrastruture.lhDebug

local EUIState={"Ctor","Initialize","Open","Close","Destroy"}
luaUIBase={
	rectTransform,
	gameObject,
	uiState,
	extends=luaBehaviour,
    Ctor=function(self)
    end,

    Initialize=function(self,obj,onInitialOver)
	    self.gameObject=obj
        lhDebug.Log(obj)
	    self.rectTransform=obj:GetComponent(typeof(UnityEngine.RectTransform))
	    self.uiState=EUIState[1]
    end,

    Open=function(self,param,onOpenOver)
	    self.uiState=EUIState[2]
    end,
    
    Close=function(self,param,onCloseOver)
	    self.uiState=EUIState[3]
    end,

    Destroy=function(self,onDestroyOver)
	    uiState=EUIState[4]
    end 

}
