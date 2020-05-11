using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    /// <summary>
    /// 时间的扩展方法
    /// </summary>
    public static class DateTimeExtension
    {
        public static DateTime ToCurrentUserTimeZone(this DateTime time)
        {
            //获取当前用户的时区
            var timeZone= ContextContainer.GetValue<int>(ContextTypes.CurrentUserTimezoneOffset);
            return time.AddHours(timeZone);
        }
        /// <summary>
        /// 获取转换成unix时间的毫秒数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetUnixTimeMilliseconds(this DateTime dateTime)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dateTime.Ticks - dt1970.Ticks) / 10000;
        }
    }
}
