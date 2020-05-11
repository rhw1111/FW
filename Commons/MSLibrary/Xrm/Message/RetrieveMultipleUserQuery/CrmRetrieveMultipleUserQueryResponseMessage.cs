using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveMultipleUserQuery
{
    [DataContract]
    public class CrmRetrieveMultipleUserQueryResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmEntityCollection Value { get; set; }
    }
}
