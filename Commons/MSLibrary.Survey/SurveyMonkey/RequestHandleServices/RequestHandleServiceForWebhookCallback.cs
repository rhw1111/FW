using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using MSLibrary.Survey.SurveyMonkey.Message;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForWebhookCallback), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForWebhookCallback : ISurveyMonkeyRequestHandleService
    {
        public RequestHandleServiceForWebhookCallback()
        {

        }

        public Task<SurveyMonkeyResponse> Execute(Func<HttpClient, Task> authHandler, string type, string configuration, SurveyMonkeyRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private async Task<string> getResponseError(HttpResponseMessage response)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = response.ReasonPhrase;
            }
            return errorMessage;
        }


        [DataContract]
        private class Response
        {
            [DataMember(Name= "name")]
            public string Name { get; set; } = null!;
            [DataMember(Name = "filter_type")]
            public string FilterType { get; set; } = null!;
            [DataMember(Name = "filter_id")]
            public string FilterID { get; set; } = null!;
            [DataMember(Name = "event_type")]
            public string EventType { get; set; } = null!;
            [DataMember(Name = "event_id")]
            public string EventID { get; set; } = null!;
            [DataMember(Name = "object_type")]
            public string ObjectType { get; set; } = null!;
            [DataMember(Name = "object_id")]
            public string ObjectID { get; set; } = null!;
            [DataMember(Name = "event_datetime")]
            public DateTime EventDatetime { get; set; }
            [DataMember(Name = "resources")]
            public JObject Resources { get; set; } = null!;
        }
    }
}
