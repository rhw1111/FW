using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public class CrmOptionMetadata:CrmMetadataBase
    {

        [DataMember]
        public int? Value { get; set; }

        [DataMember]
        public CrmLabel Label { get; set; }

        [DataMember]
        public CrmLabel Description { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public bool? IsManaged { get; set; }
        [DataMember]
        public string ExternalValue { get; set; }
    }
}
