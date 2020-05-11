using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm
{
    [DataContract]
    public class CrmExecuteEntity
    {
        public CrmExecuteEntity(string entityName, Guid id)
        {
            EntityName = entityName;
            Id = id;
            Attributes = new Dictionary<string, object>();
        }
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string EntityName { get; set; }
        /// <summary>
        /// 是否是活动实体
        /// </summary>
        [DataMember]
        public bool IsActivity { get; set; }
        [DataMember]
        public Dictionary<string,object> Attributes { get; set; }
        [DataMember]
        public string Version { get; set; }
    }
}
