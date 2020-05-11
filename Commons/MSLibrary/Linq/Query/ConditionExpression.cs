using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Linq.Query
{
    /// <summary>
    /// 条件表达式
    /// </summary>
    [DataContract]
    public class ConditionExpression
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        [DataMember]
        public string AttributeName { get; set; }
        /// <summary>
        /// 值列表
        /// </summary>
        [DataMember]
        public List<object> Values { get; set; }

    }
}
