using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.Context;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Context
{
    [Injection(InterfaceType = typeof(IClaimContextGeneratorRepository), Scope = InjectionScope.Singleton)]
    public class ClaimContextGeneratorOverrideRepository : IClaimContextGeneratorRepository
    {
        private static Dictionary<string, ClaimContextGenerator> _datas = new Dictionary<string, ClaimContextGenerator>()
        {
            {
                ClientClaimContextGeneratorNames.Default,
                new ClaimContextGenerator()
                {
                    ID=Guid.NewGuid(),
                  Name=ClientClaimContextGeneratorNames.Default,
                   Type=ClientClaimContextGeneratorTypes.Default
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
