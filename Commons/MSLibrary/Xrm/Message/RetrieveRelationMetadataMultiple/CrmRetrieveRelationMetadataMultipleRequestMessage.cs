using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveRelationMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveRelationMetadataMultipleRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveRelationMetadataMultipleRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveRelationMetadataMultiple;
        }

        [DataMember]
        public string QueryExpression { get; set; }
    }
}
