using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveCollectionAttributeAggregation
{
    [DataContract]
    public class CrmRetrieveCollectionAttributeAggregationRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveCollectionAttributeAggregationRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveCollectionAttributeAggregation;
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
        /// 集合属性名称
        /// </summary>
        [DataMember]
        public string AttributeName { get; set; }
        /// <summary>
        /// 聚合名称
        /// </summary>
        [DataMember]
        public string Aggregation { get; set; }
    }
}
