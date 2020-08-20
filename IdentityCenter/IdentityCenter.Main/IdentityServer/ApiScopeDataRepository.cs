using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using IdentityCenter.Main.IdentityServer.DAL;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IApiScopeDataRepository), Scope = InjectionScope.Singleton)]
    public class ApiScopeDataRepository : IApiScopeDataRepository
    {
        private readonly IApiScopeDataStore _apiScopeDataStore;

        public ApiScopeDataRepository(IApiScopeDataStore apiScopeDataStore)
        {
            _apiScopeDataStore = apiScopeDataStore;
        }

        public async Task<IList<ApiScopeData>> QueryAllEnabled(CancellationToken cancellationToken = default)
        {
            return await _apiScopeDataStore.QueryAllEnabled(cancellationToken);
        }

        public async Task<IList<ApiScopeData>> QueryEnabled(IList<string> names, CancellationToken cancellationToken = default)
        {
            return await _apiScopeDataStore.QueryEnabled(names, cancellationToken);
        }

        public async Task<ApiScopeData?> QueryEnabled(string name, CancellationToken cancellationToken = default)
        {
            return await _apiScopeDataStore.QueryEnabled(name, cancellationToken);
        }
    }
}
