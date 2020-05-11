using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public abstract class CrmMetadataBase
    {
        [DataMember]
        public Guid? MetadataId { get; set; }
        [DataMember]
        public bool? HasChanged { get; set; }
    }
}
