using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class WebhookDeleteRequest : SurveyMonkeyRequest
    {
        public string ID { get; set; } = null!;
    }
}
