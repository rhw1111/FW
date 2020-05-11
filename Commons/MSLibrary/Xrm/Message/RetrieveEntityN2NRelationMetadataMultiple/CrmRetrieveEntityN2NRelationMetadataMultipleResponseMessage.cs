using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm.Message.RetrieveEntityN2NRelationMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveEntityN2NRelationMetadataMultipleResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public List<CrmManyToManyRelationshipMetadata> Result { get; set; }
    }
}
