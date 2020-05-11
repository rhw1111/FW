using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm
{
    [DataContract]
    public abstract class CrmManagedProperty<T>
    {

        [DataMember]
        public T Value { get; set; }

        [DataMember]
        public bool CanBeChanged { get; set; }

        [DataMember]
        public string ManagedPropertyLogicalName { get; set; }
    }
}
