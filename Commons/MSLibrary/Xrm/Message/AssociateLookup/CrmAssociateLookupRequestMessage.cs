using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.AssociateLookup
{
    [DataContract]
    public class CrmAssociateLookupRequestMessage: CrmRequestMessage
    {
        public CrmAssociateLookupRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.AssociateLookup;
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
        /// <summary>
        /// 关联的实体名称
        /// </summary>
        [DataMember]
        public string AssociateEntityName { get; set; }
        /// <summary>
        /// 关联的实体Id
        /// </summary>
        [DataMember]
        public Guid AssociateEntityId { get; set; }

    }
}
