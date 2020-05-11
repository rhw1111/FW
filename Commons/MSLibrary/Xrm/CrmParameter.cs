using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm
{
    [DataContract]
    public class CrmParameter
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        [DataMember]
        public string Type { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        [DataMember]
        public object Value { get; set; }
    }


    /// <summary>
    /// Crm预存参数类型
    /// </summary>
    public static class CrmParameterExistsTypes
    {
        /// <summary>
        /// 实体引用
        /// </summary>
        public const string EntityReference = "EntityReference";
        /// <summary>
        /// 实体参数
        /// </summary>
        public const string EntityParameter = "EntityParameter";
    }
}
