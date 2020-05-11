using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.Retrieve
{
    [DataContract]
    public class CrmRetrieveRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.Retrieve;
        }

        /// <summary>
        /// 实体名称
        /// </summary>
        [DataMember]
        public string EntityName { get; set; }
        /// <summary>
        /// 实体Id
        /// </summary>
        [DataMember]
        public Guid EntityId { get; set; }
        /// <summary>
        /// 记录版本号
        /// </summary>
        [DataMember]
        public string Version { get; set; }
        /// <summary>
        /// 唯一键集合
        /// </summary>
        [DataMember]
        public Dictionary<string, object> AlternateKeys { get; set; }
        /// <summary>
        /// 查询表达式
        /// </summary>
        [DataMember]
        public string QueryExpression { get; set; }
    }
}
