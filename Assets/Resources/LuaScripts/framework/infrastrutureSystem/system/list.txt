--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
local list = 
{
-----------------------------------------------member
    length=0,
    first,
    last,
-----------------------------------------------construture
    Ctor=function(self,l)
        self.length=l or 0
    end,
-----------------------------------------------public methods
    Clear=function(self)
	    self.length = 0
	    self.first = nil
	    self.last = nil
    end,
    Push=function(self,v)
	    local t = {value = v}
	
	    if self.last then
		    self.last._next = t
		    t._prev = self.last
		    self.last = t		
	    else			
		    self.first = t
		    self.last = t		
	    end
	
	    self.length = self.length + 1
    end,
    Pop=function(self)
	    if not self.last then return end	
	    local t = self.last	
	
	    if t._prev then
		    t._prev._next = nil
		    self.last = t._prev
		    t._prev = nil
	    else
		    self.first = nil
		    self.last = nil
	    end
	
	    self.length = self.length - 1
	    return t.value
    end,
    Unshift=function(self,v)
	    local t = {value = v}

	    if self.first then
		    self.first._prev = t
		    t._next = self.first
		    self.first = t
	    else
		    self.first = t
		    self.last = t
	    end
	
	    self.length = self.length + 1
    end,
    Shift=function(self)
	    if not self.first then return end
	    local t = self.first

	    if t._next then
		    t._next._prev = nil
		    self.first = t._next
		    t._next = nil
	    else
		    self.first = nil
		    self.last = nil
	    end

	    self.length = self.length - 1
	    return t.value
    end,
    Remove=function(self,iter)
	    if iter._next then
		    if iter._prev then
			    iter._next._prev = iter._prev
			    iter._prev._next = iter._next
		    else
			    assert(iter == self.first)
			    iter._next._prev = nil
			    self.first = iter._next
		    end
	    elseif iter._prev then
		    assert(iter == self.last)
		    iter._prev._next = nil
		    self.last = iter._prev
	    else
		    assert(iter == self.first and iter == self.last)
		    self.first = nil
		    self.last = nil
	    end
		
	    self.length = self.length - 1
	    return iter
    end,
    Find=function(self,v, iter)
	    if iter == nil then
		    iter = self.first
	    end
	
	    while iter do
		    if v == iter.value then
			    return iter
		    end
		
		    iter = iter._next
	    end
	
	    return nil
    end,
    Findlast=function(self,v, iter)
	    if iter == nil then
		    iter = self.last
	    end
	
	    while iter do
		    if v == iter.value then
			    return iter
		    end
		
		    iter = iter._prev
	    end
	
	    return nil
    end,
    ToNext=function(self,iter)
	    if iter then		
		    if iter._next ~= nil then
			    return iter._next, iter._next.value
		    end
	    elseif self.first then
		    return self.first, self.first.value
	    end
	
	    return nil
    end,
    items=function(self)		
	    return self.ToNext, self
    end,
    Prev=function(self,iter)
	    if iter then		
		    if iter._prev ~= nil then
			    return iter._prev, iter._prev.value
		    end
	    elseif self.last then
		    return self.last, self.last.value
	    end
	
	    return nil
    end,
    Reverse_items=function(self)
	    return self.Prev, self
    end,
    Erase=function (self,value)
	    local iter = self:Find(value)

	    if iter then
		    self:Remove(iter)
	    end
    end,
    Insert=function(self,v, iter)
	    assert(v)
	    if not iter then
		    return self:Push(value)
	    end
	
	    local t = {value = v}
	
	    if iter._next then
		    iter._next._prev = t
		    t._next = iter._next
	    else
		    self.last = t
	    end
	
	    t._prev = iter
	    iter._next = t
	    self.length = self.length + 1
    end,
    Head=function(self)
      if self.first ~= nil then
        return self.first.value
      end
      return nil
    end,
    Tail=function(self)
      if self.last ~= nil then
        return self.last.value
      end
      return nil
    end,
    Clone=function(self)
	    local t = list:New()
	
	    for item in self.items() do
		    t:Push(item.value)
	    end
	
	    return t
    end,
-----------------------------------------------static methods
    New=function() 
      return Class:New(list)
    end
-----------------------------------------------element function
}

--ilist	= list.items
--rilist	= list.Reverse_items
return list