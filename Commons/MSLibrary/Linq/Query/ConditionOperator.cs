using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Linq.Query
{
    /// <summary>
    /// 条件操作符
    /// </summary>
    [DataContract]
    public enum ConditionOperator
    {
        /// <summary>
        /// 相等
        /// </summary>
        Equal=0,
        /// <summary>
        /// 不相等
        /// </summary>
        NotEqual=1,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan=2,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan=3,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEqual=4,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessEqual=5,
        /// <summary>
        /// 模糊匹配
        /// </summary>
        Like=6,
        /// <summary>
        /// 模糊匹配失败
        /// </summary>
        NotLike=7,
        /// <summary>
        /// 在多个值之间
        /// </summary>
        In=8,
        /// <summary>
        /// 不在多个值之间
        /// </summary>
        NotIn=9,
        /// <summary>
        /// 在两个值之间
        /// </summary>
        Between=10,
        /// <summary>
        /// 不在两个值之间
        /// </summary>
        NotBetween=11,
        /// <summary>
        /// 值为Null
        /// </summary>
        Null=12,
        /// <summary>
        /// 值不为Null
        /// </summary>
        NotNull=13
    }
}
