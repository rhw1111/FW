using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm.Message.RetrieveEntityMetadata
{
    [DataContract]
    public class CrmRetrieveEntityMetadataResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmEntityMetadata EntityMetadata { get; set; }
    }
}
