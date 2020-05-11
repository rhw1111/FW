using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.UnBoundAction
{
    [DataContract]
    public class CrmUnBoundActionRequestMessage:CrmRequestMessage
    {
        public CrmUnBoundActionRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.UnBoundAction;
        }

        [DataMember]
        public string ActionName { get; set; }
        [DataMember]
        public List<CrmActionParameter> Parameters { get; set; }
    }
}
