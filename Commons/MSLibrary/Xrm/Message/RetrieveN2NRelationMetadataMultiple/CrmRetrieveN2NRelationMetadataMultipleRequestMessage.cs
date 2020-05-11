using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveN2NRelationMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveN2NRelationMetadataMultipleRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveN2NRelationMetadataMultipleRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveN2NRelationMetadataMultiple;
        }

        [DataMember]
        public string QueryExpression { get; set; }
    }
}
