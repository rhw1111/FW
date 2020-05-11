using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm.Message.RetrieveAggregation
{
    [DataContract]
    public class CrmRetrieveAggregationResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public JToken Value { get; set; }
    }
}
