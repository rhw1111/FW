using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public class CrmManyToManyRelationshipMetadata: CrmRelationshipMetadataBase
    {
        [DataMember]
        public CrmAssociatedMenuConfiguration Entity1AssociatedMenuConfiguration { get; set; }
        [DataMember]
        public CrmAssociatedMenuConfiguration Entity2AssociatedMenuConfiguration { get; set; }

        [DataMember]
        public string Entity1LogicalName { get; set; }
        [DataMember]
        public string Entity2LogicalName { get; set; }
        [DataMember]
        public string IntersectEntityName { get; set; }
        [DataMember]
        public string Entity1IntersectAttribute { get; set; }
        [DataMember]
        public string Entity2IntersectAttribute { get; set; }
        [DataMember]
        public string Entity1NavigationPropertyName { get; set; }
        [DataMember]
        public string Entity2NavigationPropertyName { get; set; }
    }
}
