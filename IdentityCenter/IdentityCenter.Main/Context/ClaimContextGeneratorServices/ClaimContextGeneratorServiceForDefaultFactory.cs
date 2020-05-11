using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MSLibrary;
using MSLibrary.Context.ClaimContextGeneratorServices;
using MSLibrary.DI;

namespace IdentityCenter.Main.Context.ClaimContextGeneratorServices
{
    [Injection(InterfaceType = typeof(ClaimContextGeneratorServiceForDefaultFactory), Scope = InjectionScope.Singleton)]
    public class ClaimContextGeneratorServiceForDefaultFactory : IFactory<IClaimContextGeneratorService>
    {
        private ClaimContextGeneratorServiceForDefault _claimContextGeneratorServiceForDefault;

        public ClaimContextGeneratorServiceForDefaultFactory(ClaimContextGeneratorServiceForDefault claimContextGeneratorServiceForDefault)
        {
            _claimContextGeneratorServiceForDefault = claimContextGeneratorServiceForDefault;
        }
        public IClaimContextGeneratorService Create()
        {
            return _claimContextGeneratorServiceForDefault;
        }
    }
}
