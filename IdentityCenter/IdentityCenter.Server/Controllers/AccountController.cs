using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using MSLibrary;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main;
using IdentityCenter.Main.Application;
using IdentityCenter.Main.DTOModel;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Server.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly IAppGetLoginViewModel _appGetLoginViewModel;
        private readonly IAppLogin _appLogin;
        private readonly IAppExternalLoginPre _appExternalLoginPre;
        private readonly IAppExternalLoginCallback _appExternalLoginCallback;
        private readonly IAppExternalBind _appExternalBind;
        private readonly IAppGetLogoutInfo _appGetLogoutInfo;

        public AccountController(IIdentityServerInteractionService interaction, IClientStore clientStore, IAuthenticationSchemeProvider schemeProvider, IEventService events, 
            IAppGetLoginViewModel appGetLoginViewModel,
            IAppLogin appLogin,
           IAppExternalLoginPre appExternalLoginPre,
           IAppExternalLoginCallback appExternalLoginCallback,
           IAppExternalBind appExternalBind,
           IAppGetLogoutInfo appGetLogoutInfo
            )
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _appGetLoginViewModel = appGetLoginViewModel;
            _appLogin = appLogin;
            _appExternalLoginPre = appExternalLoginPre;
            _appExternalLoginCallback = appExternalLoginCallback;
            _appExternalBind = appExternalBind;
            _appGetLogoutInfo = appGetLogoutInfo;
        }


        [HttpGet("getloginview")]
        public async Task<LoginViewModel> GetLoginView([FromQuery]string returnUrl)
        {
            var schemes = await _schemeProvider.GetAllSchemesAsync();
            var schemeNames = (from item in schemes
                               select item.Name).ToList();

            var loginViewModel = await _appGetLoginViewModel.Do(schemeNames, HttpContext.RequestAborted);

            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                loginViewModel.EnableLocalLogin = local;
                loginViewModel.HintUserName = context?.LoginHint;


                if (!local)
                {
                    loginViewModel.IdentityProviders.Add(new IdentityProviderModel() { SchemeName=context.IdP }) ;
                }

                return loginViewModel;
            }

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        loginViewModel.IdentityProviders = loginViewModel.IdentityProviders.Where(provider => client.IdentityProviderRestrictions.Contains(provider.SchemeName)).ToList();
                    }
                }
            }

            loginViewModel.ReturnUrl = returnUrl;
            loginViewModel.EnableLocalLogin = allowLocal && loginViewModel.EnableLocalLogin;
            loginViewModel.HintUserName = context?.LoginHint;

            return loginViewModel;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LocalLoginRequest request)
        {
            var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);

            var loginResult = await _appLogin.Do(request, HttpContext.RequestAborted);
            await _events.RaiseAsync(new UserLoginSuccessEvent(loginResult.UserName, loginResult.SubjectID, loginResult.UserName, clientId: context?.ClientId));


            AuthenticationProperties props = null;
            if (loginResult.RememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(loginResult.RememberMeLoginDuration)
                };
            };

            // issue authentication cookie with subject ID and username
            await HttpContext.SignInAsync(loginResult.SubjectID, loginResult.UserName, props);

            if (context != null)
            {
                return Redirect(request.ReturnUrl);
            }

            // request for a local page
            if (Url.IsLocalUrl(request.ReturnUrl))
            {
                return Redirect(request.ReturnUrl);
            }
            else
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.IdentityServerReturnUrlInvalid,
                    DefaultFormatting = "认证服务的重定向地址验证不通过，当前地址为{0}",
                    ReplaceParameters = new List<object>() { request.ReturnUrl }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.IdentityServerReturnUrlInvalid, fragment, 1, 0);
            }
        }


        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);
                return Redirect(returnUrl);
            }
            else
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.IdentityServerReturnUrlInvalid,
                    DefaultFormatting = "认证服务的重定向地址验证不通过，当前地址为{0}",
                    ReplaceParameters = new List<object>() { returnUrl }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.IdentityServerReturnUrlInvalid, fragment, 1, 0);
            }
        }

        [HttpGet("externallogin")]
        public async Task<IActionResult> ExternalLogin(string schemeName, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.IdentityServerReturnUrlInvalid,
                    DefaultFormatting = "认证服务的重定向地址验证不通过，当前地址为{0}",
                    ReplaceParameters = new List<object>() { returnUrl }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.IdentityServerReturnUrlInvalid, fragment, 1, 0);
            }

            var preResult=await _appExternalLoginPre.Do(schemeName, HttpContext.RequestAborted);

            var props = new AuthenticationProperties
            {
                RedirectUri = preResult.ExternalCallbackUri,
                Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", schemeName },
                    }
            };

            return Challenge(props, schemeName);

        }


        [HttpGet("externalcallback")]
        public async Task<IActionResult> ExternalCallback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.ExternalIdentityAuthenticationError,
                    DefaultFormatting = "第三方认证方认证失败",
                    ReplaceParameters = new List<object>() {}
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.ExternalIdentityAuthenticationError, fragment, 1, 0);
            }

            var loginResult = await _appExternalLoginCallback.Do(result, HttpContext.RequestAborted);

            if (loginResult.ExistsUserAccount)
            {
                var context = await _interaction.GetAuthorizationContextAsync(loginResult.ReturnUrl);
                AuthenticationProperties props = null;
                await HttpContext.SignInAsync(loginResult.SubjectID, loginResult.UserName, props);
                await HttpContext.SignOutAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
                await _events.RaiseAsync(new UserLoginSuccessEvent(loginResult.SchemeName, loginResult.ProviderUserId, loginResult.SubjectID, loginResult.UserName, true, context?.ClientId));
                return Redirect(loginResult.ReturnUrl);
            }
            else
            {
                return Redirect(loginResult.ExternalIdentityBindPage);
            }      
        }

        [HttpPost("externalbindcallback")]
        public async Task<IActionResult> ExternalBindCallback(ExternalBindUser bindUser)
        {
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.ExternalIdentityAuthenticationError,
                    DefaultFormatting = "第三方认证方认证失败",
                    ReplaceParameters = new List<object>() { }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.ExternalIdentityAuthenticationError, fragment, 1, 0);
            }

            var bindResult=await _appExternalBind.Do(bindUser, result, HttpContext.RequestAborted);

            var context = await _interaction.GetAuthorizationContextAsync(bindResult.ReturnUrl);
            AuthenticationProperties props = null;
            await HttpContext.SignInAsync(bindResult.SubjectID, bindResult.UserName, props);
            await HttpContext.SignOutAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
            await _events.RaiseAsync(new UserLoginSuccessEvent(bindResult.SchemeName, bindResult.ProviderUserId, bindResult.SubjectID, bindResult.UserName, true, context?.ClientId));
            return Redirect(bindResult.ReturnUrl);

        }


        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var logoutInfo = await _appGetLogoutInfo.Do(HttpContext.RequestAborted);
            if (User?.Identity.IsAuthenticated == true)
            {
                await HttpContext.SignOutAsync();
                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {                 
                        var returnUrl = QueryHelpers.AddQueryString(logoutInfo.ExternalLogoutCallbackUri, new Dictionary<string, string>() {  { "logoutId", logoutId } });
                        return SignOut(new AuthenticationProperties { RedirectUri = returnUrl }, idp);
                    }
                }
            }
            var logoutPage = QueryHelpers.AddQueryString(logoutInfo.LoggedPage, new Dictionary<string, string>() { { "logoutId", logoutId } });
            return Redirect(logoutPage);
        }

        [HttpGet("logout")]
        public async Task<LoggedOutViewModel> GetLoggedOutView(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {          
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            return vm;
        }
    }


}