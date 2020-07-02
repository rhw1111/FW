using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Oauth.ADFS
{
    public static class AdfsHelper
    {
        private static Func<IHttpClientFactory> _httpClientFactoryGenerator;

        public static Func<IHttpClientFactory> HttpClientFactoryGenerator
        {
            set
            {
                _httpClientFactoryGenerator = value;
            }
        }
        public static async Task<AdfsAuth> GetAdfsAuth(string adfsUri, string resource, string clientId, string redirectUri, string userName, string password)
        {
            var url = BuildCodeUrl(adfsUri, resource, clientId, redirectUri);
            var tokenUrl = BuildTokenUrl(adfsUri);
            var list = BuildCodeParams(userName, password);


            using (var httpClient = _httpClientFactoryGenerator().CreateClient())
            {

                //第1次请求
                using (var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(list)))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var strContent = await response.Content.ReadAsStringAsync();
                        var postData = await (new FormUrlEncodedContent(list)).ReadAsStringAsync();

                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AdfsOauthResponseError,
                            DefaultFormatting = "在Adfs的Oauth认证的响应出错，adfs的请求url为{0}，请求内容为{1},错误内容为{2}",
                            ReplaceParameters = new List<object>() { url, postData, strContent }
                        };

                        throw new UtilityException((int)Errors.AdfsOauthResponseError, fragment);
                    }
                    if (response.Headers.Location == null)
                    {
                        var strContent = await response.Content.ReadAsStringAsync();
                        var postData = await (new FormUrlEncodedContent(list)).ReadAsStringAsync();
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AdfsOauthResponseNotFoundParameterByName,
                            DefaultFormatting = "在Adfs的Oauth认证的响应中，找不到名称为{0}的参数，adfs的请求url为{1}，请求内容为{2}",
                            ReplaceParameters = new List<object>() { "location", url, postData }
                        };

                        throw new UtilityException((int)Errors.AdfsOauthResponseNotFoundParameterByName, fragment);
                    }

                    // 获取返回的Code
                    var query = response.Headers.Location.Query;
                    var col = QueryHelpers.ParseQuery(query);
                    var code = col["code"][0];
                    if (string.IsNullOrEmpty(code))
                    {
                        var strContent = await response.Content.ReadAsStringAsync();
                        var postData = await (new FormUrlEncodedContent(list)).ReadAsStringAsync();
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AdfsOauthResponseNotFoundParameterByName,
                            DefaultFormatting = "在Adfs的Oauth认证的响应中，找不到名称为{0}的参数，adfs的请求url为{1}，请求内容为{2}",
                            ReplaceParameters = new List<object>() { "code", url, postData }
                        };
                        throw new UtilityException((int)Errors.AdfsOauthResponseNotFoundParameterByName, fragment);
                    }
                    // 第2次请求 请求Token
                    var tokenParams = BuildTokenParams(clientId, code, resource, redirectUri);
                    using (var response2 = await httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(tokenParams)))
                    {
                        if (!response2.IsSuccessStatusCode)
                        {
                            var strContent = await response2.Content.ReadAsStringAsync();
                            var postData = await (new FormUrlEncodedContent(tokenParams)).ReadAsStringAsync();
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.AdfsOauthResponseError,
                                DefaultFormatting = "在Adfs的Oauth认证的响应出错，adfs的请求url为{0}，请求内容为{1},错误内容为{2}",
                                ReplaceParameters = new List<object>() { tokenUrl, postData, strContent }
                            };

                            throw new UtilityException((int)Errors.AdfsOauthResponseError, fragment);
                        }

                        var json = await response2.Content.ReadAsStringAsync();
                        var auth = JsonSerializerHelper.Deserialize<AdfsAuth>(json);
                        return auth;
                    }


                }
            }

        }

        public static async Task<AdfsAuth> RefreshToken(string adfsUri, string refresh_token)
        {
            var tokenUrl = BuildTokenUrl(adfsUri);
            using (var httpClient = _httpClientFactoryGenerator().CreateClient())
            {
                //请求Token
                var tokenParams = BuildRefreshTokenParams(refresh_token);
                using (var response = await httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(tokenParams)))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var strContent = await response.Content.ReadAsStringAsync();
                        var postData = await (new FormUrlEncodedContent(tokenParams)).ReadAsStringAsync();

                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AdfsOauthResponseError,
                            DefaultFormatting = "在Adfs的Oauth认证的响应出错，adfs的请求url为{0}，请求内容为{1},错误内容为{2}",
                            ReplaceParameters = new List<object>() { tokenUrl, postData, strContent }
                        };

                        throw new UtilityException((int)Errors.AdfsOauthResponseError, fragment);
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var auth = JsonSerializerHelper.Deserialize<AdfsAuth>(json);

                    return auth;
                }
            }
        }


        public static async Task<AdfsAuth> RefreshToken(string adfsUri, string clientid, string clientSecret, string refresh_token)
        {
            var tokenUrl = BuildTokenUrl(adfsUri);
            using (var httpClient = _httpClientFactoryGenerator().CreateClient())
            {
                //请求Token
                var tokenParams = BuildRefreshTokenParams(refresh_token, clientid, clientSecret);
                using (var response = await httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(tokenParams)))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var strContent = await response.Content.ReadAsStringAsync();
                        var postData = await (new FormUrlEncodedContent(tokenParams)).ReadAsStringAsync();

                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AdfsOauthResponseError,
                            DefaultFormatting = "在Adfs的Oauth认证的响应出错，adfs的请求url为{0}，请求内容为{1},错误内容为{2}",
                            ReplaceParameters = new List<object>() { tokenUrl, postData, strContent }
                        };

                        throw new UtilityException((int)Errors.AdfsOauthResponseError, fragment);
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var auth = JsonSerializerHelper.Deserialize<AdfsAuth>(json);

                    return auth;
                }
            }


        }

        public static async Task<AdfsAuth> GetAdfsAuthDirect(string adfsUri, string resource, string clientId, string clientSecret, string userName, string password)
        {
            var tokenUrl = BuildTokenUrl(adfsUri);
            var list = BuildTokenPasswordParams(clientId, clientSecret, resource, userName, password);

            using (var httpClient = _httpClientFactoryGenerator().CreateClient())
            {
                //第1次请求
                using (var response = await httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(list)))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var strContent = await response.Content.ReadAsStringAsync();
                        var postData = await (new FormUrlEncodedContent(list)).ReadAsStringAsync();

                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AdfsOauthResponseError,
                            DefaultFormatting = "在Adfs的Oauth认证的响应出错，adfs的请求url为{0}，请求内容为{1},错误内容为{2}",
                            ReplaceParameters = new List<object>() { tokenUrl, postData, strContent }
                        };

                        throw new UtilityException((int)Errors.AdfsOauthResponseError, fragment);
                    }


                    var json = await response.Content.ReadAsStringAsync();
                    var auth = JsonSerializerHelper.Deserialize<AdfsAuth>(json);
                    return auth;
                }
            }

        }

        public static async Task<IEnumerable<SecurityKey>> GetAdfsSigningKeys(string adfsUri)
        {
            IConfigurationManager<OpenIdConnectConfiguration> configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>($"{adfsUri}adfs/.well-known/openid-configuration",
                 new OpenIdConnectConfigurationRetriever());
            OpenIdConnectConfiguration openIdConfig = await configurationManager.GetConfigurationAsync(CancellationToken.None);
            return openIdConfig.SigningKeys;
        }

        private static List<KeyValuePair<string, string>> BuildRefreshTokenParams(string refresh_token)
        {
            var list = new List<KeyValuePair<string, string>>
             {
                 new KeyValuePair<string, string>("grant_type", "refresh_token"),
                 new KeyValuePair<string, string>("refresh_token", refresh_token),
             };
            return list;
        }

        private static List<KeyValuePair<string, string>> BuildRefreshTokenParams(string clientId, string clientSecret, string refresh_token)
        {
            var list = new List<KeyValuePair<string, string>>
             {
                 new KeyValuePair<string, string>("grant_type", "refresh_token"),
                 new KeyValuePair<string, string>("refresh_token", refresh_token),
                 new KeyValuePair<string, string>("client_id", clientId),
                 new KeyValuePair<string, string>("client_secret", clientSecret),
             };
            return list;
        }


        private static string BuildTokenUrl(string adfsUri)
        {
            return $"{adfsUri}adfs/oauth2/token";
        }

        private static string BuildCodeUrl(string adfsUri, string resource, string clientId, string redirectUri)
        {

            if (!adfsUri.EndsWith("/"))
                adfsUri = adfsUri + "/";
            var url = $"{adfsUri}adfs/oauth2/authorize";

            var response_type = "code";

            var sb = new StringBuilder();
            sb.Append(url);
            sb.Append("?");
            sb.Append($"response_type={response_type}");
            sb.Append("&");
            sb.Append($"client_id={clientId}");
            sb.Append("&");
            sb.Append($"redirect_uri={redirectUri.ToUrlEncode()}");
            sb.Append("&");
            sb.Append($"resource={resource.ToUrlEncode()}");
            url = sb.ToString();
            return url;
        }

        private static List<KeyValuePair<string, string>> BuildCodeParams(string userName, string password)
        {
            var list = new List<KeyValuePair<string, string>>()
             {
                 new KeyValuePair<string, string>("UserName",userName),
                 new KeyValuePair<string, string>("Password",password),
                 new KeyValuePair<string, string>("Kmsi","true"),
                 new KeyValuePair<string, string>("AuthMethod","FormsAuthentication")
             };
            return list;
        }



        private static List<KeyValuePair<string, string>> BuildTokenParams(string clientId, string code, string resource, string redirectUri)
        {
            var list = new List<KeyValuePair<string, string>>
             {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                 new KeyValuePair<string, string>("client_id", clientId),
                 new KeyValuePair<string, string>("code", code),
                 new KeyValuePair<string, string>("resource", resource),
                 new KeyValuePair<string, string>("redirect_uri", redirectUri)
             };
            return list;
        }


        private static List<KeyValuePair<string, string>> BuildTokenPasswordParams(string clientId, string clientSecret, string resource, string username, string password)
        {
            var list = new List<KeyValuePair<string, string>>
             {
                new KeyValuePair<string, string>("grant_type", "password"),
                 new KeyValuePair<string, string>("client_id", clientId),
                 new KeyValuePair<string, string>("client_secret", clientSecret),
                 new KeyValuePair<string, string>("resource", resource),
                 new KeyValuePair<string, string>("username", username),
                 new KeyValuePair<string, string>("password", password),
            };
            return list;
        }


    }
}
