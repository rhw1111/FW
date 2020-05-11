using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace MSLibrary.Xrm.Message.RetrieveMultipleFetch
{
    [DataContract]
    public class CrmRetrieveMultipleFetchRequestMessage : CrmRequestMessage
    {
        public CrmRetrieveMultipleFetchRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.RetrieveMultipleFetch;
        }
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public XDocument FetchXml { get; set; }
        [DataMember]
        public string AdditionalQueryExpression { get; set; }
        [DataMember]
        public bool NeedPagingCookie { get; set; }
    }
}
