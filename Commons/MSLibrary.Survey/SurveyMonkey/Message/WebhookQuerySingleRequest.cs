using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class WebhookQuerySingleRequest:SurveyMonkeyRequest
    {
        public WebhookQuerySingleRequest() : base(SurveyMonkeyRequestTypes.WebhookQuerySingle)
        {

        }

        public string ID { get; set; } = null!;
    }
}
