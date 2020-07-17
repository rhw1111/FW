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
    [Injection(InterfaceType = typeof(RequestHandleServiceForSurveyQuery), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForSurveyQuery : ISurveyMonkeyRequestHandleService
    {
        private readonly IHttpClientFactoryWrapper _httpClientFactory;

        public RequestHandleServiceForSurveyQuery(IHttpClientFactoryWrapper httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SurveyMonkeyResponse> Execute(Func<HttpClient, Task> authHandler, Func<SurveyMonkeyRequest, Task<SurveyMonkeyResponse>> requestHandler, string type, string configuration, SurveyMonkeyRequest request, CancellationToken cancellationToken = default)
        {
            var realRequest = (SurveyQueryRequest)request;

            StringBuilder strQuery = new StringBuilder();

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                await authHandler(httpClient);
                using (var response = await httpClient.GetAsync($"{realRequest.Address}/{realRequest.Version}/surveys?page={realRequest.Page.ToString()}&per_page={realRequest.PageSize.ToString()}{strQuery.ToString()}", cancellationToken))
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
                    var result = JsonSerializerHelper.Deserialize<Response>(json);

                    var datas = (from item in result.Data
                                 select new SurveyQueryItem()
                                 {
                                     Title = item.Title,
                                     NickName = item.NickName,
                                     Href = item.Href,
                                     ID = item.ID
                                 }).ToList();

                    return new SurveyQueryResponse
                    {
                        PageSize = result.PageSize,
                        Total = result.Total,
                        Page = result.Page,
                        SurveyItems = datas
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
        private class Response
        {
            [DataMember(Name = "page")]
            public int Page { get; set; }
            [DataMember(Name = "per_page")]
            public int PageSize { get; set; }
            [DataMember(Name = "total")]
            public int Total { get; set; }
            [DataMember(Name = "data")]
            public List<DataSurvey> Data { get; set; } = null!;
        }

        [DataContract]
        private class DataSurvey
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
