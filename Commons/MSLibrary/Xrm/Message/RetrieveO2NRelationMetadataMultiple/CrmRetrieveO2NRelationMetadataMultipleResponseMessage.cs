using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm.Message.RetrieveO2NRelationMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveO2NRelationMetadataMultipleResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public List<CrmOneToManyRelationshipMetadata> Result { get; set; }
    }
}
