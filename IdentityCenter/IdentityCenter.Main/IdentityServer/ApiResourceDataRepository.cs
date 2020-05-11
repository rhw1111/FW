using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using IdentityCenter.Main.IdentityServer.DAL;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IApiResourceDataRepository), Scope = InjectionScope.Singleton)]
    public class ApiResourceDataRepository : IApiResourceDataRepository
    {
        private readonly IApiResourceDataStore _apiResourceDataStore;

        public ApiResourceDataRepository(IApiResourceDataStore apiResourceDataStore)
        {
            _apiResourceDataStore = apiResourceDataStore;
        }

        public async Task<IList<ApiResourceData>> QueryAllEnabled(CancellationToken cancellationToken = default)
        {
            return await _apiResourceDataStore.QueryAllEnabled(cancellationToken);
        }

        public async Task<IList<ApiResourceData>> QueryEnabled(IList<string> names, CancellationToken cancellationToken = default)
        {
            return await _apiResourceDataStore.QueryEnabled(names, cancellationToken);
        }

        public async Task<ApiResourceData?> QueryEnabled(string name, CancellationToken cancellationToken = default)
        {
            return await _apiResourceDataStore.QueryEnabled(name, cancellationToken);
        }
    }
}
