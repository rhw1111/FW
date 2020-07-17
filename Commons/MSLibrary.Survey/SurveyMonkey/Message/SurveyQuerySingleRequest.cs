using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class SurveyQuerySingleRequest: SurveyMonkeyRequest
    {
        public SurveyQuerySingleRequest() : base(SurveyMonkeyRequestTypes.SurveyQuerySingle)
        {
        }
        public string ID { get; set; } = null!;
    }
}
