using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public class CrmOptionSetMetadata: CrmOptionSetMetadataBase
    {
        [DataMember]
        public CrmOptionMetadataCollection Options { get; set; }
    }
}
