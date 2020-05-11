using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm.Message.RetrieveRelationMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveRelationMetadataMultipleResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public List<CrmRelationshipMetadataBase> Result { get; set; }
    }
}
