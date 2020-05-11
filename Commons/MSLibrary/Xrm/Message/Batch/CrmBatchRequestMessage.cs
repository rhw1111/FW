using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.Batch
{
    [DataContract]
    public class CrmBatchRequestMessage:CrmRequestMessage
    {
        [DataMember]
        public List<CrmRequestMessage> ChangeSetMessages { get; set; }
        [DataMember]
        public List<CrmRequestMessage> BatchMessages { get; set; }
    }
}
