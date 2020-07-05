using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class SurveyResponseQuerySingleResponse : SurveyMonkeyResponse
    {
        public string ID { get; set; } = null!;
        public int TotalTime { get; set; }
        public string IPAddress { get; set; } = null!;
        public string RecipientID { get; set; } = null!;
        public JObject LogicPath { get; set; } = null!;
        public JObject Metadata { get; set; } = null!;

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ResponseStatus { get; set; } = null!;
        public JObject CustomVariables { get; set; } = null!;
        public string CustomValue { get; set; } = null!;
        public string EditUrl { get; set; } = null!;
        public string AnalyzeUrl { get; set; } = null!;
        public List<string> PagePath { get; set; } = null!;
        public string CollectorID { get; set; } = null!;
        public string SurveyID { get; set; } = null!;
        public string CollectionMode { get; set; } = null!;
    }
}
