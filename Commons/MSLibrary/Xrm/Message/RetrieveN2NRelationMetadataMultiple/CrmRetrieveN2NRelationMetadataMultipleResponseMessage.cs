using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Xrm.Metadata;


namespace MSLibrary.Xrm.Message.RetrieveN2NRelationMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveN2NRelationMetadataMultipleResponseMessage:CrmResponseMessage
    {
        [DataMember]
        public List<CrmManyToManyRelationshipMetadata> Result { get; set; }
    }
}
