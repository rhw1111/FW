using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveMultipleFetch
{
    [DataContract]
    public class CrmRetrieveMultipleFetchResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmEntityCollection Value { get; set; }
    }
}
