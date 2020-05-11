using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveEntityMetadata
{
    [DataContract]
    public class CrmRetrieveEntityMetadataRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveEntityMetadataRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveEntityMetadata;
        }

        [DataMember]
        public string EntityName { get; set; }

        [DataMember]
        public Guid MetadataId { get; set; }
        [DataMember]
        public string QueryExpression { get; set; }
    }
}
