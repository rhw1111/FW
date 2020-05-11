using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public class CrmRelationshipMetadataBase: CrmMetadataBase
    {      
        [DataMember]
        public bool? IsCustomRelationship { get; set; }
        [DataMember]
        public CrmBooleanManagedProperty IsCustomizable { get; set; }
        [DataMember]
        public bool? IsValidForAdvancedFind { get; set; }
        [DataMember]
        public string SchemaName { get; set; }
        [DataMember]
        public CrmSecurityTypes? SecurityTypes { get; set; }
        [DataMember]
        public bool? IsManaged { get; set; }
        [DataMember]
        public CrmRelationshipType RelationshipType { get; set; }
        [DataMember]
        public string IntroducedVersion { get; set; }
    }
}
