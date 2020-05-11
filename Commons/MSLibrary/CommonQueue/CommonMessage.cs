using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.CommonQueue
{
    [DataContract]
    public class CommonMessage
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Data { get; set; }

        [DataMember]
        public DateTime? ExpectationExecuteTime { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }
    }
}
