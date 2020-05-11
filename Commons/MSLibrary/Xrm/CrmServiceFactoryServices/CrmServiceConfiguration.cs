using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.CrmServiceFactoryServices
{
    [DataContract]
    public class CrmServiceConfiguration
    {
        [DataMember]
        public string TokenServiceType { get; set; }

        [DataMember]
        public string CrmUrl { get; set; }

        [DataMember]
        public string CrmApiVersion { get; set; }
        [DataMember]
        public int CrmApiMaxRetry { get; set; }
        [DataMember]
        public Dictionary<string, string> TokenServiceParameters { get; set; }

    }
}
