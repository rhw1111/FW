using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public class CrmSecurityPrivilegeMetadata
    {
        [DataMember]
        public bool CanBeBasic { get; set; }
        [DataMember]
        public bool CanBeDeep { get; set; }
        [DataMember]
        public bool CanBeGlobal { get; set; }
        [DataMember]
        public bool CanBeLocal { get; set; }
        //
        [DataMember]
        public bool CanBeEntityReference { get; set; }
        [DataMember]
        public bool CanBeParentEntityReference { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid PrivilegeId { get; set; }
        [DataMember]
        public CrmPrivilegeType PrivilegeType { get; set; }
    }
}
