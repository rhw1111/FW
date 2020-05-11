using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveRelationMetadata
{
    [DataContract]
    public class CrmRetrieveRelationMetadataRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveRelationMetadataRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveRelationMetadata;
        }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public Guid MetadataId { get; set; }
        [DataMember]
        public string QueryExpression { get; set; }
    }
}
