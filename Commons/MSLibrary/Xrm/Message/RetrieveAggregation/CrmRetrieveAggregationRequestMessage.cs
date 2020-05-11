using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveAggregation
{
    [DataContract]
    public class CrmRetrieveAggregationRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveAggregationRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveAggregation;
        }
        /// <summary>
        /// 实体名称
        /// </summary>
        [DataMember]
        public string EntityName { get; set; }
        /// <summary>
        /// 聚合表达式
        /// </summary>
        [DataMember]
        public string Aggregation { get; set; }

    }
}
