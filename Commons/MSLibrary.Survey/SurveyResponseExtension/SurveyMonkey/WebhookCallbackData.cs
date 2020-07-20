using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Survey.SurveyResponseExtension.SurveyMonkey
{
    [DataContract]
    public class WebhookCallbackData
    {
        [DataMember(Name = "resources")]
        public WebhookCallbackResources Resources { get; set; } = null!;
    }

    [DataContract]
    public class WebhookCallbackResources
    {
        [DataMember(Name = "respondent_id")]
        public string? RespondentID { get; set; }
        [DataMember(Name = "recipient_id")]
        public string? RecipientID { get; set; }
        [DataMember(Name = "collector_id")]
        public string? CollectorID { get; set; }
        [DataMember(Name = "survey_id")]
        public string? SurveyID { get; set; } 
        [DataMember(Name = "user_id")]
        public string? UserID { get; set; }
    }
}
