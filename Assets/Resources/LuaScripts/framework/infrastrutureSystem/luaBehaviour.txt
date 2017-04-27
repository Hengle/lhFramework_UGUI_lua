local lhDebug=LaoHan.Infrastruture.lhDebug
local lhObjectManager=LaoHan.Infrastruture.lhObjectManager
luaBehaviour={
    Ctor=function(self,s)
    end,
    Instantiate=function(obj,position,rotation,objectPool)
	    if not objectPool then
		    objectPool=false
	    end
        if position ==nil then
            position=Vector3.New(0,0,0)
        end
        if rotation ==nil then
            rotation=Quaternion.New(0,0,0,0)
        end
	    if objectPool~=nil and objectPool==true then
		    return lhObjectManager.GetObject(obj,position,rotation)
	    else
		    return UnityEngine.Object.Instantiate(obj,position,rotation)
	    end
    end,

    FreeInstantiate=function(obj,waitFree,freePool)
	    lhObjectManager.FreeObject(obj,waitFree,freePool)
    end,

    SetRectTransform=function(target,parent,localScale,anchoredPosition,sizeDelta)
	    target.parent=parent.transform
	    if localScale ==nil then
		    localScale=Vector3.New(1,1,1)
	    end
	    if anchoredPosition ==nil then
		    anchoredPosition=Vector2.New(0,0)
	    end
	    if sizeDelta ==nil then
		    sizeDelta=Vector2.New(1,1)
	    end
	    target.transform.localScale=localScale
	    target.anchoredPosition=anchoredPosition
	    target.sizeDelta=sizeDelta
    end
}