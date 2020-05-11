using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.BoundAction
{
    [DataContract]
    public class CrmBoundActionRequestMessage:CrmRequestMessage
    {
        public CrmBoundActionRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.BoundAction;
        }

        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public Guid EntityId { get; set; }
        [DataMember]
        public Dictionary<string, object> AlternateKeys { get; set; }

        [DataMember]
        public string ActionName { get; set; }
        [DataMember]
        public List<CrmActionParameter> Parameters { get; set; }

    }
}
