using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MSLibrary;
using MSLibrary.LanguageTranslate;
using MSLibrary.AspNet;
using FW.TestPlatform.Main.Configuration;

namespace FW.TestPlatform.Main.AspNet.AuthenticationHandlers
{
    public class DefaultAuthenticationHandler : AuthenticationHandler<DefaultOptions>
    {
        private const int _defaultLcid = 2052;
        private const int _defaultTimezoneOffset = -480;

        private readonly ISystemConfigurationService _systemConfigurationService;
        public DefaultAuthenticationHandler(IOptionsMonitor<DefaultOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ISystemConfigurationService systemConfigurationService) : base(options, logger, encoder, clock)
        {
            _systemConfigurationService = systemConfigurationService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var defaultUserID = await _systemConfigurationService.GetDefaultUserIDAsync();

            ClaimsIdentity identity = new ClaimsIdentity(ClaimsTypes.User);
            identity.AddClaim(new Claim(ClaimsTypes.UserID, defaultUserID.ToString()));
            identity.AddClaim(new Claim(ClaimsTypes.Lcid, _defaultLcid.ToString()));
            identity.AddClaim(new Claim(ClaimsTypes.TimezoneOffset, _defaultTimezoneOffset.ToString()));

            var principal = new ClaimsPrincipal(identity);

            Context.User = principal;

            return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));

        }
    }
}
