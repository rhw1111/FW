using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MSLibrary.SystemToken;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.AspNet.AuthenticationHandlers
{
    /// <summary>
    /// 令牌认证处理
    /// </summary>
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        private ITokenControllerRepositoryCacheProxy _tokenControllerRepositoryCacheProxy;
        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ITokenControllerRepositoryCacheProxy tokenControllerRepositoryCacheProxy) : base(options, logger, encoder, clock)
        {
            
            _tokenControllerRepositoryCacheProxy = tokenControllerRepositoryCacheProxy;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            string headerName = Options.HeaderName;
            string tokenControllerName = Options.TokenControllerName;
            if (Options.Resolve!=null)
            {
                headerName = await Options.Resolve.GetHeaderName(Request);
                tokenControllerName = await Options.Resolve.GetTokenControllerName(Request);
            }

            if (!Request.Headers.ContainsKey(headerName))
            {
                return AuthenticateResult.NoResult();
            }


            var controller=await _tokenControllerRepositoryCacheProxy.QueryByName(tokenControllerName);

            if (controller==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundTokenControllerByName,
                    DefaultFormatting = "找不到名称为{0}的令牌控制器",
                    ReplaceParameters = new List<object>() { tokenControllerName}
                };

                throw new UtilityException((int)Errors.NotFoundTokenControllerByName, fragment);
            }

            var token = Request.Headers[headerName][0];
            var validateClaim = await controller.Validate(token);
            if (validateClaim != null)
            {
                return AuthenticateResult.Success(new AuthenticationTicket(validateClaim, Scheme.Name));
            }
            else
            {
                return AuthenticateResult.NoResult();
            }
        }
    }
}
