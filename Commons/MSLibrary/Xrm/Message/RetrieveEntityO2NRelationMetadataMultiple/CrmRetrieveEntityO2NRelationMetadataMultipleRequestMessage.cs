using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveEntityO2NRelationMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveEntityO2NRelationMetadataMultipleRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveEntityO2NRelationMetadataMultipleRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveEntityO2NRelationMetadataMultiple;
        }

        [DataMember]
        public string EntityName { get; set; }

        [DataMember]
        public Guid EntityMetadataId { get; set; }
        [DataMember]
        public string QueryExpression { get; set; }
    }
}
