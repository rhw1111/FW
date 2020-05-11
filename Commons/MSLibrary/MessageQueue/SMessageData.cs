using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.MessageQueue
{
    [DataContract]
    public class SMessageData
    {
        [DataMember]
        public string Key
        {
            get; set;
        }

        [DataMember]
        public string Type
        {
            get; set;
        }

        [DataMember]
        public string Data
        {
            get; set;
        }
        [DataMember]
        public DateTime ExpectationExecuteTime
        {
            get;set;
        }
    }
}
