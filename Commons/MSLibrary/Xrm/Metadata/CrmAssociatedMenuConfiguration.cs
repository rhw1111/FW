using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public class CrmAssociatedMenuConfiguration
    {

        [DataMember]
        public CrmAssociatedMenuBehavior? Behavior { get; set; }

        [DataMember]
        public CrmAssociatedMenuGroup? Group { get; set; }

        [DataMember]
        public CrmLabel Label { get; set; }
        [DataMember]
        public int? Order { get; set; }
        [DataMember]
        public bool IsCustomizable { get; set; }
        [DataMember]
        public string Icon { get; set; }
        [DataMember]
        public Guid ViewId { get; set; }
        [DataMember]
        public bool AvailableOffline { get; set; }
        [DataMember]
        public string MenuId { get; set; }
        [DataMember]
        public string QueryApi { get; set; }
    }
}
