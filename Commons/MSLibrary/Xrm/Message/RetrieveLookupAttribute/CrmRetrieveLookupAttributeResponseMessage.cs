using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveLookupAttribute
{
    [DataContract]
    public class CrmRetrieveLookupAttributeResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmEntity Value { get; set; }
    }
}
