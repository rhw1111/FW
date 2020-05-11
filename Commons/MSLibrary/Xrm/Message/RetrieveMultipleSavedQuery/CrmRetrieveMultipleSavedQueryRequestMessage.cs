using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveMultipleSavedQuery
{
    [DataContract]
    public class CrmRetrieveMultipleSavedQueryRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveMultipleSavedQueryRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveMultipleSavedQuery;
        }

        /// <summary>
        /// 实体名称
        /// </summary>
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public string AdditionalQueryExpression { get; set; }
        [DataMember]
        public Guid SavedQueryId { get; set; }
    }
}
