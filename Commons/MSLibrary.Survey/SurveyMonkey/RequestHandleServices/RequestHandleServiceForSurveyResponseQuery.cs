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
    [Injection(InterfaceType = typeof(RequestHandleServiceForSurveyResponseQuery), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForSurveyResponseQuery : ISurveyMonkeyRequestHandleService
    {
        private readonly IHttpClientFactoryWrapper _httpClientFactory;

        public RequestHandleServiceForSurveyResponseQuery(IHttpClientFactoryWrapper httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SurveyMonkeyResponse> Execute(Func<HttpClient, Task> authHandler, Func<SurveyMonkeyRequest, Task<SurveyMonkeyResponse>> requestHandler, string type, string configuration, SurveyMonkeyRequest request, CancellationToken cancellationToken = default)
        {
            var realRequest = (SurveyResponseQueryRequest)request;

            StringBuilder strQuery = new StringBuilder();
            if (realRequest.StartCreatedAt!=null)
            {
                strQuery.Append($"&start_created_at={realRequest.StartCreatedAt.ToString()}");
            }

            if (realRequest.EndCreatedAt != null)
            {
                strQuery.Append($"&end_created_at={realRequest.EndCreatedAt.ToString()}");
            }

            if (realRequest.Status!=null)
            {
                strQuery.Append($"&status={realRequest.Status.ToString()}");      
            }

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                await authHandler(httpClient);
                using (var response = await httpClient.GetAsync($"{realRequest.Address}/{realRequest.Version}/surveys/{realRequest.SurveyID.ToUrlEncode()}/responses/bulk?page={realRequest.Page.ToString()}&per_page={realRequest.PageSize.ToString()}{strQuery.ToString()}", cancellationToken))
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
                                 select new SurveyResponse()
                                 {
                                     AnalyzeUrl = item.AnalyzeUrl,
                                     CollectionMode = item.CollectionMode,
                                     CollectorID = item.CollectorID,
                                     CustomValue = item.CustomValue,
                                     CustomVariables = item.CustomVariables,
                                     DateCreated = item.DateCreated,
                                     DateModified = item.DateModified,
                                     EditUrl = item.EditUrl,
                                     Href = item.Href,
                                     ID = item.ID,
                                     IPAddress = item.IPAddress,
                                     RecipientID = item.RecipientID,
                                     ResponseStatus = item.ResponseStatus,
                                     SurveyID = item.SurveyID,
                                     TotalTime = item.TotalTime,
                                     Pages = (from item1 in item.Pages
                                              select new SurveyResponsePage()
                                              {
                                                  ID = item1.ID,
                                                  Questions = (from item2 in item1.Questions
                                                               select new SurveyResponseQuestion()
                                                               {
                                                                   ID = item2.ID,
                                                                   VariableID = item2.VariableID,
                                                                   Answers = (from item3 in item2.Answers
                                                                              select new SurveyResponseAnswer()
                                                                              {
                                                                                  ChoiceID = item3.ChoiceID,
                                                                                  ColID = item3.ColID,
                                                                                  ContentType = item3.ContentType,
                                                                                  DownloadUrl = item3.DownloadUrl,
                                                                                  OtherID = item3.OtherID,
                                                                                  RowID = item3.RowID,
                                                                                  Text = item3.Text
                                                                              }).ToList()
                                                               }).ToList()
                                              }).ToList()
                                 }).ToList();

                    return new SurveyResponseQueryResponse
                    {
                        PageSize = result.PageSize,
                        Total=result.Total,
                        Data= datas                               
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
            [DataMember(Name = "per_page")]
            public int PageSize { get; set; }
            [DataMember(Name = "total")]
            public int Total { get; set; }
            [DataMember(Name = "data")]
            public List<Data> Data { get; set; } = null!;
        }

        [DataContract]
        public class DataAnswer
        {
            [DataMember(Name = "choice_id")]
            public int ChoiceID { get; set; }
            [DataMember(Name = "row_id")]
            public int RowID { get; set; }
            [DataMember(Name = "col_id")]
            public int ColID { get; set; }
            [DataMember(Name = "other_id")]
            public int OtherID { get; set; }
            [DataMember(Name = "text")]
            public string? Text { get; set; }
            [DataMember(Name = "download_url")]
            public string DownloadUrl { get; set; } = null!;
            [DataMember(Name = "content_type")]
            public string ContentType { get; set; } = null!;
        }

        [DataContract]
        public class DataQuestion
        {
            [DataMember(Name = "id")]
            public int ID { get; set; }
            [DataMember(Name = "variable_id")]
            public int VariableID { get; set; }
            [DataMember(Name = "answers")]
            public List<SurveyResponseAnswer> Answers { get; set; } = null!;
        }

        [DataContract]
        public class Page
        {
            [DataMember(Name = "id")]
            public int ID { get; set; }
            [DataMember(Name = "questions")]
            public List<SurveyResponseQuestion> Questions { get; set; } = null!;
        }

        [DataContract]
        public class Data
        {
            [DataMember(Name = "id")]
            public string ID { get; set; } = null!;
            [DataMember(Name = "href")]
            public string Href { get; set; } = null!;
            [DataMember(Name = "survey_id")]
            public string SurveyID { get; set; } = null!;
            [DataMember(Name = "collector_id")]
            public string CollectorID { get; set; } = null!;
            [DataMember(Name = "recipient_id")]
            public string? RecipientID { get; set; }
            [DataMember(Name = "total_time")]
            public int TotalTime { get; set; }
            [DataMember(Name = "custom_value")]
            public string? CustomValue { get; set; }
            [DataMember(Name = "edit_url")]
            public string EditUrl { get; set; } = null!;
            [DataMember(Name = "analyze_url")]
            public string AnalyzeUrl { get; set; } = null!;
            [DataMember(Name = "ip_address")]
            public string IPAddress { get; set; } = null!;
            [DataMember(Name = "custom_variables")]
            public JObject CustomVariables { get; set; } = null!;
            [DataMember(Name = "response_status")]
            public string ResponseStatus { get; set; } = null!;
            [DataMember(Name = "collection_mode")]
            public string CollectionMode { get; set; } = null!;
            [DataMember(Name = "date_created")]
            public DateTime DateCreated { get; set; }
            [DataMember(Name = "date_modified")]
            public DateTime DateModified { get; set; }
            [DataMember(Name = "pages")]
            public List<Page> Pages { get; set; } = null!;
        }


    }


}
