using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveMultiplePage
{
    [DataContract]
    public class CrmRetrieveMultiplePageRequestMessage : CrmRequestMessage
    {
        public CrmRetrieveMultiplePageRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveMultiplePage;
        }

        /// <summary>
        /// 实体名称
        /// </summary>
        [DataMember]
        public string EntityName { get; set; }
        /// <summary>
        /// 查询表达式
        /// </summary>
        [DataMember]
        public string QueryExpression { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }
    }
}
