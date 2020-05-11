using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.AspNet.AuthorizationPolicyProviders;
using IdentityCenter.Main.AspNet.AuthorizationPolicyProviders.HttpContextPolicyResolveServices;
using IdentityCenter.Main.Application;
using IdentityCenter.Main.DTOModel;
using IdentityCenter.Main;

namespace IdentityCenter.ClientService.Controllers
{
    [Route("api/endpoint")]
    [ApiController]
    public class EndpointController : ControllerBase
    {
        private readonly IAppOpenIDRefreshToken _appRefreshToken;

        public EndpointController(IAppOpenIDRefreshToken appRefreshToken)
        {
            _appRefreshToken = appRefreshToken;
        }

        /// <summary>
        /// OpenID协议的登录
        /// </summary>
        /// <returns></returns>
        [HttpGet("openidauthorize")]
        [HttpContextAuthorize(Type=typeof(HttpContextPolicyResolveServiceForOpenID))]
        public async Task OpenIDLogin()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// OpenID协议的刷新令牌
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPost("openidrefreshtoken")]
        [AllowAnonymous]
        public async Task<NewTokenModel> OpenIDRefreshToken(string binding)
        {
       
            var refreshTokenCookiesName = string.Format(OpenIDCookiesNames.RefreshToken, binding);
            //从cookies中获取refreshtoken
            if (!HttpContext.Request.Cookies.TryGetValue(refreshTokenCookiesName, out string refreshToken))
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundOpenIDRefreshTokenInCookies,
                    DefaultFormatting = "在Cookies中找不到名称为{0}的OpenID刷新令牌",
                    ReplaceParameters = new List<object>() { refreshTokenCookiesName }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundOpenIDRefreshTokenInCookies, fragment, 1, 0);
            }

            var resultModel=await _appRefreshToken.Do(binding, refreshToken, HttpContext.RequestAborted);

            //需要重新刷新cookies
            HttpContext.Response.Cookies.Append(refreshTokenCookiesName, resultModel.RefreshToken, new CookieOptions() { HttpOnly=true });
            return new NewTokenModel() { Token = resultModel.Token, Expire = resultModel.Expire };
        }

        /// <summary>
        /// OpenID协议的登出
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPost("openidlogout")]
        [AllowAnonymous]
        public async Task<IActionResult> OpenIDLogout(LogoutViewModel vm)
        {
            AuthenticationProperties properties = new AuthenticationProperties();
            properties.Parameters.Add(OpenIDLogoutParameterNames.IDToken, vm.IDToken);
            properties.Parameters.Add(OpenIDLogoutParameterNames.ReturnUrl, vm.ReturnUrl);
            return await Task.FromResult(SignOut(properties,vm.Binding));
        }

    }
}