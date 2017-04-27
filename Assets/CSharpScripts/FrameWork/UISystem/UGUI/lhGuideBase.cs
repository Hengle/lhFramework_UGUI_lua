using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LaoHan.Infrastruture;

namespace LaoHan.UGUI
{
    public class lhGuideBase : lhMonoBehaviour,IGuideInterface
    {
        private List<string> m_process;
        private int m_index;
        private List<string> m_processMaskList;
        private GameObject m_currentObj;
        public void EnterNext()
        {
            SetChildLayer(m_currentObj.transform, LayerMask.NameToLayer("UI"));
            GameObject.Find(m_process[m_index++]).SetActive(false);

            GameObject obj = GameObject.Find(m_processMaskList[m_index]);
            SetChildLayer(obj.transform, LayerMask.NameToLayer("Guide"));
            GameObject.Find(m_process[m_index]).SetActive(true);
            m_currentObj = obj;
        }
        public virtual void Initialize(List<string> process,List<string> maskList)
        {
            m_process = process;
            m_processMaskList = maskList;
        }
    }
}
