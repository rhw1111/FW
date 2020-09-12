using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache.DAL;
using System.Diagnostics.Contracts;

namespace MSLibrary.Cache
{
    [Injection(InterfaceType = typeof(ICacheVersionRepository), Scope = InjectionScope.Singleton)]
    public class CacheVersionRepository : ICacheVersionRepository
    {
        private readonly ICacheVersionStore _cacheVersionStore;

        public CacheVersionRepository(ICacheVersionStore cacheVersionStore)
        {
            _cacheVersionStore = cacheVersionStore;
        }

        public async Task<CacheVersion?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            return await _cacheVersionStore.QueryByID(id, cancellationToken);
        }

        public async Task<CacheVersion?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _cacheVersionStore.QueryByName(name, cancellationToken);
        }

        public async Task<QueryResult<CacheVersion>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _cacheVersionStore.QueryByPage(matchName,page,pageSize, cancellationToken);
        }
    }
}
