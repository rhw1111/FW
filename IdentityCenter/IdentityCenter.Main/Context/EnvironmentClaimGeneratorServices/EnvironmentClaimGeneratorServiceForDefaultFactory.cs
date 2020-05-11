using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.Context.EnvironmentClaimGeneratorServices;
using MSLibrary.DI;


namespace IdentityCenter.Main.Context.EnvironmentClaimGeneratorServices
{
    [Injection(InterfaceType = typeof(EnvironmentClaimGeneratorServiceForDefaultFactory), Scope = InjectionScope.Singleton)]
    public class EnvironmentClaimGeneratorServiceForDefaultFactory : IFactory<IEnvironmentClaimGeneratorService>
    {
        private readonly EnvironmentClaimGeneratorServiceForDefault _environmentClaimGeneratorServiceForDefault;

        public EnvironmentClaimGeneratorServiceForDefaultFactory(EnvironmentClaimGeneratorServiceForDefault environmentClaimGeneratorServiceForDefault)
        {
            _environmentClaimGeneratorServiceForDefault = environmentClaimGeneratorServiceForDefault;
        }
        public IEnvironmentClaimGeneratorService Create()
        {
            return _environmentClaimGeneratorServiceForDefault;
        }
    }
}
