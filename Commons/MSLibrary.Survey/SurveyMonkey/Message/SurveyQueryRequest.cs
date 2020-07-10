using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class SurveyQueryRequest: SurveyMonkeyRequest
    {
        public SurveyQueryRequest() : base(SurveyMonkeyRequestTypes.SurveyQuery)
        {
        }

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
