using UnityEngine;
using System.Collections;
using System;

namespace LaoHan.Infrastruture
{
    public sealed class lhTime
    {
        /// <summary>
        /// 时间戳转换为时间(自动后加7个零)
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToDataTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        /// <summary>
        /// 时间转换为时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int DataTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        /// <summary>
        /// 获取本地系统时间 yyyy年MM月dd HH时mm分ss秒
        /// </summary>
        /// <returns></returns>
        public static string LocalSystemTime()
        {
            return System.DateTime.Now.ToString("yyyy年MM月dd HH时mm分ss秒");
        }
        /// <summary>
        /// 根据时间戳来获取是周几
        /// </summary>
        /// <param name="timeStamps"></param>
        /// <returns></returns>
        public static string DayOfWeek(string timeStamps)
        {
            System.DateTime _dateTime = StampToDataTime(timeStamps);
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return Day[System.Convert.ToInt16(_dateTime.DayOfWeek)];
        }
    }
}
