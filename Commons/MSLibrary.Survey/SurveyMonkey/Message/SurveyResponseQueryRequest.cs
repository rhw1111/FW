using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class SurveyResponseQueryRequest : SurveyMonkeyRequest
    {
        public SurveyResponseQueryRequest():base(SurveyMonkeyRequestTypes.SurveyResponseQuery)
        {

        }
        public string SurveyID { get; set; } = null!;

        public int Page { get; set; }
        public int PageSize { get; set; }

        public DateTime? StartCreatedAt { get; set; }
        public DateTime? EndCreatedAt { get; set; }
        public string? Status { get; set; }

    }
}
