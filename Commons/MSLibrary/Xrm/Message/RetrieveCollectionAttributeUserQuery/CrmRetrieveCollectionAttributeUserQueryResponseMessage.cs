using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveCollectionAttributeUserQuery
{
    [DataContract]
    public class CrmRetrieveCollectionAttributeUserQueryResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmEntityCollection Value { get; set; }
    }
}
