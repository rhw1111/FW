using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.NetCap
{
    /// <summary>
    /// 表示 time_t 时间类型。
    /// 又被称作 UnixTime 或 PosixTime。
    /// 表示自 1970 年 1 月 1 日起经过的秒数。
    /// </summary>
    public struct UnixTime
    {
        public static readonly DateTime MinDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
        public static readonly DateTime MaxDateTime = new DateTime(2038, 1, 19, 3, 14, 7);

        private readonly int _Value;

        public UnixTime(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("value");
            _Value = value;
        }

        public int Value
        {
            get { return _Value; }
        }

        public DateTime ToDateTime()
        {
            const long START = 621355968000000000; // 1970-1-1 00:00:00
            return new DateTime(START + (_Value * (long)10000000)).ToLocalTime();
        }

        public static UnixTime FromDateTime(DateTime dateTime)
        {
            if (dateTime < MinDateTime || dateTime > MaxDateTime)
                throw new ArgumentOutOfRangeException("dateTime");
            TimeSpan span = dateTime.Subtract(MinDateTime);
            return new UnixTime((int)span.TotalSeconds);
        }

        public override string ToString()
        {
            return ToDateTime().ToString();
        }

    }
}
