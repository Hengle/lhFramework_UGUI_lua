
----------------------------------------------------------------framework
require "framework/infrastrutureSystem/luaBehaviour"
require "framework/uiSystem/luaUIManager"
require "framework/uiSystem/luaUIBase"
require "framework/dataSystem/luaConfigData"
require "framework/dataSystem/luaMemoryData"
require "framework/dataSystem/luaCacheData"
require "framework/controlSystem/luaControlNetwork"
----------------------------------------------------------------gameplay

----------------------------------------------------------------local parameter
local lhDebug=LaoHan.Infrastruture.lhDebug
local m_uiManager
local m_configData
local m_memoryData
local m_cacheData
local m_controlNetwork

luaInfrastrutureManager={
    tag="luaInfrastruture",
    Ctor=function(self)
        m_configData=Class:Singlton(luaConfigData,
            function()
                m_memoryData=Class:Singlton(luaMemoryData)
                m_cacheData=Class:Singlton(luaCacheData)
                m_controlNetwork=Class:Singlton(luaControlNetwork)
                m_uiManager=Class:Singlton(luaUIManager)
                m_uiManager:Initialize()
            end
        )
    end
}