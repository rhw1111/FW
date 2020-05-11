using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Linq.Query
{
    /// <summary>
    /// 排序类型
    /// </summary>
    [DataContract]
    public enum OrderType
    {
        /// <summary>
        /// 正序
        /// </summary>
        Ascending=0,
        /// <summary>
        /// 反序
        /// </summary>
        Descending=1
    }
}
