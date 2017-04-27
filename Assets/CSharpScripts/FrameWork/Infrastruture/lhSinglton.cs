using System;
using System.Collections.Generic;

namespace LaoHan.Infrastruture
{
    public interface ISinglton
    {
        void Ctor();
        void Dispose();
    }
    public class lhSinglton<T>:ISinglton where T: class , ISinglton,new()
    {
        protected static T i_instance;
        public static T GetInstance()
        {
            if (i_instance != null) return null;
            i_instance = new T();
            i_instance.Ctor();
            return i_instance;
        }
        public virtual void Ctor()
        {

        }
        public virtual void Dispose()
        {
            i_instance = null;
        }
    }
}
