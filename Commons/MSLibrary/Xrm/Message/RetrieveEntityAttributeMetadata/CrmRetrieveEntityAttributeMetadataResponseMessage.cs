using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveEntityAttributeMetadata
{
    [DataContract]
    public class CrmRetrieveEntityAttributeMetadataResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public object Result { get; set; }
    }
}
