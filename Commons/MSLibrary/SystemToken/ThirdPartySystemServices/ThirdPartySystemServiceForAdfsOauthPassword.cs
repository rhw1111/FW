using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.Security;

namespace MSLibrary.SystemToken.ThirdPartySystemServices
{
    /// <summary>
    /// 基于AdfsOauth的Password模式的第三方系统服务
    /// configuration格式为
    /// {
    /// "AdfsUrl":"adfs地址"
    /// "Scope"："资源",
    /// "ClientId":"应用程序Id",
    /// "Timeout":"令牌失效时间（与最后刷新时间比，单位秒）"
    /// }
    /// 获取的键值对为
    /// upn
    /// </summary>
    [Injection(InterfaceType = typeof(ThirdPartySystemServiceForAdfsOauthPassword), Scope = InjectionScope.Singleton)]
    public class ThirdPartySystemServiceForAdfsOauthPassword : IThirdPartySystemService
    {
        private IHttpClientFactoryWrapper _httpClinetFactory;
        private ISecurityService _securityService;

        public ThirdPartySystemServiceForAdfsOauthPassword(IHttpClientFactoryWrapper httpClinetFactory, ISecurityService securityService)
        {
            _httpClinetFactory = httpClinetFactory;
            _securityService = securityService;
        }

        public async Task<string> GetCommunicationToken(string configurationInfo, string systemToken)
        {
            //令牌格式为AdfsOauthCodePostResponse的json序列化字符串，
            //因此需要反序列化为AdfsOauthCodePostResponse
            var tokenObj = JsonSerializerHelper.Deserialize<AdfsOauthCodePostResponse>(systemToken);
            //返回accesstoken
            return await Task.FromResult(tokenObj.AccessToken);
        }

        public async Task<string> GetKeepLoginUrl(string configurationInfo, string systemToken)
        {
            var configuration = getConfiguration(configurationInfo);
            return await Task.FromResult(configuration.AdfsUri);
        }

        public async Task<string> GetLoginUrl(string configurationInfo, string systemLoginRedirectUrl, string clientRedirectUrl)
        {
            await Task.FromResult(0);
            //该服务不支持此操作
            var fragment = new TextFragment()
            {
                Code = TextCodes.ThirdPartySystemServiceNotSupportOperate,
                DefaultFormatting = "第三方系统服务{0}不支持{1}操作",
                ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthPassword).FullName, "GetLoginUrl" }
            };
            throw new UtilityException((int)Errors.ThirdPartySystemServiceNotSupportOperate, fragment);
        }

        public async Task<string> GetLogoutUrl(string configurationInfo, string systemToken)
        {
            var configuration = getConfiguration(configurationInfo);
            return await Task.FromResult($"{configuration.AdfsUri}/adfs/oauth2/logout");
        }

        public async Task<string> GetRealRedirectUrl(string configurationInfo, HttpRequest request)
        {
            await Task.FromResult(0);
            //该服务不支持此操作
            var fragment = new TextFragment()
            {
                Code = TextCodes.ThirdPartySystemServiceNotSupportOperate,
                DefaultFormatting = "第三方系统服务{0}不支持{1}操作",
                ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthPassword).FullName, "GetRealRedirectUrl" }
            };
            throw new UtilityException((int)Errors.ThirdPartySystemServiceNotSupportOperate, fragment);
        }

        public async Task<GetThirdPartySystemTokenResult> GetSystemToken(string configurationInfo, string systemLoginRedirectUrl, string clientRedirectUrl)
        {
            await Task.FromResult(0);
            //该服务不支持此操作
            var fragment = new TextFragment()
            {
                Code = TextCodes.ThirdPartySystemServiceNotSupportOperate,
                DefaultFormatting = "第三方系统服务{0}不支持{1}操作",
                ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthPassword).FullName, "GetSystemToken" }
            };
            throw new UtilityException((int)Errors.ThirdPartySystemServiceNotSupportOperate, fragment);
        }

        public async Task<ThirdPartySystemToken> GetSystemToken(string configurationInfo, HttpRequest request)
        {
            await Task.FromResult(0);
            //该服务不支持此操作
            var fragment = new TextFragment()
            {
                Code = TextCodes.ThirdPartySystemServiceNotSupportOperate,
                DefaultFormatting = "第三方系统服务{0}不支持{1}操作",
                ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthPassword).FullName, "GetSystemToken" }
            };
            throw new UtilityException((int)Errors.ThirdPartySystemServiceNotSupportOperate, fragment);
        }

        public async Task<ThirdPartySystemToken> GetSystemTokenByPassword(string configurationInfo, string userName, string password)
        {
            var configuration = getConfiguration(configurationInfo);
            string strResponseContent = null;
            TextFragment fragment;
            using (var httpClient = _httpClinetFactory.CreateClient())
            {
                StringBuilder strContent = new StringBuilder();
                strContent.Append($"client_id={configuration.ClientId.ToUrlEncode()}");
                strContent.Append($"&scope={configuration.Scope.ToUrlEncode()}");
                strContent.Append($"&grant_type=password");
                strContent.Append($"&username={userName.ToUrlEncode()}");
                strContent.Append($"&password={password.ToUrlEncode()}");

                StringContent content = new StringContent(strContent.ToString());
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var response = await httpClient.PostAsync($"{configuration.AdfsUri}/adfs/oauth2/token", content);
                strResponseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    fragment = new TextFragment()
                    {
                        Code = TextCodes.ThirdPartySystemServiceHttpPostError,
                        DefaultFormatting = "第三方系统服务{0}的HttpPost发生错误，配置信息为{1}，PostUrl为{2}，Post内容为{3}，Post响应为{4}",
                        ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthPassword).FullName, configurationInfo, $"{configuration.AdfsUri}/adfs/oauth2/token", strContent.ToString(), strResponseContent }
                    };

                    throw new UtilityException((int)Errors.ThirdPartySystemServiceHttpPostError, fragment);
                }

            }

            var postResponse = JsonSerializerHelper.Deserialize<AdfsOauthCodePostResponse>(strResponseContent);

            var jwtPL = _securityService.GetPlayloadFromJWT(postResponse.AccessToken);

            if (!jwtPL.ValidateResult.Result)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.ThirdPartySystemServiceTokenValidateError,
                    DefaultFormatting = "第三方系统服务{0}的令牌验证错误，PostUrl：{1}，令牌内容：{2}",
                    ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthPassword).FullName, $"{configuration.AdfsUri}/adfs/oauth2/token", postResponse.AccessToken }
                };

                //验证未通过，抛出异常
                throw new UtilityException((int)Errors.ThirdPartySystemServiceTokenValidateError, fragment);
            }

            var upnKey = "upn";

            if (!jwtPL.Playload.ContainsKey(upnKey))
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.ThirdPartySystemServiceTokenNotContainKey,
                    DefaultFormatting = "第三方系统服务{0}的令牌{1}中缺少键为{2}的键值对，PostUrl：{3}",
                    ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthPassword).FullName, postResponse.AccessToken, upnKey, $"{configuration.AdfsUri}/adfs/oauth2/token" }
                };

                //验证未通过，抛出异常
                throw new UtilityException((int)Errors.ThirdPartySystemServiceTokenNotContainKey, fragment);
            }

            ThirdPartySystemToken result = new ThirdPartySystemToken()
            {
                Token = strResponseContent,
                Attributes = new Dictionary<string, string>()
                 {
                     { "upn",jwtPL.Playload[upnKey]}
                 }
            };

            return result;
        }

        public async Task<int> GetTimeout(string configurationInfo)
        {
            var configuration = getConfiguration(configurationInfo);
            return await Task.FromResult(configuration.Timeout);
        }

        public async Task Logout(string configurationInfo, string systemToken)
        {
            await Task.FromResult(0);
        }

        public async Task<string> RefreshToken(string configurationInfo, string systemToken)
        {
            //令牌格式为AdfsOauthCodePostResponse的json序列化字符串，
            //因此需要反序列化为AdfsOauthCodePostResponse
            var tokenObj = JsonSerializerHelper.Deserialize<AdfsOauthCodePostResponse>(systemToken);

            var configuration = getConfiguration(configurationInfo);

            //执行刷新操作，返回新的令牌
            var url = $"{configuration.AdfsUri}/adfs/oauth2/token";

            string strResponseContent = string.Empty;
            using (var httpClient = _httpClinetFactory.CreateClient())
            {
                StringBuilder strContent = new StringBuilder();
                strContent.Append($"client_id={configuration.ClientId.ToUrlEncode()}");
                strContent.Append($"&refresh_token={tokenObj.RefreshToken.ToUrlEncode()}");
                strContent.Append($"&grant_type=refresh_token");

                StringContent content = new StringContent(strContent.ToString());
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var response = await httpClient.PostAsync($"{configuration.AdfsUri}/adfs/oauth2/token", content);
                strResponseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    /*fragment = new TextFragment()
                    {
                        Code = TextCodes.ThirdPartySystemServiceHttpPostError,
                        DefaultFormatting = "第三方系统服务{0}的HttpPost发生错误，配置信息为{1}，PostUrl为{2}，Post内容为{3}，Post响应为{4}",
                        ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthCode).FullName, configurationInfo, $"{configuration.AdfsUri}/adfs/oauth2/token", strContent.ToString(), strResponseContent }
                    };

                    throw new UtilityException((int)Errors.ThirdPartySystemServiceHttpPostError, fragment);*/

                    return systemToken;
                }

            }

            return strResponseContent;
        }

        public async Task<bool> VerifyToken(string configurationInfo, string systemToken)
        {
            return await Task.FromResult(true);
        }

        private AdfsConfiguration getConfiguration(string configurationInfo)
        {
            return JsonSerializerHelper.Deserialize<AdfsConfiguration>(configurationInfo);
        }

        [DataContract]
        private class AdfsConfiguration
        {
            [DataMember]
            public string AdfsUri { get; set; }
            [DataMember]
            public string Scope { get; set; }
            [DataMember]
            public string ClientId { get; set; }
            [DataMember]
            public int Timeout { get; set; }
        }


        [DataContract]
        private class AdfsOauthCodePostResponse
        {
            [DataMember(Name = "access_token")]
            public string AccessToken { get; set; }

            [DataMember(Name = "token_type")]
            public string TokenType { get; set; }

            [DataMember(Name = "expires_in")]
            public int ExpiresIn { get; set; }
            [DataMember(Name = "refresh_token")]
            public string RefreshToken { get; set; }

            [DataMember(Name = "refresh_token_expires_in")]
            public int RefreshTokenExpiresIn { get; set; }

            [DataMember(Name = "id_token")]
            public string IdToken { get; set; }
            [DataMember(Name = "scope")]
            public string Scope { get; set; }

        }

    }
}
