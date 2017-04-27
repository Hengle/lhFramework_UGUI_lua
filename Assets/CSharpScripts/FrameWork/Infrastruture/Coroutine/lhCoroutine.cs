using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace LaoHan.Infrastruture
{
    public enum ECoroutineType
    {
        None,
        UI,
        Battle
    }
    public class lhCoroutine
    {
        private static lhCoroutine m_instance;
        private Dictionary<ECoroutineType, List<System.Collections.IEnumerator>> m_enumerators = new Dictionary<ECoroutineType, List<IEnumerator>>();
        private Dictionary<ECoroutineType, List<System.Collections.IEnumerator>> m_enumeratorsDelete = new Dictionary<ECoroutineType, List<IEnumerator>>();
        public static lhCoroutine GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhCoroutine();
        }
        lhCoroutine() { }
        public void Dispose()
        {
            m_instance = null;
        }
        public void LateUpdate()
        {
            lock(m_enumerators)
            {
                foreach (var item in m_enumerators)
                {
                    var obj = item.Key;
                    var enumerators = item.Value;
                    for (int i = 0; i < enumerators.Count; ++i)
                    {
                        if (m_enumeratorsDelete.ContainsKey(obj))
                        {
                            if ( m_enumeratorsDelete[obj].Contains(enumerators[i]))
                            {
                                continue;
                            }
                        }

                        if (enumerators[i].Current is lhWaitForSeconds)
                        {
                            lhWaitForSeconds waitForSeconds = enumerators[i].Current as lhWaitForSeconds;
                            waitForSeconds.time -= Time.deltaTime;
                            if (waitForSeconds.time > 0)
                                continue;
                        }
                        else if (enumerators[i].Current is lhWaitForCount)
                        {
                            lhWaitForCount waitForCount = enumerators[i] as lhWaitForCount;
                            waitForCount.count--;
                            if (waitForCount.count>0)
                                continue;
                        }
                        else if (enumerators[i] is lhWaitForReturn)
                        {
                            if (m_enumeratorsDelete.ContainsKey(obj))
                            {
                                m_enumeratorsDelete[obj].Add(enumerators[i]);
                            }
                            else
                            {
                                m_enumeratorsDelete.Add(obj, new List<IEnumerator>() { enumerators[i] });
                            }
                        }
                        else if (enumerators[i].Current is WWW)
                        {
                            WWW www = enumerators[i].Current as WWW;
                            if (!www.isDone)
                                continue;
                        }
                        else if (enumerators[i].Current is AssetBundleRequest)
                        {
                            AssetBundleRequest request = enumerators[i].Current as AssetBundleRequest;
                            if (!request.isDone)
                                continue;
                        }
                        //--------------------------
                        //others
                        //--------------------------
                        if (!enumerators[i].MoveNext())
                        {
                            if (m_enumeratorsDelete.ContainsKey(obj))
                            {
                                m_enumeratorsDelete[obj].Add(enumerators[i]);
                            }
                            else
                            {
                                m_enumeratorsDelete.Add(obj, new List<IEnumerator>() { enumerators[i] });
                            }
                            continue;
                        }
                    }
                }
                foreach (var item in m_enumeratorsDelete)
                {
                    foreach (var del in item.Value)
                    {
                        if (m_enumerators.ContainsKey(item.Key))
                        {
                            if (m_enumerators[item.Key].Contains(del))
                            {
                                m_enumerators[item.Key].Remove(del);
                            }
                        }
                    }
                }
                m_enumeratorsDelete.Clear();
            }
        }
        public static void StartCoroutine(IEnumerator enumerator, ECoroutineType target = ECoroutineType.None)
        {
            if (m_instance == null) return;
            if (m_instance.m_enumerators.ContainsKey(target))
            {
                m_instance.m_enumerators[target].Add(enumerator);
            }
            else
            {
                var list = new List<IEnumerator>();
                list.Add(enumerator);
                m_instance.m_enumerators.Add(target, list);
            }
        }
        public static void StopCoroutine(IEnumerator enumerator, ECoroutineType target = ECoroutineType.None)
        {
            if (m_instance == null) return;
            if (enumerator == null) return;
            if (m_instance.m_enumerators.ContainsKey(target))
            {
                if (m_instance.m_enumeratorsDelete.ContainsKey(target))
                {
                    m_instance.m_enumeratorsDelete[target].Add(enumerator);
                }
                else
                {
                    m_instance.m_enumeratorsDelete.Add(target, new List<IEnumerator>() { enumerator });
                }
            }
        }
        public static void StopAllCoroutine()
        {
            if (m_instance == null) return;
            m_instance.m_enumeratorsDelete = m_instance.m_enumerators;
        }
        public static void StopAllCoroutine(ECoroutineType target =ECoroutineType.None)
        {
            if (m_instance == null) return;
            if (m_instance.m_enumeratorsDelete.ContainsKey(target))
            {
                if (m_instance.m_enumerators.ContainsKey(target))
                {
                    m_instance.m_enumeratorsDelete[target].AddRange(m_instance.m_enumerators[target]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (m_instance.m_enumerators.ContainsKey(target))
                {
                    m_instance.m_enumeratorsDelete.Add(target, m_instance.m_enumerators[target]);
                }
                else
                {
                    return;
                }
            }
            
        }
    }
}