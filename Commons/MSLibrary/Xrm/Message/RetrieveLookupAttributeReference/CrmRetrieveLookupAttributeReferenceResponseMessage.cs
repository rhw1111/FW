using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveLookupAttributeReference
{
    [DataContract]
    public class CrmRetrieveLookupAttributeReferenceResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmEntityReference Value { get; set; }
    }
}
