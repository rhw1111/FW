using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.Context.EnvironmentClaimGeneratorServices;
using MSLibrary.DI;
using FW.TestPlatform.Main.Configuration;

namespace FW.TestPlatform.Main.Context.EnvironmentClaimGeneratorServices
{
    [Injection(InterfaceType = typeof(EnvironmentClaimGeneratorServiceForDefault), Scope = InjectionScope.Singleton)]
    public class EnvironmentClaimGeneratorServiceForDefault : IEnvironmentClaimGeneratorService
    {
        private const int _defaultLcid = 2052;
        private const int _defaultTimezoneOffset = -480;

        private readonly ISystemConfigurationService _systemConfigurationService;

        public EnvironmentClaimGeneratorServiceForDefault(ISystemConfigurationService systemConfigurationService)
        {
            _systemConfigurationService = systemConfigurationService;
        }

        public async Task<ClaimsIdentity> Do()
        {
            var userId=await _systemConfigurationService.GetDefaultUserIDAsync();
            ClaimsIdentity identity = new ClaimsIdentity(ClaimsTypes.User);
            identity.AddClaim(new Claim(ClaimsTypes.UserID, userId.ToString()));
            identity.AddClaim(new Claim(ClaimsTypes.Lcid, _defaultLcid.ToString()));
            identity.AddClaim(new Claim(ClaimsTypes.TimezoneOffset, _defaultTimezoneOffset.ToString()));

            return identity;
        }
    }
}
