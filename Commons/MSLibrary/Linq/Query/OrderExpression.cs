using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Linq.Query
{
    /// <summary>
    /// 排序表达式
    /// </summary>
    [DataContract]
    public class OrderExpression
    {
        public OrderExpression()
        {

        }
        public OrderExpression(string attributeName, OrderType orderType)
        {
            AttributeName = attributeName;
            OrderType = orderType;
        }
        /// <summary>
        /// 参数排序的属性名称
        /// </summary>
        [DataMember]
        public string AttributeName
        {
            get;set;
        }
        /// <summary>
        /// 排序类型
        /// </summary>
        [DataMember]
        public OrderType OrderType
        {
            get;set;
        }
    }
}
