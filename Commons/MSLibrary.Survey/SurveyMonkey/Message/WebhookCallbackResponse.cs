using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class WebhookCallbackResponse
    {
        public string Name { get; set; } = null!;
        public string FilterType { get; set; } = null!;
        public string FilterID { get; set; } = null!;
        public string EventType { get; set; } = null!;
        public string EventID { get; set; } = null!;
        public string ObjectType { get; set; } = null!;
        public string ObjectID { get; set; } = null!;
        public DateTime EventDatetime { get; set; }
        public JObject Resources { get; set; } = null!;
    }
}
