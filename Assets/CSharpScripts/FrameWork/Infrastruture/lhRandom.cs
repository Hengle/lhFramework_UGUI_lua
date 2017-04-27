using UnityEngine;
using System.Collections;

namespace LaoHan.Infrastruture
{
    public class lhRandom
    {


        private int m_seed;

        // magic number
        private int m_a = 31;
        private int m_c = 53;
        private int m_m = int.MaxValue;

        private static lhRandom m_instance;
        public static lhRandom GetInstance(int seed)
        {
            if (m_instance != null) return null;
            return m_instance = new lhRandom(seed);
        }
        /// <summary>
        /// avoid some mistake in use, seed must be provided while Constructing.
        /// NOTE: one thread one instance.
        /// </summary>
        /// <param name="seed">Seed.</param>
        public lhRandom(int seed)
        {
            m_seed = System.Math.Abs(seed);
        }

        /// <summary>
        /// c# Random compatable.
        /// </summary>
        // 0 ~ int.MaxValue
        public static int Next()
        {
            m_instance.m_seed = (m_instance.m_seed * m_instance.m_a + m_instance.m_c) % m_instance.m_m;
            return m_instance.m_seed;
        }

        //0 ~ up
        public static int Next(int max)
        {
            return Next() % max;
        }

        //down ~ up
        public static int Next(int from, int to)
        {
            return Next() % (to - from) + from;
        }

        //0.0 ~ 1.0
        public static double NextDouble()
        {
            return Next() / (double)m_instance.m_m;
        }

        /// <summary>
        /// unity3d Random compatable
        /// </summary>
        // down ~ up
        public static int Range(int from, int to)
        {
            return Next(from, to);
        }
        public static float Range(float from, float to)
        {
            return (float)NextDouble() + Range((int)from, (int)System.Math.Floor(to));
        }
        public static Vector3 Range(Vector3 from, Vector3 to)
        {
            return new Vector3(Range(from.x, to.x), Range(from.y, to.y), Range(from.z, to.z));
        }
        public static Vector2 Range(Vector2 from, Vector2 to)
        {
            return new Vector2(Range(from.x, to.x), Range(from.y, to.y));
        }
        public static int seed
        {
            get
            {
                return m_instance.m_seed;
            }
        }

        public static double value
        {
            get
            {
                return NextDouble();
            }
        }

    }
}