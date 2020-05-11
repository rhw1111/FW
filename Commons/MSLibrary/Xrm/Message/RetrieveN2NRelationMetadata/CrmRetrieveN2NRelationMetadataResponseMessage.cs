using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;

namespace MSLibrary.Xrm.Message.RetrieveN2NRelationMetadata
{
    [DataContract]
    public class CrmRetrieveN2NRelationMetadataResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public CrmManyToManyRelationshipMetadata Result { get; set; }
    }
}
