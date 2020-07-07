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
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using MSLibrary.Survey.SurveyMonkey.Message;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForWebhookQuerySingle), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForWebhookQuerySingle : ISurveyMonkeyRequestHandleService
    {
        private readonly IHttpClientFactoryWrapper _httpClientFactory;

        public RequestHandleServiceForWebhookQuerySingle(IHttpClientFactoryWrapper httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SurveyMonkeyResponse> Execute(Func<HttpClient, Task> authHandler, Func<SurveyMonkeyRequest, Task<SurveyMonkeyResponse>> requestHandler, string type, string configuration, SurveyMonkeyRequest request, CancellationToken cancellationToken = default)
        {
            var realRequest = (WebhookQuerySingleRequest)request;

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                await authHandler(httpClient);

                using (var response = await httpClient.GetAsync($"{realRequest.Address}/{realRequest.Version}/webhooks/{realRequest.ID.ToUrlEncode()}", cancellationToken))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = SurveyTextCodes.SurveyMonkeyRequestHandleError,
                            DefaultFormatting = "SurveyMonkey终结点{0}请求处理错误，错误信息为{1}",
                            ReplaceParameters = new List<object>() { realRequest.Address, getResponseError(response) }
                        };

                        throw new UtilityException((int)SurveyErrorCodes.SurveyMonkeyRequestHandleError, fragment, 1, 0);
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializerHelper.Deserialize<Register>(json);

                    return new WebhookQuerySingleResponse
                    {
                        ID = result.ID,
                        Name = result.Name,
                        EventType = result.EventType,
                        Href = result.Href,
                        ObjectType = result.ObjectType,
                        ObjectIds = result.ObjectIds,
                        SubscriptionUrl = result.SubscriptionUrl
                    };
                }

            }
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
