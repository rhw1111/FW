using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm.Message.RetrieveSignleAttribute
{
    [DataContract]
    public class CrmRetrieveSignleAttributeResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public JToken Value { get; set; }
    }
}
