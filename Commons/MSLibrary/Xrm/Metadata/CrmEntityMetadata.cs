using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public class CrmEntityMetadata: CrmMetadataBase
    {

        [DataMember]
        public string LogicalName { get; set; }

        [DataMember]
        public bool? IsQuickCreateEnabled { get; set; }

        [DataMember]
        public bool? IsReadingPaneEnabled { get; set; }
        
        [DataMember]
        public string MobileOfflineFilters { get; set; }
        
        [DataMember]
        public int? DaysSinceRecordLastModified { get; set; }
        
        [DataMember]
        public CrmBooleanManagedProperty IsOfflineInMobileClient { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsReadOnlyInMobileClient { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsVisibleInMobileClient { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsVisibleInMobile { get; set; }

        [DataMember]
        public bool? IsValidForAdvancedFind { get; set; }

        [DataMember]
        public bool? IsEnabledForTrace { get; set; }

        [DataMember]
        public bool? IsEnabledForCharts { get; set; }

        [DataMember]
        public bool? IsManaged { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsMailMergeEnabled { get; set; }

        [DataMember]
        public bool? IsIntersect { get; set; }

        [DataMember]
        public bool? IsImportable { get; set; }
        
        [DataMember]
        public CrmBooleanManagedProperty CanChangeTrackingBeEnabled { get; set; }

        [DataMember]
        public bool? ChangeTrackingEnabled { get; set; }

        [DataMember]
        public bool? IsOptimisticConcurrencyEnabled { get; set; }

        [DataMember]
        public CrmManyToManyRelationshipMetadata[] ManyToManyRelationships { get; set; }

        [DataMember]
        public CrmOneToManyRelationshipMetadata[] ManyToOneRelationships { get; set; }

        [DataMember]
        public CrmOneToManyRelationshipMetadata[] OneToManyRelationships { get; set; }

        [DataMember]
        public int? ObjectTypeCode { get; set; }

        [DataMember]
        public bool? IsPrivate { get; set; }

        [DataMember]
        public bool? IsEnabledForExternalChannels { get; set; }

        [DataMember]
        public string EntitySetName { get; set; }

        [DataMember]
        public string CollectionSchemaName { get; set; }
        
        [DataMember]
        public string ExternalCollectionName { get; set; }

        [DataMember]
        public string LogicalCollectionName { get; set; }

        [DataMember]
        public CrmEntityKeyMetadata[] Keys { get; set; }

        [DataMember]
        public string EntityColor { get; set; }
        
        [DataMember]
        public string ExternalName { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty CanChangeHierarchicalRelationship { get; set; }

        [DataMember]
        public bool? EnforceStateTransitions { get; set; }

        [DataMember]
        public string IntroducedVersion { get; set; }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public string ReportViewName { get; set; }

        [DataMember]
        public string RecurrenceBaseEntityLogicalName { get; set; }

        [DataMember]
        public CrmSecurityPrivilegeMetadata[] Privileges { get; set; }

        [DataMember]
        public string PrimaryIdAttribute { get; set; }

        [DataMember]
        public string PrimaryImageAttribute { get; set; }

        [DataMember]
        public string PrimaryNameAttribute { get; set; }

        [DataMember]
        public CrmOwnershipTypes? OwnershipType { get; set; }
        
        [DataMember]
        public bool? IsStateModelAware { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty CanModifyAdditionalSettings { get; set; }

        [DataMember]
        public bool? SyncToExternalSearchIndex { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty CanEnableSyncToExternalSearchIndex { get; set; }

        [DataMember]
        public bool? IsActivity { get; set; }

        [DataMember]
        public bool? AutoCreateAccessTeams { get; set; }
        
        [DataMember]
        public Guid? DataSourceId { get; set; }
        
        [DataMember]
        public Guid? DataProviderId { get; set; }

        [DataMember]
        public bool? IsDocumentRecommendationsEnabled { get; set; }

        [DataMember]
        public bool? IsBPFEntity { get; set; }

        [DataMember]
        public bool? IsSLAEnabled { get; set; }

        [DataMember]
        public bool? IsKnowledgeManagementEnabled { get; set; }

        [DataMember]
        public bool? IsInteractionCentricEnabled { get; set; }

        [DataMember]
        public bool? IsActivityParty { get; set; }

        [DataMember]
        public bool? IsOneNoteIntegrationEnabled { get; set; }

        [DataMember]
        public string EntityHelpUrl { get; set; }

        [DataMember]
        public bool? EntityHelpUrlEnabled { get; set; }

        [DataMember]
        public CrmLabel DisplayName { get; set; }

        [DataMember]
        public CrmLabel DisplayCollectionName { get; set; }

        [DataMember]
        public CrmLabel Description { get; set; }

        [DataMember]
        public bool? CanTriggerWorkflow { get; set; }

        [DataMember]
        public bool? AutoRouteToOwnerQueue { get; set; }

        [DataMember]
        public CrmAttributeMetadata[] Attributes { get; }

        [DataMember]
        public int? ActivityTypeMask { get; set; }

        [DataMember]
        public bool? IsDocumentManagementEnabled { get; set; }
        
        [DataMember]
        public bool? UsesBusinessDataLabelTable { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsAuditEnabled { get; set; }

        [DataMember]
        public bool? IsChildEntity { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty CanBeInManyToMany { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty CanBePrimaryEntityInRelationship { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty CanBeRelatedEntityInRelationship { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty CanCreateCharts { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty CanCreateViews { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty CanCreateForms { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty CanCreateAttributes { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsDuplicateDetectionEnabled { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsMappable { get; set; }

        [DataMember]
        public bool? IsAvailableOffline { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsRenameable { get; set; }

        [DataMember]
        public bool? IsBusinessProcessEnabled { get; set; }

        [DataMember]
        public bool? IsCustomEntity { get; set; }
        //
        [DataMember]
        public string IconVectorName { get; set; }

        [DataMember]
        public string IconSmallName { get; set; }

        [DataMember]
        public string IconMediumName { get; set; }

        [DataMember]
        public string IconLargeName { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsConnectionsEnabled { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsValidForQueue { get; set; }

        [DataMember]
        public bool? IsAIRUpdated { get; set; }

        [DataMember]
        public CrmBooleanManagedProperty IsCustomizable { get; set; }
        //
        [DataMember]
        public bool? IsLogicalEntity { get; set; }
    }
}
