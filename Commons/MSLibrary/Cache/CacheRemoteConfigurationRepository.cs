using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache.DAL;

namespace MSLibrary.Cache
{
    [Injection(InterfaceType = typeof(ICacheRemoteConfigurationRepository), Scope = InjectionScope.Singleton)]
    public class CacheRemoteConfigurationRepository : ICacheRemoteConfigurationRepository
    {
        private ICacheRemoteConfigurationStore _cacheRemoteConfigurationStore;

        public CacheRemoteConfigurationRepository(ICacheRemoteConfigurationStore cacheRemoteConfigurationStore)
        {
            _cacheRemoteConfigurationStore = cacheRemoteConfigurationStore;
        }

        public async Task<CacheRemoteConfiguration> QueryById(Guid id)
        {
            return await _cacheRemoteConfigurationStore.QueryById(id);
        }

        public async Task<CacheRemoteConfiguration> QueryByName(string name)
        {
            return await _cacheRemoteConfigurationStore.QueryByName(name);
        }

        public async Task<QueryResult<CacheRemoteConfiguration>> QueryByName(string name, int page, int pageSize)
        {
            
            return await _cacheRemoteConfigurationStore.QueryByName(name,page,pageSize);
        }
    }
}
