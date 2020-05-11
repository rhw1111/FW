using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm.Message.RetrieveCollectionAttributeReference
{
    [DataContract]
    public class CrmRetrieveCollectionAttributeReferenceResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmEntityReferenceCollection Value { get; set; }
    }
}
