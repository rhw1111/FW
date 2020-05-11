using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{

    [DataContract]
    public class CrmOptionSetMetadataBase:CrmMetadataBase
    {
        [DataMember]
        public CrmLabel Description { get; set; }

        [DataMember]
        public CrmLabel DisplayName { get; set; }

        [DataMember]
        public bool? IsCustomOptionSet { get; set; }

        [DataMember]
        public bool? IsGlobal { get; set; }

        [DataMember]
        public bool? IsManaged { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsCustomizable { get; set; }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ExternalTypeName { get; set; }
        [DataMember]
        public CrmOptionSetType? OptionSetType { get; set; }

        [DataMember]
        public string IntroducedVersion { get; set; }
    }
}
