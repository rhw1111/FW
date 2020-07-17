using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class SurveyMessageQueryResponse:SurveyMonkeyResponse
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }

        public List<SurveyMessageItem> SurveyMessageItems
        {
            get; set;
        } = null!;
    }

    public class SurveyMessageItem
    {
        public string ID { get; set; } = null!;
        public string Type { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string Href { get; set; } = null!;
    }
}
