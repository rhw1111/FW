using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm.Message.RetrieveCollectionAttributeAggregation
{
    [DataContract]
    public class CrmRetrieveCollectionAttributeAggregationResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public JToken Value { get; set; }
    }
}
