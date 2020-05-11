using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm.Message.RetrieveRelationMetadata
{
    [DataContract]
    public class CrmRetrieveRelationMetadataResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmRelationshipMetadataBase Result { get; set; }
    }
}
