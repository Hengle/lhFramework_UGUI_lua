using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace LaoHan.Infrastruture
{
    public class lhLoop
    {
        private static lhLoop m_instance;
        public static lhLoop GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhLoop();
        }
        lhLoop()
        {

        }
        public void Dispose()
        {
            m_instance = null;
        }
        /// <summary>
        /// 延迟一定时间执行一个回调
        /// </summary>
        /// <param name="time"></param>
        /// <param name="onTimeOver"></param>
        public static void Wait(float time,Action onTimeOver)
        {
            lhCoroutine.StartCoroutine(m_instance.EWaitForSeconds(time, onTimeOver));
        }
        /// <summary>
        /// 等待执行，包含update，
        /// </summary>
        /// <param name="delay">等待执行的时间</param>
        /// <param name="overTime">等待后执行多长时间</param>
        /// <param name="onStart">方法开始的回调，等待前执行的回调</param>
        /// <param name="onUpdate">overTime过程中的update刷新回调,回调参数是执行的帧数</param>
        /// <param name="onTimeOver">所有的全部执行完毕后的回调</param>
        public static void Wait(float delay, float overTime, Action onStart, Action<int> onUpdate, Action onTimeOver)
        {
            lhCoroutine.StartCoroutine(m_instance.EWaitForUpdate(delay, overTime, onStart, onUpdate, onTimeOver));
        }
        IEnumerator EWaitForSeconds(float time,Action onTimeOver)
        {
            yield return new lhWaitForSeconds(time);
            onTimeOver();
        }
        IEnumerator EWaitForUpdate(float delay, float overTime, Action onStart, Action<int> onUpdate, Action onTimeOver)
        {
            float _time = Time.time;
            onStart();
            yield return new lhWaitForSeconds(delay);
            int i = 0;
            while (true)
            {
                onUpdate(i);
                i++;
                if (Time.time - _time > overTime)
                {
                    onTimeOver();
                    break;
                }
                yield return null;
            }
        }
    }
}