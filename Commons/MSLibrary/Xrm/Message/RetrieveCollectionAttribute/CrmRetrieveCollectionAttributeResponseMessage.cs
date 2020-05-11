using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveCollectionAttribute
{
    [DataContract]
    public class CrmRetrieveCollectionAttributeResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmEntityCollection Value { get; set; }
    }
}
