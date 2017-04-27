using UnityEditor;
using System.Collections;

namespace LaoHan.Tools.BundleBuilder
{
    public class lhInpectorBase<T> : Editor where T : UnityEngine.Object
    {

        protected T p_target { get { return (T)target; } }

    }
}
