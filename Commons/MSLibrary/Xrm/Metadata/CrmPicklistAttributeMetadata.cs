using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{

    [DataContract]   
    public class CrmPicklistAttributeMetadata : CrmEnumAttributeMetadata
    {
        [DataMember]
        public string FormulaDefinition { get; set; }
        [DataMember]
        public int? SourceTypeMask { get; set; }
    }
}
