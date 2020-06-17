using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Context;

namespace FW.TestPlatform.Main.Context
{
    [Injection(InterfaceType = typeof(IEnvironmentClaimGeneratorRepository), Scope = InjectionScope.Singleton)]
    public class EnvironmentClaimGeneratorOverrideRepository : IEnvironmentClaimGeneratorRepository
    {
        private readonly static Dictionary<string, EnvironmentClaimGenerator> _datas = new Dictionary<string, EnvironmentClaimGenerator>()
        {
            {
                EnvironmentClaimGeneratorNames.Default,
                new EnvironmentClaimGenerator()
                {
                 ID=Guid.NewGuid(),
                  Name=EnvironmentClaimGeneratorNames.Default,
                   Type=EnvironmentClaimGeneratorTypes.Default
                }
            }

        };

        public async Task<EnvironmentClaimGenerator?> QueryByName(string name)
        {
            if (_datas.TryGetValue(name, out EnvironmentClaimGenerator? generator))
            {
                return await Task.FromResult(generator);
            }

            return null;
        }
    }
}
