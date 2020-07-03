using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class WebhookRegisterRequest:SurveyMonkeyRequest
    {
        public WebhookRegisterRequest()
        {
            Type = SurveyMonkeyRequestTypes.WebhookRegister;
        }

        public string Name
        {
            get; set;
        } = null!;

        public string EventType
        {
            get; set;
        } = null!;

        public string? ObjectType
        {
            get;set;
        }

        public List<string>? ObjectIds
        {
            get;set;
        }

        public string SubscriptionUrl
        {
            get; set;
        } = null!;
    }
}
