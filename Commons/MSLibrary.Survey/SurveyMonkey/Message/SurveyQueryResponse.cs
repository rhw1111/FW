using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class SurveyQueryResponse: SurveyMonkeyResponse
    {
        public int Page { get; set; }
        public int PageSzie { get; set; }
        public int Total { get; set; }

        public List<SurveyQueryItem> SurveyItems
        {
            get; set;
        } = null!;
    }

    public class SurveyQueryItem
    {
        public string ID { get; set; } = null!;
        public string Title { get; set; } = null!;

        public string NickName { get; set; } = null!;

        public string Href { get; set; } = null!;
    }
}
