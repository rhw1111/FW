using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class WebhookQuerySingleResponse: SurveyMonkeyResponse
    {
        public string ID
        {
            get; set;
        } = null!;
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
            get; set;
        }

        public List<string>? ObjectIds
        {
            get; set;
        }

        public string SubscriptionUrl
        {
            get; set;
        } = null!;
        public string Href
        {
            get; set;
        } = null!;
    }
}
