using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Survey.SurveyResponseExtension.SurveyMonkey
{
    [DataContract]
    public class SurveyData
    {
        [DataMember]
        public string ID { get; set; } = null!;
    }
}
