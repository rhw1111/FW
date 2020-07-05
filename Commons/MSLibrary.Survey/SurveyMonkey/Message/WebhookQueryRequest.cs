using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class WebhookQueryRequest:SurveyMonkeyRequest
    {
        public WebhookQueryRequest() : base(SurveyMonkeyRequestTypes.WebhookQuery)
        {

        }

        public int Page
        {
            get; set;
        } = 1;

        public int PageSize
        {
            get; set;
        } = 50;

    }
}
