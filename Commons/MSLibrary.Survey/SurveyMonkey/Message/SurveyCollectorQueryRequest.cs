using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class SurveyCollectorQueryRequest:SurveyMonkeyRequest
    {
        public SurveyCollectorQueryRequest() : base(SurveyMonkeyRequestTypes.SurveyCollectorQuery)
        {
        }

        public string SurveyID
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
