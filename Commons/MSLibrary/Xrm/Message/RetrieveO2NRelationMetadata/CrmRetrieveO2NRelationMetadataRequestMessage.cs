using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveO2NRelationMetadata
{
    [DataContract]
    public class CrmRetrieveO2NRelationMetadataRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveO2NRelationMetadataRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveO2NRelationMetadata;
        }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public Guid MetadataId { get; set; }
        [DataMember]
        public string QueryExpression { get; set; }
    }
}
