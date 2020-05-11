using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveGlobalOptionSetMetadata
{
    [DataContract]
    public class CrmRetrieveGlobalOptionSetMetadataRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveGlobalOptionSetMetadataRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveGlobalOptionSetMetadata;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Guid MetadataId { get; set; }
        [DataMember]
        public string QueryExpression { get; set; }
    }
}
