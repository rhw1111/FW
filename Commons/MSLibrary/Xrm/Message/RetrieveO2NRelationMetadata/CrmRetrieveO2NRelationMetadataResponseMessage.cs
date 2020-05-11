using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm.Message.RetrieveO2NRelationMetadata
{
    [DataContract]
    public class CrmRetrieveO2NRelationMetadataResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmOneToManyRelationshipMetadata Result { get; set; }
    }
}
