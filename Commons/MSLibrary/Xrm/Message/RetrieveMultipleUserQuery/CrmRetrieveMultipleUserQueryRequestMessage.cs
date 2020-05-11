using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveMultipleUserQuery
{
    [DataContract]
    public class CrmRetrieveMultipleUserQueryRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveMultipleUserQueryRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveMultipleUserQuery;
        }
        /// <summary>
        /// 实体名称
        /// </summary>
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public string AdditionalQueryExpression { get; set; }
        [DataMember]
        public Guid UserQueryId { get; set; }
    }
}
