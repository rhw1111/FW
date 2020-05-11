using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public abstract class CrmConstantsBase<T>
    {

        [DataMember]
        public T Value { get; set; }
    }
}
