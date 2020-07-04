using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class SurveyResponseQueryResponse : SurveyMonkeyResponse
    {
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<SurveyResponse> Data { get; set; } = null!;
    }

    public class SurveyResponseAnswer
    {
        public int ChoiceID { get; set; }
        public int RowID { get; set; }
        public int ColID { get; set; }
        public int OtherID { get; set; }
        public string? Text { get; set; }
        public string DownloadUrl { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }

    public class SurveyResponseQuestion
    {
        public int ID { get; set; }
        public int VariableID { get; set; }

        public List<SurveyResponseAnswer> Answers { get; set; } = null!;
    }

    public class SurveyResponsePage
    {
        public int ID { get; set; }
        public List<SurveyResponseQuestion> Questions { get; set; } = null!;
    }

    public class SurveyResponse
    {
        public string ID { get; set; } = null!;
        public string Href { get; set; } = null!;
        public string SurveyID { get; set; } = null!;
        public string CollectorID { get; set; } = null!;
        public string? RecipientID { get; set; }
        public int TotalTime { get; set; }
        public string? CustomValue { get; set; }
        public string EditUrl { get; set; } = null!;
        public string AnalyzeUrl { get; set; } = null!;
        public string IPAddress { get; set; } = null!;
        public JObject CustomVariables { get; set; } = null!;
        public string ResponseStatus { get; set; } = null!;
        public string CollectionMode { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public List<SurveyResponsePage> Pages { get; set; } = null!;
    }
}
