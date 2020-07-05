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
    /// 基于AdfsOauth的Code模式的第三方系统服务
    /// configuration格式为
    /// {
    /// "AdfsUrl":"adfs地址"
    /// "Resource"："资源",
    /// "ClientId":"应用程序Id",
    /// "ClientSecret":"应用程序机密",
    /// "Domain":"进行身份验证的域，如果不需要指定验证域，则设为null"
    /// "Timeout":"令牌失效时间（与最后刷新时间比，单位秒）"
    /// }
    /// 获取的键值对为
    /// upn
    /// </summary>
    [Injection(InterfaceType = typeof(ThirdPartySystemServiceForAdfsOauthCode), Scope = InjectionScope.Singleton)]
    public class ThirdPartySystemServiceForAdfsOauthCode : IThirdPartySystemService
    {
        private IHttpClientFactoryWrapper _httpClinetFactory;
        private ISecurityService _securityService;

        public ThirdPartySystemServiceForAdfsOauthCode(IHttpClientFactoryWrapper httpClinetFactory, ISecurityService securityService)
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

        public async Task<string> GetLoginUrl(string configurationInfo, string systemLoginRedirectUrl, string clientRedirectUrl)
        {
            var configuration = getConfiguration(configurationInfo);

            var url = $"{configuration.AdfsUri}/adfs/oauth2/authorize";

            var response_type = "code";

            var sb = new StringBuilder();
            sb.Append(url);
            sb.Append("?");
            sb.Append($"response_type={response_type}");
            sb.Append("&");
            sb.Append($"client_id={configuration.ClientId}");
            sb.Append("&");
            sb.Append($"redirect_uri={systemLoginRedirectUrl.ToUrlEncode()}");
            sb.Append("&");
            sb.Append($"resource={configuration.Resource.ToUrlEncode()}");
            sb.Append("&");
            sb.Append($"state={clientRedirectUrl.ToUrlEncode()}");
            if (configuration.Domain!=null)
            {
                sb.Append("&");
                sb.Append($"domain_hint={configuration.Domain.ToUrlEncode()}");
            }
            
            url = sb.ToString();
            return await Task.FromResult(url);
        }

        public async Task<string> GetLogoutUrl(string configurationInfo, string systemToken)
        {
            var configuration = getConfiguration(configurationInfo);
            return await Task.FromResult($"{configuration.AdfsUri}/adfs/oauth2/logout");
        }

        public async Task<string> GetRealRedirectUrl(string configurationInfo, HttpRequest request)
        {
            var stateCollection=request.Query["state"];
            if (stateCollection.Count==0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterFromHttpRequestInThirdPartySystemService,
                    DefaultFormatting = "在第三方系统服务{0}中的Http请求中，配置信息为{1}，找不到名称为{2}的参数",
                    ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthCode).FullName,configurationInfo, "state" }
                };

                throw new UtilityException((int)Errors.NotFoundParameterFromHttpRequestInThirdPartySystemService, fragment);
            }

            return await Task.FromResult(stateCollection[0]);
        }

        public async Task<GetThirdPartySystemTokenResult> GetSystemToken(string configurationInfo, string systemLoginRedirectUrl, string clientRedirectUrl)
        {
            var configuration = getConfiguration(configurationInfo);
            GetThirdPartySystemTokenResult result = new GetThirdPartySystemTokenResult()
            {
                Direct = false,
                 RedirectUrl= await GetLoginUrl(configurationInfo, systemLoginRedirectUrl, clientRedirectUrl)
            };
            return result;
        }

        /// <summary>
        /// 从Http请求中获取第三方系统令牌
        /// 在该实现中获取的键值对为
        /// upn
        /// </summary>
        /// <param name="configurationInfo"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ThirdPartySystemToken> GetSystemToken(string configurationInfo, HttpRequest request)
        {
            TextFragment fragment;
            var configuration = getConfiguration(configurationInfo);
            var stateCollection = request.Query["code"];
            if (stateCollection.Count == 0)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterFromHttpRequestInThirdPartySystemService,
                    DefaultFormatting = "在第三方系统服务{0}中的Http请求中，配置信息为{1}，找不到名称为{2}的参数",
                    ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthCode).FullName, configurationInfo, "code" }
                };

                throw new UtilityException((int)Errors.NotFoundParameterFromHttpRequestInThirdPartySystemService, fragment);
            }
            var code = stateCollection[0];

            string strResponseContent = null;
            using (var httpClient=_httpClinetFactory.CreateClient())
            {
                StringBuilder strContent = new StringBuilder();
                strContent.Append($"client_id={configuration.ClientId.ToUrlEncode()}");
                strContent.Append($"&code={code.ToUrlEncode()}");
                strContent.Append($"&redirect_uri={request.GetDisplayUrl().ToUrlEncode()}");
                strContent.Append($"&grant_type=authorization_code");
                strContent.Append($"&client_secret={configuration.ClientSecret.ToUrlEncode()}");


                StringContent content = new StringContent(strContent.ToString());
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var response=await httpClient.PostAsync($"{configuration.AdfsUri}/adfs/oauth2/token", content);
                strResponseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                     fragment = new TextFragment()
                    {
                        Code = TextCodes.ThirdPartySystemServiceHttpPostError,
                        DefaultFormatting = "第三方系统服务{0}的HttpPost发生错误，配置信息为{1}，PostUrl为{2}，Post内容为{3}，Post响应为{4}",
                        ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthCode).FullName,configurationInfo, $"{configuration.AdfsUri}/adfs/oauth2/token", strContent.ToString(), strResponseContent }
                    };

                    throw new UtilityException((int)Errors.ThirdPartySystemServiceHttpPostError, fragment);
                }
             
            }

            var postResponse = JsonSerializerHelper.Deserialize<AdfsOauthCodePostResponse>(strResponseContent);

            var jwtPL= _securityService.GetPlayloadFromJWT(postResponse.AccessToken);

            if (!jwtPL.ValidateResult.Result)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.ThirdPartySystemServiceTokenValidateError,
                    DefaultFormatting = "第三方系统服务{0}的令牌验证错误，PostUrl：{1}，令牌内容：{2}",
                    ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthCode).FullName, $"{configuration.AdfsUri}/adfs/oauth2/token", postResponse.AccessToken }
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
                    ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthCode).FullName, postResponse.AccessToken, upnKey, $"{configuration.AdfsUri}/adfs/oauth2/token" }
                };

                //验证未通过，抛出异常
                throw new UtilityException((int)Errors.ThirdPartySystemServiceTokenNotContainKey, fragment);
            }




            ThirdPartySystemToken result = new ThirdPartySystemToken()
            {
                 Token= strResponseContent,
                 Attributes=new Dictionary<string, string>()
                 {
                     { "upn",jwtPL.Playload[upnKey]}
                 }
            };

            return result;
        }

        public async Task<ThirdPartySystemToken> GetSystemTokenByPassword(string configurationInfo, string userName, string password)
        {
            await Task.FromResult(0);
            //该服务不支持此操作
            var fragment = new TextFragment()
            {
                Code = TextCodes.ThirdPartySystemServiceNotSupportOperate,
                DefaultFormatting = "第三方系统服务{0}不支持{1}操作",
                ReplaceParameters = new List<object>() { typeof(ThirdPartySystemServiceForAdfsOauthCode).FullName, "GetSystemTokenByPassword" }
            };
            throw new UtilityException((int)Errors.ThirdPartySystemServiceNotSupportOperate, fragment);
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
                strContent.Append($"&client_secret={configuration.ClientSecret.ToUrlEncode()}");

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


        public async Task<string> GetKeepLoginUrl(string configurationInfo, string systemToken)
        {
            var configuration = getConfiguration(configurationInfo);
            return await Task.FromResult(configuration.AdfsUri);
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
            public string Resource { get; set; }
            [DataMember]
            public string ClientId { get; set; }
            [DataMember]
            public string ClientSecret { get; set; }
            [DataMember]
            public string Domain { get; set; }
            [DataMember]
            public int Timeout { get; set; }
        }

        [DataContract]
        private class AdfsOauthCodePostResponse
        {
            [DataMember(Name ="access_token")]
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

        }
    }
}
