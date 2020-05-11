using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm
{
    [DataContract]
    public class CrmLocalizedLabel: CrmMetadataBase
    {
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public int LanguageCode { get; set; }
        [DataMember]
        public bool? IsManaged { get; set; }
    }
}
