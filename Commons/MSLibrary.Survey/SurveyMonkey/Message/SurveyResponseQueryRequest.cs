using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class SurveyResponseQueryRequest: SurveyMonkeyRequest
    {
        public string SurveyID { get; set; } = null!;
        public string ResponseID { get; set; } = null!;
    }
}
