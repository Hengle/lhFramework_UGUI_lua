local lhDebug=LaoHan.Infrastruture.lhDebug
--local member


--local function



--local button event



--local server event


--local private methods


---------------------------
--public function and member
local #NAME#={
	tag="#NAME#",
	extends=luaUIBase,
	Ctor=function(self)
	end,
	Initialize=function(self,obj,onInitialOver)
		self.super:Initialize(obj,onInitialOver)
		if onInitialOver then
			onInitialOver()
		end
	end,
	Open=function(self,param,onOpenOver)
		self.super:Open(param,onOpenOver)
		lhDebug.Log("#NAME#:open")
		self.super.gameObject:SetActive(true)
		if onOpenOver then
			onOpenOver(nil)
		end
	end,
	Close=function(self,param,onCloseOver)
		self.super:Close(param,onCloseOver)
		lhDebug.Log("#NAME#:close")
		self.super.gameObject:SetActive(false)
		if onCloseOver then
			onCloseOver(nil)
		end
	end,
	Destroy=function(self,onDestroyOver)
		self.super:Destroy(onDestroyOver)
		lhDebug.Log("#NAME#:destroy")
		if onDestroyOver then
			onDestroyOver(nil)
		end
	end,
	
	Receive=function(self,mark,value)
	
	end,
	
	Transmit=function(mark,value)
	
	end
}return #NAME#
