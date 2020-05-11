using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.RetrieveEntityMetadataMultiple
{
    [DataContract]
    public class CrmRetrieveEntityMetadataMultipleRequestMessage:CrmRequestMessage
    {
        public CrmRetrieveEntityMetadataMultipleRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveEntityMetadataMultiple;
        }
        [DataMember]
       public string QueryExpression { get; set; }
    }
}
