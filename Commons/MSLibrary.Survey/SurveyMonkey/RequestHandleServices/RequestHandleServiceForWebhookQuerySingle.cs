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
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using MSLibrary.Survey.SurveyMonkey.Message;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForWebhookQuerySingle), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForWebhookQuerySingle : ISurveyMonkeyRequestHandleService
    {
        public async Task<SurveyMonkeyResponse> Execute(Func<HttpClient, Task> authHandler, SurveyMonkeyRequest request, CancellationToken cancellationToken = default)
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
        private class Register
        {
            [DataMember(Name="id")]
            public string ID
            {
                get; set;
            } = null!;

            [DataMember(Name = "name")]
            public string Name
            {
                get; set;
            } = null!;

            [DataMember(Name = "event_type")]
            public string EventType
            {
                get; set;
            } = null!;

            [DataMember(Name = "object_type")]
            public string? ObjectType
            {
                get; set;
            }

            [DataMember(Name = "object_ids")]
            public List<string>? ObjectIds
            {
                get; set;
            }

            [DataMember(Name = "subscription_url")]
            public string SubscriptionUrl
            {
                get; set;
            } = null!;

            [DataMember(Name = "href")]
            public string Href
            {
                get; set;
            } = null!;
        }
    }
}
