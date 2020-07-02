using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.SystemToken
{
    [Injection(InterfaceType = typeof(ITokenControllerRepository), Scope = InjectionScope.Singleton)]
    public class TokenControllerRepository : ITokenControllerRepository
    {
        public static IDictionary<string, TokenController> Controllers { get; } = new Dictionary<string, TokenController>();


        public async Task<TokenController> QueryByName(string name)
        {
            if (Controllers.TryGetValue(name,out TokenController controller))
            {
                return await Task.FromResult(controller);
            }

            return null;
        }
    }
}
