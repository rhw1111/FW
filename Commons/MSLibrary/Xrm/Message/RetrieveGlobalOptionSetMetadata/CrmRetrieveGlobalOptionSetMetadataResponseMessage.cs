using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm.Message.RetrieveGlobalOptionSetMetadata
{
    [DataContract]
    public class CrmRetrieveGlobalOptionSetMetadataResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmOptionSetMetadata Result { get; set; }
    }
}
