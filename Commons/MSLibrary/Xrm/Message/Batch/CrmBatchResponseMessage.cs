using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.Batch
{
    [DataContract]
    public class CrmBatchResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public List<CrmResponseMessage> ChangeSetResponses { get; set; }
        [DataMember]
        public List<CrmResponseMessage> BatchResponses { get; set; }
    }
}
