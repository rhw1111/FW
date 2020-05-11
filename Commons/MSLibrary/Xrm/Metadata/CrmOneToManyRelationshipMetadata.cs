using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public class CrmOneToManyRelationshipMetadata: CrmRelationshipMetadataBase
    {
        [DataMember]
        public CrmAssociatedMenuConfiguration AssociatedMenuConfiguration { get; set; }

        [DataMember]
        public CrmCascadeConfiguration CascadeConfiguration { get; set; }

        [DataMember]
        public string ReferencedAttribute { get; set; }

        [DataMember]
        public string ReferencedEntity { get; set; }

        [DataMember]
        public string ReferencingAttribute { get; set; }

        [DataMember]
        public string ReferencingEntity { get; set; }

        [DataMember]
        public bool? IsHierarchical { get; set; }
 
        [DataMember]
        public string ReferencedEntityNavigationPropertyName { get; set; }

        [DataMember]
        public string ReferencingEntityNavigationPropertyName { get; set; }
    }
}
