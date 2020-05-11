using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.UnBoundFunction
{
    [DataContract]
    public class CrmUnBoundFunctionRequestMessage:CrmRequestMessage
    {
        public CrmUnBoundFunctionRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.UnBoundFunction;
        }

        [DataMember]
        public string FunctionName { get; set; }
        [DataMember]
        public List<CrmFunctionParameter> Parameters { get; set; }
    }
}
