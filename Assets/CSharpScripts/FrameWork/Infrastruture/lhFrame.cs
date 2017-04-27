using UnityEngine;
using System.Collections;

namespace LaoHan.Infrastruture
{
    public class lhFrame
    {
        private static lhFrame m_instance;
        public static lhFrame GetInstance(int rate)
        {
            if (m_instance != null) return null;
            return m_instance = new lhFrame(rate);
        }
        public static float fps
        {
            get
            {
                return m_instance.m_fps;
            }
        }
        public lhFrame(int rate)
        {
            this.m_rate = rate;
        }
        private int m_count;
        private float m_fps;
        private int m_rate;
        private float m_totalDeltaTime;
        private int m_minFPS;
        private int m_maxFPS;
        private bool m_autoFPS;
        // Update is called once per frame
        public void Update()
        {
            m_count++;
            m_totalDeltaTime += Time.deltaTime;
            if (m_count >= m_rate)
            {
                m_fps = 1 / (m_totalDeltaTime / m_rate);
                m_count = 0;
                m_totalDeltaTime = 0;
                if (m_autoFPS)
                {
                    if (m_fps < m_minFPS)
                        SetVerticalSynchronization(true);
                    else if (m_fps > m_maxFPS)
                        SetVerticalSynchronization(false);
                }
            }
        }
        public void AutoFPS(int minFPS, int maxFPS)
        {
            this.m_minFPS = minFPS;
            this.m_maxFPS = maxFPS;
            m_autoFPS = true;
        }
        private void SetVerticalSynchronization(bool close)
        {
            if (close)
                QualitySettings.vSyncCount = 0;
            else
                QualitySettings.vSyncCount = 1;
        }
    }
}