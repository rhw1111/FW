using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveEntityN2NRelationMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveEntityN2NRelationMetadataMultipleRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveEntityN2NRelationMetadataMultipleRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveEntityN2NRelationMetadataMultiple;
        }

        [DataMember]
        public string EntityName { get; set; }

        [DataMember]
        public Guid EntityMetadataId { get; set; }
        [DataMember]
        public string QueryExpression { get; set; }
    }
}
