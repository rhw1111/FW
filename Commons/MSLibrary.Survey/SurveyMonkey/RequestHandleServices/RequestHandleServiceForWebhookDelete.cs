using System;
using System.Collections.Generic;
using System.Net;
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
    [Injection(InterfaceType = typeof(RequestHandleServiceForWebhookDelete), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForWebhookDelete : ISurveyMonkeyRequestHandleService
    {
        private readonly IHttpClientFactoryWrapper _httpClientFactory;

        public RequestHandleServiceForWebhookDelete(IHttpClientFactoryWrapper httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SurveyMonkeyResponse> Execute(Func<HttpClient, Task> authHandler, Func<SurveyMonkeyRequest, Task<SurveyMonkeyResponse>> requestHandler, string type, string configuration, SurveyMonkeyRequest request, CancellationToken cancellationToken = default)
        {
            var realRequest = (WebhookDeleteRequest)request;

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                await authHandler(httpClient);




                using (var response = await httpClient.DeleteAsync($"{realRequest.Address}/{realRequest.Version}/webhooks/{realRequest.ID.ToUrlEncode()}",cancellationToken))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        TextFragment fragment;
                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            fragment = new TextFragment()
                            {
                                Code = SurveyTextCodes.NotFoundSurveyMonkeyWebhookByID,
                                DefaultFormatting = "找不到指定ID为{0}的Webhook注册",
                                ReplaceParameters = new List<object>() { realRequest.ID }
                            };

                            throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyMonkeyWebhookByID, fragment, 1, 0);
                        }


                         fragment = new TextFragment()
                        {
                            Code = SurveyTextCodes.SurveyMonkeyRequestHandleError,
                            DefaultFormatting = "SurveyMonkey终结点{0}请求处理错误，错误信息为{1}",
                            ReplaceParameters = new List<object>() { realRequest.Address, getResponseError(response) }
                        };

                        throw new UtilityException((int)SurveyErrorCodes.SurveyMonkeyRequestHandleError, fragment, 1, 0);
                    }

                    return new WebhookDeleteResponse
                    {
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
    }
}
