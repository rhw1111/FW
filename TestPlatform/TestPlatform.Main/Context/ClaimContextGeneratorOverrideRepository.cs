using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Context;

namespace FW.TestPlatform.Main.Context
{
    [Injection(InterfaceType = typeof(IClaimContextGeneratorRepository), Scope = InjectionScope.Singleton)]
    public class ClaimContextGeneratorOverrideRepository : IClaimContextGeneratorRepository
    {
        private static readonly Dictionary<string, ClaimContextGenerator> _datas = new Dictionary<string, ClaimContextGenerator>()
        {
            {
                ClaimContextGeneratorNames.Default,
                new ClaimContextGenerator()
                {
                    ID=Guid.NewGuid(),
                  Name=ClaimContextGeneratorNames.Default,
                   Type=ClaimContextGeneratorTypes.Default
                }
            }

        };
        public async Task<ClaimContextGenerator?> QueryByName(string name)
        {
            if (_datas.TryGetValue(name, out ClaimContextGenerator? generator))
            {
                return await Task.FromResult(generator);
            }

            return null;
        }
    }
}
