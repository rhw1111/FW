using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm
{
    [DataContract]
    public class CrmEntityReference
    {
        public CrmEntityReference(string entityName,Guid id)
        {
            EntityName = entityName;
            Id = id;
        }
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public Guid Id { get; set; }
    }

    [DataContract]
    public class CrmEntityReferenceNull
    {
        public CrmEntityReferenceNull(string entityName)
        {
            EntityName = entityName;
        }
        [DataMember]
        public string EntityName { get; set; }
    }
}
