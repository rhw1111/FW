using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveCollectionAttributeSavedQuery
{
    [DataContract]
    public class CrmRetrieveCollectionAttributeSavedQueryResponseMessage: CrmResponseMessage
    {
        [DataMember]
        public CrmEntityCollection Value { get; set; }
    }
}
