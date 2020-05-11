using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveEntityAttributeMetadata
{
    [DataContract]
    public class CrmRetrieveEntityAttributeMetadataRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveEntityAttributeMetadataRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveEntityAttributeMetadata;
        }

        [DataMember]
        public string EntityName { get; set; }

        [DataMember]
        public Guid EntityMetadataId { get; set; }

        [DataMember]
        public string AttributeName { get; set; }

        [DataMember]
        public Guid AttributeMetadataId { get; set; }


        [DataMember]
        public string QueryExpression { get; set; }
        [DataMember]
        public string AttributeType { get; set; }
        [DataMember]
        public string AttributeReturnType { get; set; }

    }
}
