using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;

namespace MSLibrary.Oauth.AAD
{
    public static class AADHelper
    {
        public static async Task<AADAuth> GetPasswordAuth(string baseUri, string tenant, string resource, string clientId, string userName, string password)
        {
            var tokenUrl = BuildUrl(baseUri,tenant);
            var list = BuildTokenPasswordParams(clientId, resource, userName, password);

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(list)))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var strContent = await response.Content.ReadAsStringAsync();
                        var postData = await (new FormUrlEncodedContent(list)).ReadAsStringAsync();

                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AADOauthResponseError,
                            DefaultFormatting = "在AAD的Oauth认证的响应出错，AAD的请求url为{0}，请求内容为{1},错误内容为{2}",
                            ReplaceParameters = new List<object>() { tokenUrl, postData, strContent }
                        };

                        throw new UtilityException((int)Errors.AADOauthResponseError, fragment);
                    }


                    var json = await response.Content.ReadAsStringAsync();
                    var auth = JsonSerializerHelper.Deserialize<AADAuth>(json);
                    return auth;
                }
            }
        }

        public static async Task<AADAuth> GetClientAuth(string baseUri,string tenant, string resource, string clientId, string clientSecret)
        {
            var tokenUrl = BuildUrl(baseUri,tenant);
            var list = BuildTokenClientParams(clientId, resource, clientSecret);

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(list)))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var strContent = await response.Content.ReadAsStringAsync();
                        var postData = await (new FormUrlEncodedContent(list)).ReadAsStringAsync();

                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AADOauthResponseError,
                            DefaultFormatting = "在AAD的Oauth认证的响应出错，AAD的请求url为{0}，请求内容为{1},错误内容为{2}",
                            ReplaceParameters = new List<object>() { tokenUrl, postData, strContent }
                        };

                        throw new UtilityException((int)Errors.AADOauthResponseError, fragment);
                    }


                    var json = await response.Content.ReadAsStringAsync();
                    var auth = JsonSerializerHelper.Deserialize<AADAuth>(json);
                    return auth;
                }
            }
        }


        private static string BuildUrl(string baseUri,string tenant)
        {
            var url = $"https://{baseUri}/{tenant}/oauth2/v2.0/authorize";
            return url;
        }


        private static List<KeyValuePair<string, string>> BuildTokenPasswordParams(string resource, string clientId, string userName, string password)
        {
            var list = new List<KeyValuePair<string, string>>
             {
                new KeyValuePair<string, string>("grant_type", "password"),
                 new KeyValuePair<string, string>("client_id", clientId),
                 new KeyValuePair<string, string>("scope", $"{resource} offline_access"),
                 new KeyValuePair<string, string>("username", userName),
                 new KeyValuePair<string, string>("password", password),
            };
            return list;
        }

        private static List<KeyValuePair<string, string>> BuildTokenClientParams(string resource, string clientId, string clientSecret)
        {
            var list = new List<KeyValuePair<string, string>>
             {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                 new KeyValuePair<string, string>("client_id", clientId),
                 new KeyValuePair<string, string>("scope", resource),
                 new KeyValuePair<string, string>("client_secret", clientSecret),
            };
            return list;
        }

    }
}
