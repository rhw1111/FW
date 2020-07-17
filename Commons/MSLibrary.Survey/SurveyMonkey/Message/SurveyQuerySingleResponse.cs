using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class SurveyQuerySingleResponse: SurveyMonkeyResponse
    {
        public string ID { get; set; } = null!;
        public string Title { get; set; } = null!;

        public string NickName { get; set; } = null!;

        public string Href { get; set; } = null!;
    }
}
