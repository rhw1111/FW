using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm.Message.RetrieveEntityMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveEntityMetadataMultipleResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public List<CrmEntityMetadata> EntityMetadatas { get; set; }
    }
}
