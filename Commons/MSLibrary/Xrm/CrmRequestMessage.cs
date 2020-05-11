using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm
{
    /// <summary>
    /// Crm请求消息
    /// </summary>
    [DataContract]
    public class CrmRequestMessage
    {
       public CrmRequestMessage()
        {
            Headers = new Dictionary<string, IEnumerable<string>>();
        }
        /// <summary>
        /// 消息名称
        /// </summary>
        [DataMember]
        public string MessageName { get; protected set; }

        /// <summary>
        /// Crm组织的Uri
        /// </summary>
        [DataMember]
        public string OrganizationURI { get; internal set; }
        /// <summary>
        /// Api的版本
        /// </summary>
        [DataMember]
        public string ApiVersion { get; internal set; }
        /// <summary>
        /// 当请求遇到limit时，最大重试次数
        /// </summary>
        [DataMember]
        public int MaxRetry { get; internal set; }

        /// <summary>
        /// 附加的请求头
        /// </summary>
        [DataMember]
        public Dictionary<string, IEnumerable<string>> Headers
        {
            get;
        }

        /// <summary>
        /// 要被代理的用户Id
        /// </summary>
        [DataMember]
        public Guid? ProxyUserId { get; set; }
    }


    /// <summary>
    /// 预设的Crm请求消息名称
    /// </summary>
    public static class CrmRequestMessageExistsNames
    {
        public const string Create = "Create";
        public const string CreateRetrieve = "CreateRetrieve";
        public const string Update = "Update";
        public const string UpdateRetrieve = "UpdateRetrieve";
        public const string Delete = "Delete";
        public const string Upsert = "Upsert";
        public const string UpsertRetrieve = "UpsertRetrieve";
        public const string Retrieve = "Retrieve";
        public const string RetrieveAggregation = "RetrieveAggregation";
        public const string RetrieveSignleAttribute = "RetrieveSignleAttribute";
        public const string RetrieveLookupAttribute = "RetrieveLookupAttribute";
        public const string RetrieveLookupAttributeReference = "RetrieveLookupAttributeReference";
        public const string RetrieveCollectionAttribute = "RetrieveCollectionAttribute";
        public const string RetrieveCollectionAttributeSavedQuery = "RetrieveCollectionAttributeSavedQuery";
        public const string RetrieveCollectionAttributeUserQuery = "RetrieveCollectionAttributeUserQuery";
        public const string RetrieveCollectionAttributeReference = "RetrieveCollectionAttributeReference";
        public const string RetrieveCollectionAttributeAggregation = "RetrieveCollectionAttributeAggregation";
        public const string RetrieveMultiple = "RetrieveMultiple";
        public const string RetrieveMultiplePage = "RetrieveMultiplePage";
        public const string RetrieveMultipleFetch = "RetrieveMultipleFetch";
        public const string RetrieveMultipleSavedQuery = "RetrieveMultipleSavedQuery";
        public const string RetrieveMultipleUserQuery = "RetrieveMultipleUserQuery";
        public const string AssociateLookup = "AssociateLookup";
        public const string DisAssociateLookup = "DisAssociateLookup";
        public const string AssociateCollection = "AssociateCollection";
        public const string DisAssociateCollection = "DisAssociateCollection";
        public const string AssociateCollectionMultiple = "AssociateCollectionMultiple";
        public const string BoundFunction = "BoundFunction";
        public const string UnBoundFunction = "UnBoundFunction";
        public const string BoundAction = "BoundAction";
        public const string UnBoundAction = "UnBoundAction";
        public const string RetrieveEntityMetadata = "RetrieveEntityMetadata";
        public const string RetrieveEntityMetadataMultiple = "RetrieveEntityMetadataMultiple";
        public const string RetrieveEntityAttributeMetadata = "RetrieveEntityAttributeMetadata";
        public const string RetrieveEntityAttributeMetadataMultiple = "RetrieveEntityAttributeMetadataMultiple";
        public const string RetrieveEntityN2NRelationMetadataMultiple = "RetrieveEntityN2NRelationMetadataMultiple";
        public const string RetrieveEntityO2NRelationMetadataMultiple = "RetrieveEntityO2NRelationMetadataMultiple";
        public const string RetrieveRelationMetadataMultiple = "RetrieveRelationMetadataMultiple";
        public const string RetrieveN2NRelationMetadataMultiple = "RetrieveN2NRelationMetadataMultiple";
        public const string RetrieveO2NRelationMetadataMultiple = "RetrieveO2NRelationMetadataMultiple";

        public const string RetrieveRelationMetadata = "RetrieveRelationMetadata";
        public const string RetrieveN2NRelationMetadata = "RetrieveN2NRelationMetadata";
        public const string RetrieveO2NRelationMetadata = "RetrieveO2NRelationMetadata";
        public const string RetrieveGlobalOptionSetMetadata = "RetrieveGlobalOptionSetMetadata";

        public const string GetFileAttributeUploadInfo = "GetFileAttributeUploadInfo";
        public const string FileAttributeUploadChunking = "FileAttributeUploadChunking";
        public const string FileAttributeDeleteData = "FileAttributeDeleteData";
        public const string FileAttributeDownloadChunking = "FileAttributeDownloadChunking";
    }
}
