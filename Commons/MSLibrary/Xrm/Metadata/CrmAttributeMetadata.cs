using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public class CrmAttributeMetadata:CrmMetadataBase
    {
        [DataMember]
        public bool? IsManaged { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsGlobalFilterEnabled { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsSortableEnabled { get; set; }

        [DataMember]
        public Guid? LinkedAttributeId { get; set; }

        [DataMember]
        public string LogicalName { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsCustomizable { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsRenameable { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsValidForAdvancedFind { get; set; }

        [DataMember]
        public bool? IsValidForForm { get; set; }

        [DataMember]
        public bool? IsRequiredForForm { get; set; }

        [DataMember]
        public bool? IsValidForGrid { get; set; }

        [DataMember]
        public CrmAttributeRequiredLevelManagedProperty RequiredLevel { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty CanModifyAdditionalSettings { get; set; }

        [DataMember]
        public string SchemaName { get; set; }
  
        [DataMember]
        public string ExternalName { get; set; }

        [DataMember]
        public bool? IsLogical { get; set; }
       
        [DataMember]
        public bool? IsDataSourceSecret { get; set; }
     
        [DataMember]
        public string InheritsFrom { get; set; }
        
        [DataMember]
        public bool? IsSearchable { get; set; }
        
        [DataMember]
        public bool? IsFilterable { get; set; }

        [DataMember]
        public bool? IsSecured { get; set; }

        [DataMember]
        public int? SourceType { get; set; }

        [DataMember]
        public string AttributeOf { get; set; }

        [DataMember]
        public CrmAttributeTypeCode? AttributeType { get; set; }

        [DataMember]
        public CrmAttributeTypeDisplayName AttributeTypeName { get; set; }

        [DataMember]
        public int? ColumnNumber { get; set; }

        [DataMember]
        public CrmLabel Description { get; set; }

        [DataMember]
        public CrmLabel DisplayName { get; set; }

        [DataMember]
        public string DeprecatedVersion { get; set; }

        [DataMember]
        public string IntroducedVersion { get; set; }

        [DataMember]
        public string EntityLogicalName { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsAuditEnabled { get; set; }

        [DataMember]
        public bool? IsCustomAttribute { get; set; }

        [DataMember]
        public bool? IsPrimaryId { get; set; }

        [DataMember]
        public bool? IsPrimaryName { get; set; }

        [DataMember]
        public bool? IsValidForCreate { get; set; }

        [DataMember]
        public bool? IsValidForRead { get; set; }

        [DataMember]
        public bool? IsValidForUpdate { get; set; }

        [DataMember]
        public bool? CanBeSecuredForRead { get; set; }

        [DataMember]
        public bool? CanBeSecuredForCreate { get; set; }

        [DataMember]
        public bool? CanBeSecuredForUpdate { get; set; }
        
        [DataMember]
        public bool? IsRetrievable { get; set; }
 
        [DataMember]
        public string AutoNumberFormat { get; set; }
    }
}
