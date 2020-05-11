using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    /// <summary>
    /// 针对long类型的扩展方法
    /// </summary>
    public static class LongExtension
    {
        /// <summary>
        /// 在数字的前面补0，使其达到指定长度
        /// 如果该数字已经超过指定长度，则返回该数字的实际字符串
        /// </summary>
        /// <param name="v"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetPrefixComplete(this long v,int length)
        {
            var strV = Convert.ToString(v);
            if (strV.Length>=length)
            {
                return strV;
            }

            StringBuilder strPrefix = new StringBuilder();
            for(var index=0;index<=length-strV.Length-1;index++)
            {
                strPrefix.Append("0");
            }

            return strPrefix.Append(strV).ToString();
        }
    }
}
