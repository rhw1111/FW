using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public class CrmCascadeConfiguration
    {

        [DataMember]
        public CrmCascadeType? Assign { get; set; }

        [DataMember]
        public CrmCascadeType? Delete { get; set; }

        [DataMember]
        public CrmCascadeType? Merge { get; set; }

        [DataMember]
        public CrmCascadeType? Reparent { get; set; }
        [DataMember]
        public CrmCascadeType? Share { get; set; }
        [DataMember]
        public CrmCascadeType? Unshare { get; set; }

        [DataMember]
        public CrmCascadeType? RollupView { get; set; }
    }
}
