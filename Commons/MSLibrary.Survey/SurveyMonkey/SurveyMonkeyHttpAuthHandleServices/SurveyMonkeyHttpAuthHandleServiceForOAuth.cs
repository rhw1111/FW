using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Collections;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using Quartz.Impl.Triggers;

namespace MSLibrary.Survey.SurveyMonkey.SurveyMonkeyHttpAuthHandleServices
{
    /// <summary>
    /// 基于OAuth的Http鉴权
    /// </summary>
    [Injection(InterfaceType = typeof(SurveyMonkeyHttpAuthHandleServiceForOAuth), Scope = InjectionScope.Singleton)]
    public class SurveyMonkeyHttpAuthHandleServiceForOAuth : ISurveyMonkeyHttpAuthHandleService
    {
        private readonly static IDictionary<string, SharePool<AccessTokenContainer>> _accessTokenContainers = new Dictionary<string, SharePool<AccessTokenContainer>>();

        private readonly IHttpClientFactory _httpClientFactory;

        public SurveyMonkeyHttpAuthHandleServiceForOAuth(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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

        private async Task<string> getAccessToken(string address, string authConfiguration, CancellationToken cancellationToken = default)
        {
            var configuration = JsonSerializerHelper.Deserialize<AuthConfiguration>(authConfiguration);

            using (var httpClient=_httpClientFactory.CreateClient())
            {

                var response=await httpClient.GetAsync($"{address}/oauth/authorize?response_type={configuration.ResponseType.ToUrlEncode()}&client_id={configuration.ClientID.ToUrlEncode()}&redirect_uri={configuration.RedirectUri.ToUrlEncode()}", cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var fragment = new TextFragment()
                    {
                        Code = SurveyTextCodes.SurveyMonkeyAuthError,
                        DefaultFormatting = "SurveyMonkey终结点{0}鉴权错误，错误信息为{1}”",
                        ReplaceParameters = new List<object>() { address, getResponseError (response) }
                    };

                    throw new UtilityException((int)SurveyErrorCodes.SurveyMonkeyAuthError, fragment, 1, 0);
                }


                if (response.Headers.Location == null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = SurveyTextCodes.SurveyMonkeyAuthError,
                        DefaultFormatting = "SurveyMonkey终结点{0}鉴权错误，错误信息为{1}”",
                        ReplaceParameters = new List<object>() { address, "Not Found Redirect Location in Response" }
                    };

                    throw new UtilityException((int)SurveyErrorCodes.SurveyMonkeyAuthError, fragment, 1, 0);
                }

              
                var query = response.Headers.Location.Query;
                var col = QueryHelpers.ParseQuery(query);
            
                if (!col.TryGetValue("code", out StringValues codeValue))
                {
                    string error = string.Empty;
                    string errorDescription = string.Empty;

                    if (col.TryGetValue("error", out StringValues errorValue))
                    {
                        error = errorValue[0];
                    }

                    if (col.TryGetValue("error_description", out StringValues errorDescriptionValue))
                    {
                        errorDescription = errorDescriptionValue[0];
                    }

                    var fragment = new TextFragment()
                    {
                        Code = SurveyTextCodes.SurveyMonkeyAuthError,
                        DefaultFormatting = "SurveyMonkey终结点{0}鉴权错误，错误信息为{1}”",
                        ReplaceParameters = new List<object>() { address, $"Error:{error},ErrorDescription:{errorDescription}" }
                    };

                    throw new UtilityException((int)SurveyErrorCodes.SurveyMonkeyAuthError, fragment, 1, 0);
                }

                Dictionary<string, string> paramerets = new Dictionary<string, string>()
                {
                    { "client_secret",configuration.ClientSecret},
                    { "code",codeValue[0]},
                    {"redirect_uri",configuration.RedirectUri },
                    { "client_id",configuration.ClientID},
                    { "grant_type","authorization_code"}
                };
                using (var response2 = await httpClient.PostAsync($"{address}/oauth/token", new FormUrlEncodedContent(paramerets)))
                {
                    if (!response2.IsSuccessStatusCode)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = SurveyTextCodes.SurveyMonkeyAuthError,
                            DefaultFormatting = "SurveyMonkey终结点{0}鉴权错误，错误信息为{1}”",
                            ReplaceParameters = new List<object>() { address, getResponseError(response) }
                        };

                        throw new UtilityException((int)SurveyErrorCodes.SurveyMonkeyAuthError, fragment, 1, 0);
                    }

                    var json = await response2.Content.ReadAsStringAsync();
                    var authResult = JsonSerializerHelper.Deserialize<AuthResult>(json);
                    return authResult.AccessToken;
                }
            }
        }

        public async Task Handle(HttpClient httpClient, string address, string authConfiguration, CancellationToken cancellationToken = default)
        {
            var key = $"{address}_{authConfiguration}";
            if (!_accessTokenContainers.TryGetValue(key,out SharePool<AccessTokenContainer>? pool))
            {
                lock (_accessTokenContainers)
                {
                    if (!_accessTokenContainers.TryGetValue(key, out pool))
                    {
                        pool = new SharePool<AccessTokenContainer>(
                            "SurveyMonkeyHttpAuthHandleServiceForOAuth",
                            null
                            ,
                            null
                            ,
                            null
                            ,
                            async () =>
                            {
                                var accesstoken = await getAccessToken(address, authConfiguration, cancellationToken);
                                return new AccessTokenContainer { AccessToken = accesstoken, CreateTime = DateTime.UtcNow };
                            }
                            ,
                            async (container) =>
                            {
                                return await Task.FromResult(true);
                            }
                            ,
                            async (container) =>
                            {
                                await Task.CompletedTask;
                            }
                            ,
                            5
                            );

                        _accessTokenContainers[key] = pool;
                    }
                }
            }

            var accessTokenContainer=await pool.GetAsync();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessTokenContainer.AccessToken}");   
        }

        private class AccessTokenContainer
        {
            public string AccessToken { get; set; } = null!;
            public DateTime CreateTime { get; set; }
        }

        [DataContract]
        private class AuthConfiguration
        {
            [DataMember]
            public string ResponseType { get; set; } = null!;
            [DataMember]
            public string ClientID { get; set; } = null!;
            [DataMember]
            public string ClientSecret { get; set; } = null!;
            [DataMember]
            public string RedirectUri { get; set; } = null!;
        }

        [DataContract]
        private class AuthResult
        {
            [DataMember(Name = "access_token")]
            public string AccessToken
            {
                get; set;
            } = null!;
        }
    }
}
