using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm
{
    [DataContract]
    public class CrmEntity
    {
        public CrmEntity(string entityName, Guid id)
        {
            EntityName = entityName;
            Id = id;
        }
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public bool IsActivity { get; set; }
        [DataMember]
        public JObject Attributes { get; set; }
        [DataMember]
        public string Version { get; set; }
    }
}
