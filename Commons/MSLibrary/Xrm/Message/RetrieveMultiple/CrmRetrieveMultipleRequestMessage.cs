using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveMultiple
{
    [DataContract]
    public class CrmRetrieveMultipleRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveMultipleRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveMultiple;
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
    }
}
