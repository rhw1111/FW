using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm
{
    /// <summary>
    /// 表示Crm实体的参数
    /// </summary>
    [DataContract]
    public class CrmEntityParameter
    {
        /// <summary>
        /// 实体名称
        /// </summary>
        [DataMember]
        public string EntityName { get; set; }
        /// <summary>
        /// 实体主键键值对
        /// </summary>
        [DataMember]
        public Dictionary<string, object> Keys { get; set; }
    }
}
