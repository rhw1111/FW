using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Survey.SurveyResponseExtension.SurveyMonkey
{
    [DataContract]
    public class ResponseData
    {
        [DataMember]
        public string ID { get; set; } = null!;
        [DataMember]
        public string SurveyID { get; set; } = null!;
        [DataMember]
        public string CollectorID { get; set; } = null!;
        [DataMember]
        public string? RecipientID { get; set; }
        [DataMember]
        public int TotalTime { get; set; }
        [DataMember]
        public string? CustomValue { get; set; }
        [DataMember]
        public string IPAddress { get; set; } = null!;
        [DataMember]
        public Dictionary<string,string> CustomVariables { get; set; } = null!;
        [DataMember]
        public string ResponseStatus { get; set; } = null!;
        [DataMember]
        public string CollectionMode { get; set; } = null!;
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime DateModified { get; set; }
    }
}
