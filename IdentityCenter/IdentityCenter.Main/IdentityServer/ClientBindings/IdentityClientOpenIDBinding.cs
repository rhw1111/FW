using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using System.Net;
using System.Linq;
using System.Runtime.Serialization;

namespace IdentityCenter.Main.IdentityServer.ClientBindings
{
    public class IdentityClientOpenIDBinding:IdentityClientBinding
    {
        private static IFactory<IIdentityClientOpenIDBindingIMP>? _identityClientOpenIDBindingIMPFactory;

        public static IFactory<IIdentityClientOpenIDBindingIMP> IdentityClientOpenIDBindingIMPFactory
        {
            set
            {
                _identityClientOpenIDBindingIMPFactory = value;
            }
        }


        private class innerIMPFactory : IFactory<IIdentityClientOpenIDBindingIMP>
        {
            public IIdentityClientOpenIDBindingIMP Create()
            {
                IIdentityClientOpenIDBindingIMP imp;
                var di = ContextContainer.GetValue<IDIContainer>("DI");
                if (di == null)
                {
                    imp = DIContainerContainer.Get<IIdentityClientOpenIDBindingIMP>();
                }
                else
                {
                    imp = di.Get<IIdentityClientOpenIDBindingIMP>();
                }
                return imp;
            }
        }


        public override IFactory<IIdentityClientBindingIMP>? GetIMPFactory()
        {
            if (_identityClientOpenIDBindingIMPFactory == null)
            {
                return new innerIMPFactory();
            }
            else
            {
                return _identityClientOpenIDBindingIMPFactory;
            }
        }

        public string ClientId
        {
            get
            {

                return GetAttribute<string>(nameof(ClientId));
            }
            set
            {
                SetAttribute<string>(nameof(ClientId), value);
            }
        }

        public string ClientSecret
        {
            get
            {

                return GetAttribute<string>(nameof(ClientSecret));
            }
            set
            {
                SetAttribute<string>(nameof(ClientSecret), value);
            }
        }

        public string ResponseMode
        {
            get
            {

                return GetAttribute<string>(nameof(ResponseMode));
            }
            set
            {
                SetAttribute<string>(nameof(ResponseMode), value);
            }
        }

        public string ResponseType
        {
            get
            {

                return GetAttribute<string>(nameof(ResponseType));
            }
            set
            {
                SetAttribute<string>(nameof(ResponseType), value);
            }
        }

        public string Authority
        {
            get
            {

                return GetAttribute<string>(nameof(Authority));
            }
            set
            {
                SetAttribute<string>(nameof(Authority), value);
            }
        }

        public bool RequireHttpsMetadata
        {
            get
            {

                return GetAttribute<bool>(nameof(RequireHttpsMetadata));
            }
            set
            {
                SetAttribute<bool>(nameof(RequireHttpsMetadata), value);
            }
        }

        /// <summary>
        /// 令牌传递是否使用querystring
        /// false，使用#
        /// </summary>
        public bool TokenUseQuery
        {
            get
            {

                return GetAttribute<bool>(nameof(TokenUseQuery));
            }
            set
            {
                SetAttribute<bool>(nameof(TokenUseQuery), value);
            }
        }

        public string? AccessDeniedPath
        {
            get
            {

                return GetAttribute<string?>(nameof(AccessDeniedPath));
            }
            set
            {
                SetAttribute<string?>(nameof(AccessDeniedPath), value);
            }
        }

        public List<string> Scope
        {
            get
            {

                return GetAttribute<List<string>>(nameof(Scope));
            }
            set
            {
                SetAttribute<List<string>>(nameof(Scope), value);
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        public string RedirectUrl
        {
            get
            {

                return GetAttribute<string>(nameof(RedirectUrl));
            }
            set
            {
                SetAttribute<string>(nameof(RedirectUrl), value);
            }
        }

        /// <summary>
        /// 初始化指定的Options
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public override async Task<IIdentityClientBindingOptionsInit> InitOptions()
        {
            return await ((IIdentityClientOpenIDBindingIMP)_imp).InitOptions(this);
        }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<RefreshTokenResult> RefreshToken( string refreshToken, CancellationToken cancellationToken = default)
        {
            return await ((IIdentityClientOpenIDBindingIMP)_imp).RefreshToken(this, refreshToken, cancellationToken);
        }

    }

    public interface IIdentityClientOpenIDBindingIMP : IIdentityClientBindingIMP
    {
        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<RefreshTokenResult> RefreshToken(IdentityClientOpenIDBinding binding,string refreshToken,CancellationToken cancellationToken = default);
    }

    public abstract class RefreshTokenResult
    {
        public abstract string AccessToken { get; }
        public abstract string RefreshToken { get; }
        public abstract int ExpireSeconds { get; }
    }

    [Injection(InterfaceType = typeof(IIdentityClientOpenIDBindingIMP), Scope = InjectionScope.Transient)]
    public class IdentityClientOpenIDBindingIMP:IdentityClientBindingIMP, IIdentityClientOpenIDBindingIMP
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public IdentityClientOpenIDBindingIMP(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public override async Task<IIdentityClientBindingOptionsInit> InitOptions(IdentityClientBinding binding)
        {
            var realBinding = (IdentityClientOpenIDBinding)binding;
            return await Task.FromResult(new IIdentityClientBindingOptionsInitForOpenID(realBinding));
        }

        public async Task<RefreshTokenResult> RefreshToken(IdentityClientOpenIDBinding binding, string refreshToken,CancellationToken cancellationToken = default)
        {
            using (var client=_httpClientFactory.CreateClient())
            {
                string strPost = $"client_id={WebUtility.UrlEncode(binding.ClientId)}&client_secret={WebUtility.UrlEncode(binding.ClientSecret)}&grant_type=refresh_token&refresh_token={WebUtility.UrlEncode(refreshToken)}&scope={WebUtility.UrlEncode(binding.Scope.ToDisplayString((str)=>str,()=>" "))}";


                StringContent strContent = new StringContent(strPost);
                strContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsync($"{binding.IdentityServerUrl}/token", strContent,cancellationToken);
                }
                catch(Exception ex)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.OpenIDRefreshTokenRequestError,
                        DefaultFormatting = "OpenID刷新令牌请求出错，错误内容为{0}，绑定名称为{1}",
                        ReplaceParameters = new List<object>() { ex.ToStackTraceString(),binding.Name}
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.OpenIDRefreshTokenRequestError, fragment, -1, 0);
                }
                if (response.IsSuccessStatusCode)
                {
         
                    var responseObj=JsonSerializerHelper.Deserialize<refreshTokenResponse>(await response.Content.ReadAsStringAsync());
                    var result = new refreshTokenResult(responseObj.AccessToken, responseObj.RefreshToken, responseObj.ExpiresIn);
                    return result;
                }
                else
                {

                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = response.ReasonPhrase;
                    }

                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.OpenIDRefreshTokenResponseError,
                        DefaultFormatting = "OpenID刷新令牌响应出错，错误内容为{0},绑定名称为{1}",
                        ReplaceParameters = new List<object>() { errorMessage,binding.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.OpenIDRefreshTokenResponseError, fragment, -1, 0);
                }
            }
            
        }

        private class refreshTokenResult:RefreshTokenResult
        {
            private string _accessToken;
            private string _refreshToken;
            private int _expireSeconds;

            public refreshTokenResult(string accessToken, string refreshToken, int expireSeconds)
            {
                _accessToken = accessToken;
                _refreshToken = refreshToken;
                _expireSeconds = expireSeconds;
            }
            public override string AccessToken
            {
                get
                {
                    return _accessToken;
                }
            }

            public override string RefreshToken
            {
                get
                {
                    return _refreshToken;
                }
            }

            public override int ExpireSeconds
            {
                get
                {
                    return _expireSeconds;
                }
            }
        }

        [DataContract]
        private class refreshTokenResponse
        {
            [DataMember(Name = "access_token")]
            public string AccessToken
            {
                get; set;
            } = null!;

            [DataMember(Name = "refresh_token")]
            public string RefreshToken
            {
                get; set;
            } = null!;

            [DataMember(Name = "expires_in")]
            public int ExpiresIn
            {
                get; set;
            }
        }
    }


    public class IIdentityClientBindingOptionsInitForOpenID : IIdentityClientBindingOptionsInit
    {
        private const string _accessTokenParameterName = "accesstoken";
        private const string _idTokenParameterName = "idtoken";

        private readonly IdentityClientOpenIDBinding _binding;

        public IIdentityClientBindingOptionsInitForOpenID(IdentityClientOpenIDBinding binding)
        {
            _binding = binding;
        }
        public void Init<T>(T options)
        {
            var realOptions = options as OpenIdConnectOptions;
            if (realOptions == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.TypeNotRequire,
                    DefaultFormatting = "类型{0}不是所需的类型，要求的类型为{1}，发生位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(T).FullName ?? string.Empty, typeof(OpenIdConnectOptions).FullName ?? string.Empty, $"{typeof(IdentityClientOpenIDBindingIMP).FullName}.InitOptions" }
                };

                throw new UtilityException((int)Errors.TypeNotRequire, fragment, 1, 0);
            }

            realOptions.Authority = _binding.Authority;
            realOptions.RequireHttpsMetadata = _binding.RequireHttpsMetadata;
            realOptions.ClientId = _binding.ClientId;
            realOptions.ClientSecret = _binding.ClientSecret;
            realOptions.AccessDeniedPath = _binding.AccessDeniedPath;
            realOptions.ResponseMode = _binding.ResponseMode;
            realOptions.ResponseType = _binding.ResponseType;


            foreach (var scopeItem in _binding.Scope)
            {
                realOptions.Scope.Add(scopeItem);
            }
            realOptions.Events.OnRedirectToIdentityProvider = async (context) =>
            {
                //检查请求中的returnurl参数是否匹配AllowReturnBaseUrls
                if (!context.Request.Query.TryGetValue("returnurl", out StringValues returnUrls))
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.NotFoundReturnUrlInOpenIDRequest,
                        DefaultFormatting = "在OpenID认证请求中找不到ReturnUrl参数,绑定名称为{0}",
                        ReplaceParameters = new List<object>() { _binding.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundReturnUrlInOpenIDRequest, fragment, 1, 0);
                }

                var returnUrl = returnUrls[0];
                bool urlValidate = false;
                foreach (var item in _binding.AllowReturnBaseUrls)
                {
                    urlValidate = (new Uri(item)).IsBaseOf(new Uri(returnUrl));
                    if (urlValidate)
                    {
                        break;
                    }
                }

                if (!urlValidate)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.OpenIDRequestReturnUrlInvalid,
                        DefaultFormatting = "OpenID认证请求中的ReturnUrl验证错误，当前ReturnUrl为{0}，合法的基地址为{1}，绑定名称为{2}",
                        ReplaceParameters = new List<object>() { returnUrl, _binding.AllowReturnBaseUrls.ToDisplayString((str) => str, () => ""), _binding.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.OpenIDRequestReturnUrlInvalid, fragment, 1, 0);
                }

                //string state = Guid.NewGuid().ToString();
                //将returnurl保存到cookies中，通过state标识
                var objStaterReturnUrl = new staterReturnUrl()
                {
                    State = $"{_binding.Name}+{Guid.NewGuid().ToString()}",
                    ReturnUrl = returnUrl
                };
                context.Response.Cookies.Append(string.Format(OpenIDCookiesNames.State, _binding.Name), JsonSerializerHelper.Serializer(objStaterReturnUrl), new CookieOptions() { HttpOnly = true, });
                //为上下文赋值
                context.ProtocolMessage.State = _binding.Name;
                context.ProtocolMessage.RedirectUri = _binding.RedirectUrl;
                await Task.CompletedTask;
            };

            realOptions.Events.OnTokenValidated = async (context) =>
            {

                //从cookies中获取returnurl
                if (!context.Request.Cookies.TryGetValue(string.Format(OpenIDCookiesNames.State, _binding.Name), out string strReturnUrl))
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.NotFoundReturnUrlStateInOpenIDCookies,
                        DefaultFormatting = "在OpenID的Cookies中找不到名称为{0}的StateReturnUrl，绑定名称为{1}",
                        ReplaceParameters = new List<object>() { string.Format(OpenIDCookiesNames.State, _binding.Name), _binding.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundReturnUrlStateInOpenIDCookies, fragment, 1, 0);
                }

                var returnUrlObj = JsonSerializerHelper.Deserialize<staterReturnUrl>(strReturnUrl);
                if (returnUrlObj.State != context.ProtocolMessage.State)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.OpenIDStateInvalid,
                        DefaultFormatting = "OpenID认证中的State验证错误，接收的state为{0}，存储的state为{1}，绑定名称为{2}",
                        ReplaceParameters = new List<object>() { context.ProtocolMessage.State, returnUrlObj.State ?? string.Empty, _binding.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.OpenIDStateInvalid, fragment, 1, 0);
                }

                //验证returnurl
                var returnUrl = returnUrlObj.ReturnUrl;
                bool urlValidate = false;
                foreach (var item in _binding.AllowReturnBaseUrls)
                {
                    urlValidate = (new Uri(item)).IsBaseOf(new Uri(returnUrl));
                    if (urlValidate)
                    {
                        break;
                    }
                }

                if (!urlValidate)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.OpenIDRequestReturnUrlInvalid,
                        DefaultFormatting = "OpenID认证请求中的ReturnUrl验证错误，当前ReturnUrl为{0}，合法的基地址为{1}，绑定名称为{2}",
                        ReplaceParameters = new List<object>() { returnUrl, _binding.AllowReturnBaseUrls.ToDisplayString((str) => str, () => ""), _binding.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.OpenIDRequestReturnUrlInvalid, fragment, 1, 0);
                }

                var returnUri = new Uri(returnUrl);
                 

                if (_binding.TokenUseQuery)
                {
                    var dictQuery = QueryHelpers.ParseQuery(returnUri.Query);
                    dictQuery[_accessTokenParameterName] = context.ProtocolMessage.AccessToken;
                    dictQuery[_idTokenParameterName] = context.ProtocolMessage.IdToken;
                    returnUrl = QueryHelpers.AddQueryString(returnUri.ToString(), dictQuery.ToDictionary((kv) => kv.Key, (kv) => kv.Value[0]));
                }
                else
                {
                    if (returnUrl.IndexOf('#') == -1)
                    {
                        returnUrl = $"{returnUrl}#{_accessTokenParameterName}={WebUtility.UrlEncode(context.ProtocolMessage.AccessToken)}&{_idTokenParameterName}={WebUtility.UrlEncode(context.ProtocolMessage.IdToken)}";
                    }
                    else
                    {
                        returnUrl = $"{returnUrl}&{_accessTokenParameterName}={WebUtility.UrlEncode(context.ProtocolMessage.AccessToken)}&{_idTokenParameterName}={WebUtility.UrlEncode(context.ProtocolMessage.IdToken)}";
                    }
                }
                //移除cookies中的returnurl
                context.Response.Cookies.Delete(string.Format(OpenIDCookiesNames.State, _binding.Name));
                //将refreshtoken加入到cookies
                if (context.ProtocolMessage.RefreshToken != null)
                {
                    context.Response.Cookies.Append(string.Format(OpenIDCookiesNames.RefreshToken, _binding.Name), context.ProtocolMessage.RefreshToken, new CookieOptions() { HttpOnly = true });
                }

                //重定向到returnurl
                context.Response.Redirect(returnUrl);
                context.HandleResponse();
                await Task.CompletedTask;
            };

            realOptions.Events.OnSignedOutCallbackRedirect = async (context) =>
            {
                //从cookies中获取returnurl
                if (!context.Request.Cookies.TryGetValue(string.Format(OpenIDCookiesNames.LogoutState, _binding.Name), out string strReturnUrl))
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.NotFoundReturnUrlStateInOpenIDCookies,
                        DefaultFormatting = "在OpenID的Cookies中找不到名称为{0}的StateReturnUrl，绑定名称为{1}",
                        ReplaceParameters = new List<object>() { string.Format(OpenIDCookiesNames.LogoutState, _binding.Name), _binding.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundReturnUrlStateInOpenIDCookies, fragment, 1, 0);
                }

                var returnUrlObj = JsonSerializerHelper.Deserialize<staterReturnUrl>(strReturnUrl);
                if (returnUrlObj.State != context.ProtocolMessage.State)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.OpenIDStateInvalid,
                        DefaultFormatting = "OpenID认证中的State验证错误，接收的state为{0}，存储的state为{1}，绑定名称为{2}",
                        ReplaceParameters = new List<object>() { context.ProtocolMessage.State, returnUrlObj.State ?? string.Empty, _binding.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.OpenIDStateInvalid, fragment, 1, 0);
                }

                //验证returnurl
                var returnUrl = returnUrlObj.ReturnUrl;
                bool urlValidate = false;
                foreach (var item in _binding.AllowReturnBaseUrls)
                {
                    urlValidate = (new Uri(item)).IsBaseOf(new Uri(returnUrl));
                    if (urlValidate)
                    {
                        break;
                    }
                }

                if (!urlValidate)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.OpenIDRequestReturnUrlInvalid,
                        DefaultFormatting = "OpenID认证请求中的ReturnUrl验证错误，当前ReturnUrl为{0}，合法的基地址为{1}，绑定名称为{2}",
                        ReplaceParameters = new List<object>() { returnUrl, _binding.AllowReturnBaseUrls.ToDisplayString((str) => str, () => ""), _binding.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.OpenIDRequestReturnUrlInvalid, fragment, 1, 0);
                }

                //移除所有cookies
                var cookeiesNames = (from item in context.Request.Cookies
                                     select item.Key).ToList();

                foreach (var item in cookeiesNames)
                {
                    context.Response.Cookies.Delete(item);
                }

                //重定向到returnurl
                context.Response.Redirect(returnUrl);
                context.HandleResponse();
                await Task.CompletedTask;

            };
            realOptions.Events.OnRedirectToIdentityProviderForSignOut = async (context) =>
            {
                //检查认证参数中的returnurl参数是否匹配AllowReturnBaseUrls
                if (!context.Properties.Parameters.TryGetValue(OpenIDLogoutParameterNames.ReturnUrl, out object? returnUrlObj))
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.NotFoundReturnUrlInOpenIDRequest,
                        DefaultFormatting = "在OpenID认证请求中找不到ReturnUrl参数,绑定名称为{0}",
                        ReplaceParameters = new List<object>() { _binding.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundReturnUrlInOpenIDRequest, fragment, 1, 0);
                }

                string returnUrl = returnUrlObj?.ToString() ?? string.Empty;

                bool urlValidate = false;
                foreach (var item in _binding.AllowReturnBaseUrls)
                {
                    urlValidate = (new Uri(item)).IsBaseOf(new Uri(returnUrl));
                    if (urlValidate)
                    {
                        break;
                    }
                }

                if (!urlValidate)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.OpenIDRequestReturnUrlInvalid,
                        DefaultFormatting = "OpenID认证请求中的ReturnUrl验证错误，当前ReturnUrl为{0}，合法的基地址为{1}，绑定名称为{2}",
                        ReplaceParameters = new List<object>() { returnUrl, _binding.AllowReturnBaseUrls.ToDisplayString((str) => str, () => ""), _binding.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.OpenIDRequestReturnUrlInvalid, fragment, 1, 0);
                }


                //将returnurl保存到cookies中，通过state标识
                var objStaterReturnUrl = new staterReturnUrl()
                {
                    State = $"{_binding.Name}+{Guid.NewGuid().ToString()}",
                    ReturnUrl = returnUrl
                };
                context.Response.Cookies.Append(string.Format(OpenIDCookiesNames.LogoutState, _binding.Name), JsonSerializerHelper.Serializer(objStaterReturnUrl), new CookieOptions() { HttpOnly = true, });

                //为上下文赋值
                context.ProtocolMessage.State = _binding.Name;
                context.ProtocolMessage.PostLogoutRedirectUri = _binding.RedirectUrl;
                await Task.CompletedTask;

            };
        }

        private class staterReturnUrl
        {
            public string State { get; set; } = null!;
            public string ReturnUrl { get; set; } = null!;
        }
    }
}
