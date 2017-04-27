local lhDebug=LaoHan.Infrastruture.lhDebug
local lhResources=LaoHan.Infrastruture.lhResources
local uiPath="gameplay/uiSystem/"
local this={}
local super={}

local m_defaultParent

local m_uiLibrary={}
local m_utilityLibrary={}
local m_uiPath={}
local m_utilityPath={}

local m_guidePath={}

local m_currentUI
local m_uiMessageHandler={}

luaUIManager={
    tag="luaUIManager",
    extends=luaBehaviour,
    ctor=function(self)
    end,
    Initialize=function(self)
	    local uiType=luaConfigData.uiType
	    function onInitial()
		    for i=1,table.getn(uiType.uiLibrary) do
			    m_uiPath[uiType.uiLibrary[i].uiClass]=uiType.uiLibrary[i]
		    end
		
		    for i=1,table.getn(uiType.utilityLibrary) do
			    m_utilityPath[uiType.utilityLibrary[i].uiClass]=uiType.utilityLibrary[i]
		    end
		
		    if uiType.guide ~=nil then
			    for i=1,table.getn(uiType.guide) do
				    m_guidePath[uiType[i].guideId]=uiType.guide[i]
			    end
		    end
		    if uiType.defaultData~=nil then
		        if uiType.defaultData.uiClass and uiType.defaultData.uiClass ~="" then
			        self:EnterUI(uiType.defaultData.uiClass,
				        function (uiBase)
					        uiBase:Open(nil,
						        function (p)
						        end
					        )
				        end
			        ,true)
		        end
		    end
	    end
	
	    if uiType.defaultData.uiRoot and  uiType.defaultData.uiRoot ~="" then
            m_defaultParent=UnityEngine.GameObject.Find(uiType.defaultData.uiRoot).transform
            onInitial()
	    else
		    m_defaultParent=LaoHan.UGUI.lhUIManager.GetCanvasTransform()
		    onInitial()
	    end
        UnityEngine.Object.DontDestroyOnLoad(m_defaultParent.gameObject)
    end,
    EnterUI=function(self,uiClass,onEnterOver,closeCurrentUI)
	    if m_uiLibrary[uiClass] then
		    local uiBase=m_uiLibrary[uiClass]
		    uiBase.gameObject:SetActive(false)
		    if closeCurrentUI==true and m_currentUI ~=nil then
			    m_currentUI:Close(onCloseOver)
			    m_currentUI=uiBase
			    if onEnterOver then
				    onEnterOver(uiBase)
			    end
		    end
	    else
		    if  m_uiPath[uiClass]==nil then
			    lhDebug.LogError("LaoHan:Lua uiPath dont has this Class: < ".. "".. uiClass.. " >")
			    return
		    end
		    lhResources.Load(m_uiPath[uiClass].uiPath,
			    function (o)
				    local obj=self.super.Instantiate(o,Vector3.New(0,0,0),Quaternion.New(0,0,0,0),false)
                    lhDebug.Log(m_uiPath[uiClass].uiPath)
                    local ui=require(uiPath..uiClass)
                    local uiBase=Class:New(ui)
				    uiBase:Initialize(obj,
					    function ()
						    self.super.SetRectTransform(uiBase.super.rectTransform,m_defaultParent)
						    uiBase.super.gameObject:SetActive(false)
						    if closeCurrentUI==true then
							    if m_currentUI ~=nil then
								    m_currentUI:Close(onCloseOver)
							    end
							    m_currentUI=uiBase
						    end
						    if onEnterOver ~=nil then
							    onEnterOver(uiBase)
						    end
                            if m_uiMessageHandler[uiBase]==nil then
						        m_uiMessageHandler[uiBase]=uiBase.receiveMessage
                            else
						        m_uiMessageHandler[uiBase]=m_uiMessageHandler[uiBase]+uiBase.receiveMessage
                            end
					    end
				    )
			    end
		    )
	    end
    end,
    closeUI=function(self,uiClass,onCloseOver)
	    if m_defaultParent ==nil then
		    lhDebug.LogError("LaoHan:defaultParent is nil")
		    return
	    end
	    if m_uiLibrary.uiClass ==nil then
		    lhDebug.LogWarning("LaoHan:m_uiLibrary dont has this Class:"..""..uiClass)
	    else
		    if m_uiPath.uiClass ==nil then
			    lhDebug.LogError("LaoHan:uiPath dont has this Class: "..""..uiClass)
		    else
			    local uiBase=m_uiLibrary[uiClass]
			    uiBase.gameObject.SetActive(true)
			    if uiBase.uiState ~="Close" then
				    uiBase.Close(onCloseOver)
			    else
				    lhDebug.LogWarning("LaoHan:Class <"..uiClass.."".."> UIState <"..uiBase.uiState.." >")
			    end
		    end
	    end
    end,
    destroyUI=function(self,uiClass,onDestroyOver)
	    if m_defaultParent ==nil then
		    lhDebug.LogError("LaoHan: defaultParent is nil")
		    return
	    end
	    if m_uiLibrary.uiClass ==nil then
		    lhDebug.LogWarning("LaoHan:m_uiLibrary dont has this class: <"..uiClass..">")
	    else
		    if m_uiPath.uiClass ==nil then
			    lhDebug.LogError("LaoHan;uiPath dont has this Class:<"..uiClass..">")
		    else
			    local uiBase=m_uiLibrary[uiClass]
                
			    uiBase.gameObject:SetActive(false)
			    if uiBase.uiState ~="destroy" then
				    m_uiMessageHandler.uiBase =nil
				    m_uiLibrary.uiClass=nil
				    uiBase:destroy(onDestroyOver)
				    UnityEngine.Object.Destroy(uiBase.gameObject)
			    else
				    lhDebug.LogWarning("LaoHan:Class <"..uiClass.."> UIState <"..uiClass.uiState..">")
			    end
		    end
	    end
    end,
    enterUtility=function(self,uiClass,onEnterOver)
            local GO=function(obj)
                local go = self.super.Instantiate(obj);
                
                local utilityBase = go.GetComponent(typeof(lhUtilityBase))
                self.super.SetRectTransform(utilityBase.rectTransform, m_defaultParent)
                if utilityBase.uiState == "none" then
                    utilityBase.Initialize(
                        function()
                            if onEnterOver ~= nil then
                                onEnterOver(utilityBase)
                            end
                        end
                    )
                else
                    if onEnterOver ~= nil then
                        onEnterOver(utilityBase)
                    end
                end
                if m_uiMessageHandler[uiBase]==nil then
					m_uiMessageHandler[uiBase]=utilityBase.receiveMessage
                else
					m_uiMessageHandler[uiBase]=m_uiMessageHandler[uiBase]+utilityBase.receiveMessage
                end
            end
            if m_utilityLibrary.uiClass ~=nil then
                lhResources.Load(m_instance.m_utilityPath[uiClass].uiPath,
                    function (o)
                        local obj = o
                        m_utilityLibrary.Add(uiClass, obj)
                        GO(obj)
                    end
                )
            else
                local obj = m_utilityLibrary[uiClass]
                GO(obj)
            end
    end,
    closeUtility=function(self,uiClass,utilityBase, onCloseOver)
        if m_utilityLibrary.uiClass ~=nil then
            lhDebug.LogError("LaoHan: uiClass < " + uiClass + " > dont in utilityLibrary")
            return;
        end
        utilityBase.Close(onCloseOver)
        self.super.freeInstantiate(m_utilityLibrary[uiClass], utilityBase.gameObject)
    end,
    getUIBase=function(self,uiClass)
        local uiBase = m_uiLibrary[uiClass]
        if uiBase ~= nil then
            return uiBase
        else
            return nil
        end
    end
}