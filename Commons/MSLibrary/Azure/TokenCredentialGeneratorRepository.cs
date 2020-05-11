using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Azure.DAL;

namespace MSLibrary.Azure
{
    [Injection(InterfaceType = typeof(ITokenCredentialGeneratorRepository), Scope = InjectionScope.Singleton)]
    public class TokenCredentialGeneratorRepository : ITokenCredentialGeneratorRepository
    {
        private ITokenCredentialGeneratorStore _tokenCredentialGeneratorStore;

        public TokenCredentialGeneratorRepository(ITokenCredentialGeneratorStore tokenCredentialGeneratorStore)
        {
            _tokenCredentialGeneratorStore = tokenCredentialGeneratorStore;
        }
        public async Task<TokenCredentialGenerator> QueryByName(string name)
        {
            return await _tokenCredentialGeneratorStore.QueryByName(name);
        }
    }
}
