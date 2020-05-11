using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm.Message.RetrieveEntityO2NRelationMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveEntityO2NRelationMetadataMultipleResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public List<CrmOneToManyRelationshipMetadata> Result { get; set; }
    }
}
