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
    [Injection(InterfaceType = typeof(RequestHandleServiceForSurveyResponseQuerySingle), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForSurveyResponseQuerySingle : ISurveyMonkeyRequestHandleService
    {
        private readonly IHttpClientFactoryWrapper _httpClientFactory;

        public RequestHandleServiceForSurveyResponseQuerySingle(IHttpClientFactoryWrapper httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<SurveyMonkeyResponse> Execute(Func<HttpClient, Task> authHandler, Func<SurveyMonkeyRequest, Task<SurveyMonkeyResponse>> requestHandler, string type, string configuration, SurveyMonkeyRequest request, CancellationToken cancellationToken = default)
        {
            var realRequest = (SurveyResponseQuerySingleRequest)request;

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                await authHandler(httpClient);
                using (var response = await httpClient.GetAsync($"{realRequest.Address}/{realRequest.Version}/surveys/{realRequest.SurveyID.ToUrlEncode()}/responses/{realRequest.ResponseID.ToUrlEncode()}", cancellationToken))
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


                    return new SurveyResponseQuerySingleResponse
                    {
                        ID = result.ID,
                        TotalTime = result.TotalTime,
                        SurveyID = result.SurveyID,
                        CollectorID = result.CollectorID,
                        CollectionMode = result.CollectionMode,
                        CustomValue = result.CustomValue,
                        CustomVariables = result.CustomVariables,
                        IPAddress = result.IPAddress,
                        RecipientID = result.RecipientID,
                        PagePath = result.PagePath,
                        LogicPath = result.LogicPath,
                        Metadata = result.Metadata,
                        ResponseStatus = result.ResponseStatus,
                        AnalyzeUrl = result.AnalyzeUrl,
                        EditUrl = result.EditUrl,
                        DateCreated = result.DateCreated,
                        DateModified = result.DateModified
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
            [DataMember(Name ="id")]
            public string ID { get; set; } = null!;
            [DataMember(Name = "total_time")]
            public int TotalTime { get; set; }
            [DataMember(Name = "ip_address")]
            public string IPAddress { get; set; } = null!;
            [DataMember(Name = "recipient_id")]
            public string RecipientID { get; set; } = null!;
            [DataMember(Name = "logic_path")]
            public JObject LogicPath { get; set; } = null!;
            [DataMember(Name = "metadata")]
            public JObject Metadata { get; set; } = null!;
            [DataMember(Name = "date_created")]
            public DateTime DateCreated { get; set; }

            [DataMember(Name = "date_modified")]
            public DateTime DateModified { get; set; }
            [DataMember(Name = "response_status")]
            public string ResponseStatus { get; set; } = null!;
            [DataMember(Name = "custom_variables")]
            public JObject CustomVariables { get; set; } = null!;
            [DataMember(Name = "custom_value")]
            public string CustomValue { get; set; } = null!;
            [DataMember(Name = "edit_url")]
            public string EditUrl { get; set; } = null!;
            [DataMember(Name = "analyze_url")]
            public string AnalyzeUrl { get; set; } = null!;
            [DataMember(Name = "page_path")]
            public List<string> PagePath { get; set; } = null!;
            [DataMember(Name = "collector_id")]
            public string CollectorID { get; set; } = null!;
            [DataMember(Name = "survey_id")]
            public string SurveyID { get; set; } = null!;
            [DataMember(Name = "collection_mode")]
            public string CollectionMode { get; set; } = null!;
        }
    }
}
