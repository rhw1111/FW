using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveO2NRelationMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveO2NRelationMetadataMultipleRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveO2NRelationMetadataMultipleRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveO2NRelationMetadataMultiple;
        }

        [DataMember]
        public string QueryExpression { get; set; }
    }
}
