using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm.Message.UnBoundFunction
{
    [DataContract]
    public class CrmUnBoundFunctionResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public JObject Value { get; set; }
    }
}
