using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class WebhookCallbackRequest:SurveyMonkeyRequest
    {
        public string SmApikey { get; set; } = null!;
        public string SmSignature { get; set; } = null!;

        public string Body { get; set; } = null!;
    }
}
