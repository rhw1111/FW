using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Linq.Query
{
    /// <summary>
    /// 过滤表达式
    /// </summary>
    [DataContract]
    public class FilterExpression
    {
        /// <summary>
        /// 包含的条件
        /// </summary>
        [DataMember]
        public List<ConditionExpression> Conditions { get; } = new List<ConditionExpression>();
        /// <summary>
        /// 过滤逻辑操作符
        /// </summary>
        [DataMember]
        public LogicalOperator FilterOperator { get; set; }
        /// <summary>
        /// 包含的子过滤表达式列表
        /// </summary>
        [DataMember]
        public List<FilterExpression> Filters { get; } = new List<FilterExpression>();
    }
}
