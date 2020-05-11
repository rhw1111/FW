using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm.Message.RetrieveEntityAttributeMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveEntityAttributeMetadataMultipleResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public List<CrmAttributeMetadata> Result { get; set; }
    }
}
