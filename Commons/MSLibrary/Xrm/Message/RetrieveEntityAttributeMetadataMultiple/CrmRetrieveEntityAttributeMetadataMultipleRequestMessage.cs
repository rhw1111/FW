using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveEntityAttributeMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveEntityAttributeMetadataMultipleRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveEntityAttributeMetadataMultipleRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveEntityAttributeMetadataMultiple;
        }

        [DataMember]
        public string EntityName { get; set; }

        [DataMember]
        public Guid EntityMetadataId { get; set; }

        [DataMember]
        public string QueryExpression { get; set; }
    }
}
