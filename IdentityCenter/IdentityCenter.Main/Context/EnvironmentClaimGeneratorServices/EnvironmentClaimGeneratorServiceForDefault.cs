using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.Context.EnvironmentClaimGeneratorServices;
using MSLibrary.DI;

namespace IdentityCenter.Main.Context.EnvironmentClaimGeneratorServices
{
    /// <summary>
    /// 默认的环境声明生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(EnvironmentClaimGeneratorServiceForDefault), Scope = InjectionScope.Singleton)]
    public class EnvironmentClaimGeneratorServiceForDefault : IEnvironmentClaimGeneratorService
    {
        private const int _defaultLcid = 2052;
        private const int _defaultTimezoneOffset = -480;

        public async Task<ClaimsIdentity> Do()
        {
            
            ClaimsIdentity identity = new ClaimsIdentity(ClaimsTypes.User);
            identity.AddClaim(new Claim(ClaimsTypes.Lcid, _defaultLcid.ToString()));
            identity.AddClaim(new Claim(ClaimsTypes.TimezoneOffset, _defaultTimezoneOffset.ToString()));

            return await Task.FromResult(identity);
        }
    }

}
