using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm.Message.UnBoundAction
{
    [DataContract]
    public class CrmUnBoundActionResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public JObject Value { get; set; }
    }
}
