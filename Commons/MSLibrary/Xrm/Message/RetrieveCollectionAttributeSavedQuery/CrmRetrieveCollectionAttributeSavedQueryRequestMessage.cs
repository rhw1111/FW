using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveCollectionAttributeSavedQuery
{
    [DataContract]
    public class CrmRetrieveCollectionAttributeSavedQueryRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveCollectionAttributeSavedQueryRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveCollectionAttributeSavedQuery;
        }

        /// <summary>
        /// 实体名称
        /// </summary>
        [DataMember]
        public string EntityName { get; set; }
        /// <summary>
        /// 实体Id
        /// </summary>
        [DataMember]
        public Guid EntityId { get; set; }
        /// <summary>
        /// 唯一键集合
        /// </summary>
        [DataMember]
        public Dictionary<string, object> AlternateKeys { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        [DataMember]
        public string AttributeName { get; set; }

        [DataMember]
        public string AdditionalQueryExpression { get; set; }
        [DataMember]
        public Guid SavedQueryId { get; set; }
    }
}
