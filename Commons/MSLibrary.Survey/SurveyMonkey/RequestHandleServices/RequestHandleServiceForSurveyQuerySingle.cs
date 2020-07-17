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
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using MSLibrary.Survey.SurveyMonkey.Message;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForSurveyQuerySingle), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForSurveyQuerySingle : ISurveyMonkeyRequestHandleService
    {
        private readonly IHttpClientFactoryWrapper _httpClientFactory;

        public RequestHandleServiceForSurveyQuerySingle(IHttpClientFactoryWrapper httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SurveyMonkeyResponse> Execute(Func<HttpClient, Task> authHandler, Func<SurveyMonkeyRequest, Task<SurveyMonkeyResponse>> requestHandler, string type, string configuration, SurveyMonkeyRequest request, CancellationToken cancellationToken = default)
        {
            var realRequest = (SurveyQuerySingleRequest)request;

            StringBuilder strQuery = new StringBuilder();

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                await authHandler(httpClient);
                using (var response = await httpClient.GetAsync($"{realRequest.Address}/{realRequest.Version}/surveys/{realRequest.ID.ToUrlEncode()}", cancellationToken))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        TextFragment fragment;
                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            fragment = new TextFragment()
                            {
                                Code = SurveyTextCodes.NotFoundSurveyMonkeySurveyByID,
                                DefaultFormatting = "找不到ID为{0}的SurveyMonkey的Survey",
                                ReplaceParameters = new List<object>() { realRequest.ID }
                            };

                            throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyMonkeySurveyByID, fragment, 1, 0);
                        }

                        fragment = new TextFragment()
                        {
                            Code = SurveyTextCodes.SurveyMonkeyRequestHandleError,
                            DefaultFormatting = "SurveyMonkey终结点{0}请求处理错误，错误信息为{1}",
                            ReplaceParameters = new List<object>() { realRequest.Address, getResponseError(response) }
                        };

                        throw new UtilityException((int)SurveyErrorCodes.SurveyMonkeyRequestHandleError, fragment, 1, 0);
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializerHelper.Deserialize<Response>(json);



                    return new SurveyQuerySingleResponse
                    {
                        ID = result.ID,
                        Title = result.Title,
                        NickName = result.NickName,
                        Href = result.Href
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
        public class Response
        {
            [DataMember(Name = "id")]
            public string ID { get; set; } = null!;

            [DataMember(Name = "title")]
            public string Title { get; set; } = null!;
            [DataMember(Name = "nickname")]
            public string NickName { get; set; } = null!;

            [DataMember(Name = "href")]
            public string Href { get; set; } = null!;
        }
    }
}
