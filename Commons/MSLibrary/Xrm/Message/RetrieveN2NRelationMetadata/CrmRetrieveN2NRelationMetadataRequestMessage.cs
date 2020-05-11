using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveN2NRelationMetadata
{
    [DataContract]
    public class CrmRetrieveN2NRelationMetadataRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveN2NRelationMetadataRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveN2NRelationMetadata;
        }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public Guid MetadataId { get; set; }
        [DataMember]
        public string QueryExpression { get; set; }
    }
}
