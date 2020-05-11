using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Security
{
    [Injection(InterfaceType = typeof(ISecurityVaultEndpointRepository), Scope = InjectionScope.Singleton)]
    public class SecurityVaultEndpointRepository : ISecurityVaultEndpointRepository
    {
        public static IDictionary<string, SecurityVaultEndpoint> Datas { get; } = new Dictionary<string, SecurityVaultEndpoint>();

        public async Task<SecurityVaultEndpoint> QueryByName(string name)
        {
           if (Datas.TryGetValue(name, out SecurityVaultEndpoint endpoint))
            {
                return await Task.FromResult(endpoint);
            }
            return null;
        }
    }
}
