using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.BoundFunction
{
    [DataContract]
    public class CrmBoundFunctionRequestMessage:CrmRequestMessage
    {
        public CrmBoundFunctionRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.BoundFunction;
        }

        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public Guid EntityId { get; set; }
        [DataMember]
        public Dictionary<string, object> AlternateKeys { get; set; }

        [DataMember]
        public string FunctionName { get; set; }
        [DataMember]
        public List<CrmFunctionParameter> Parameters { get; set; }
    }
}
