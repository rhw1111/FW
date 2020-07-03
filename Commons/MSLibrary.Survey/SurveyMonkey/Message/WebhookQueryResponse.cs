using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey.Message
{
    public class WebhookQueryResponse: SurveyMonkeyResponse
    {
        public List<WebhookRegisterItem> RegisterItems
        {
            get; set;
        } = null!;
    }


    public class WebhookRegisterItem
    {
        public string ID { get; set; } = null!;
        public string Name { get; set; } = null!;

        public string Href { get; set; } = null!;
    }
}
