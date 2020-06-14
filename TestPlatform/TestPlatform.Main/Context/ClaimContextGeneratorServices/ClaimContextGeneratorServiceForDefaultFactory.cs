using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context.ClaimContextGeneratorServices;

namespace FW.TestPlatform.Main.Context.ClaimContextGeneratorServices
{
    [Injection(InterfaceType = typeof(ClaimContextGeneratorServiceForDefaultFactory), Scope = InjectionScope.Singleton)]
    public class ClaimContextGeneratorServiceForDefaultFactory : IFactory<IClaimContextGeneratorService>
    {
        private readonly ClaimContextGeneratorServiceForDefault _claimContextGeneratorServiceForDefault;

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
