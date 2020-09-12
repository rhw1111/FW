using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary
{
    [DataContract]
    public class ErrorMessage
    {
        [DataMember]
        public int Level { get; set; }
        [DataMember]
        public int Type { get; set; }
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
