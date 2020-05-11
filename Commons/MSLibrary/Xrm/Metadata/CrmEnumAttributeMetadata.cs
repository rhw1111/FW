using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{

    [DataContract]
    public class CrmEnumAttributeMetadata : CrmAttributeMetadata
    {
        [DataMember]
        public int? DefaultFormValue { get; set; }
        [DataMember]
        public CrmOptionSetMetadata OptionSet { get; set; }
    }
}
