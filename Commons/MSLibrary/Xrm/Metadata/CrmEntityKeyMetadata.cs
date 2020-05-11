using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public class CrmEntityKeyMetadata:CrmMetadataBase
    {
        [DataMember]
        public CrmLabel DisplayName { get; set; }

        [DataMember]
        public string LogicalName { get; set; }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public string EntityLogicalName { get; set; }

        [DataMember]
        public string[] KeyAttributes { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsCustomizable { get; set; }

        [DataMember]
        public bool? IsManaged { get; set; }

        [DataMember]
        public string IntroducedVersion { get; set; }

        [DataMember]
        public CrmEntityKeyIndexStatus EntityKeyIndexStatus { get; set; }
    }
}
