using UnityEngine;
using System.Collections;

using LaoHan.Infrastruture;
//using comm.msg;
using System.Collections.Generic;
using LaoHan.Data;
using System;
namespace LaoHan.Battle
{
    public class lhBuffSync : lhMonoBehaviour, ISync
    {
        //public enum BuffStatus
        //{
        //    Run,
        //    Store
        //}
        //private class BuffData
        //{
        //    public uint stateid;
        //    public GameObject effect;
        //    public uint materialId;
        //    public object preParam;
        //    public string paramType;
        //    public uint leftTime;
        //    public BuffStatus status;
        //}

        //private ulong m_charid;
        //private Dictionary<uint,BuffData> m_buffDic;
        //private List<uint> m_removeBuff = new List<uint>();
        //private BuffStatus m_buffStatus;
        //public void Initialize(ulong charid)
        //{
        //    this.m_charid = charid;
        //    m_buffDic = new Dictionary<uint, BuffData>();
        //    lhSyncManager.buffInfoHandler[charid] = OnBuffInfo;
        //}
        //public void SetBuffStatus(BuffStatus status)
        //{
        //    if (status==BuffStatus.Run)
        //    {
        //        foreach (var item in m_buffDic)
        //        {
        //            if (item.Value.status==BuffStatus.Store)
        //            {
        //                var effect = lhConfigData.effect.dic[(int)item.Key];
        //                int effect_animation = effect.p3_effect_animation_id;
        //                int materialId = effect.material_id;
        //                ExecuteBuff(effect_animation, item.Value, effect, materialId);
        //            }
        //        }
        //    }
        //    m_buffStatus = status;
        //}
        //public void Clear()
        //{
        //    m_buffDic.Clear();
        //}
        //void OnBuffInfo(StateInfos info)
        //{
        //    //return; //[CY ADD:] 临时禁止buff，因为新buff没粒子会导致后面代码出错

        //    for (int i = 0; i < m_removeBuff.Count; i++)
        //    {
        //        if (m_buffDic.ContainsKey(m_removeBuff[i]))
        //             m_buffDic.Remove(m_removeBuff[i]);
        //    }
        //    m_removeBuff.Clear();
        //    for (int i = 0; i < info.states.Count; i++)
        //    {
        //        var data = info.states[i];
        //        if (m_buffDic.ContainsKey(data.stateid))
        //        {
        //            var buffData=m_buffDic[data.stateid];
        //            if (data.lefttime<=0)
        //            {
        //                var skinned = GetComponentInChildren<SkinnedMeshRenderer>();
        //                UnityEngine.Material material = null;    
        //                if (skinned != null)
        //                {
        //                    material = skinned.material;
        //                }
        //                RemoveBuff(material,lhConfigData.effect.dic[(int)data.stateid], data, m_buffDic[data.stateid]);
        //            }
        //        }
        //        else
        //        {
        //            if (data.lefttime > 0)
        //            {
        //                var effect = lhConfigData.effect.dic[(int)data.stateid];
        //                int effect_animation =effect.p3_effect_animation_id;
        //                int materialId = effect.material_id;
        //                var buffData = new BuffData();
        //                buffData.stateid = data.stateid;
        //                buffData.leftTime = data.lefttime;
        //                buffData.status = m_buffStatus;
        //                if (m_buffStatus==BuffStatus.Run)
        //                {
        //                    ExecuteBuff(effect_animation,buffData, effect, materialId);
        //                }
        //                m_buffDic.Add(data.stateid, buffData);
        //            }
        //        }
        //    }
        //}
        //void ExecuteBuff(int effect_animation,BuffData buffData,LaoHan.Data.Effect effect,int materialId)
        //{
        //    if (effect_animation > 0)
        //    {
        //        buffData.effect = lhObjectManager.GetObject(effect_animation, PoolType.Effect);
        //        buffData.effect.SetActive(true);
        //        buffData.effect.transform.parent = transform;
        //    }
        //    if (effect.material_id > 0)
        //    {
        //        var skinned = GetComponentInChildren<SkinnedMeshRenderer>();
        //        if (skinned != null)
        //        {
        //            //buffData.preMaterial = skinned.material;
        //            lhAssetManager.GetAsset(materialId, (o) =>
        //            {
        //                skinned.material = o as UnityEngine.Material;
        //                MaterialVariety(skinned.material, effect, buffData);
        //            });
        //        }
        //    }
        //    else
        //    {
        //        var skinned = GetComponentInChildren<SkinnedMeshRenderer>();
        //        if (skinned != null)
        //        {
        //            //buffData.preMaterial = skinned.material;
        //            if (!string.IsNullOrEmpty(effect.material_variety) && !skinned.material != null)
        //            {
        //                MaterialVariety(skinned.material, effect, buffData);
        //            }
        //        }
        //    }
        //}
        //void MaterialVariety(UnityEngine.Material preMaterial, Effect e, BuffData b)
        //{
        //    if (e.material_value_type!=null && e.material_value_type.ToLower().Equals("float"))
        //    {
        //        b.paramType = e.material_value_type;
        //        if (preMaterial != null)
        //        {
        //            if (preMaterial.HasProperty(e.material_variety))
        //            {
        //                b.preParam = preMaterial.GetFloat(e.material_variety);
        //                preMaterial.SetFloat(e.material_variety,Convert.ToSingle( e.material_value));
        //            }
        //        }
        //    }
        //    else if (e.material_value_type != null && e.material_value_type.ToLower().Equals("int"))
        //    {
        //        b.paramType = e.material_value_type;
        //        if (preMaterial != null)
        //        {
        //            if (preMaterial.HasProperty(e.material_variety))
        //            {
        //                b.preParam = preMaterial.GetInt(e.material_variety);
        //                preMaterial.SetInt(e.material_variety, Convert.ToInt32(e.material_value));
        //            }
        //        }
        //    }
        //}
        //void RemoveBuff(UnityEngine.Material preMaterial, Effect e, StateInfo s, BuffData b)
        //{
        //    if (b.effect != null)
        //    {
        //        lhObjectManager.FreeObject(e.p3_effect_animation_id, b.effect, true, PoolType.Effect, -1, () =>
        //        {
        //            if (m_buffDic.ContainsKey(s.stateid))
        //                m_buffDic.Remove(s.stateid);
        //        });
        //    }
        //    if (b.preParam != null)
        //    {
        //        if (b.paramType.ToLower().Equals("float"))
        //        {
        //            if (preMaterial!=null)
        //                preMaterial.SetFloat(e.material_variety, Convert.ToSingle(b.preParam));
        //        }
        //        else if (b.paramType.ToLower().Equals("int"))
        //        {
        //            if (preMaterial != null)
        //                preMaterial.SetInt(e.material_variety, Convert.ToInt32(b.preParam));
        //        }
        //        if (m_buffDic.ContainsKey(s.stateid))
        //            m_buffDic.Remove(s.stateid);
        //    }
        //    if (preMaterial != null)
        //    {
        //        lhAssetManager.FreeAsset(e.material_id, preMaterial, -1, () =>
        //        {
        //            if (m_buffDic.ContainsKey(s.stateid))
        //                m_buffDic.Remove(s.stateid);
        //        });
        //    }
        //}
        public void Initialize(ulong charid)
        {
            throw new NotImplementedException();
        }
    }
}
