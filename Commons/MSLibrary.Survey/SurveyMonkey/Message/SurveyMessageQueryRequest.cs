using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class SurveyMessageQueryRequest:SurveyMonkeyRequest
    {
        public SurveyMessageQueryRequest() : base(SurveyMonkeyRequestTypes.SurveyMessageQuery)
        {
        }

        public string CollectorID
        {
            get; set;
        } = null!;

        public int Page
        {
            get; set;
        } = 1;

        public int PageSize
        {
            get; set;
        } = 1000;
    }
}
