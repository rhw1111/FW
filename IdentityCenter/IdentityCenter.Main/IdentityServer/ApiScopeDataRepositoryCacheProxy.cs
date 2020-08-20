using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IApiScopeDataRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class ApiScopeDataRepositoryCacheProxy : IApiScopeDataRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_ApiScopeDataRepository",
            ExpireSeconds = -1,
            MaxLength = 5
        };

        private IApiScopeDataRepository _apiScopeDataRepository;

        public ApiScopeDataRepositoryCacheProxy(IApiScopeDataRepository apiScopeDataRepository)
        {
            _apiScopeDataRepository = apiScopeDataRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;


        public async Task<IList<ApiScopeData>> QueryAllEnabled(CancellationToken cancellationToken = default)
        {
            var scopeList = await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _apiScopeDataRepository.QueryAllEnabled(cancellationToken);
                },
                "All"
                );

            return scopeList;
        }

        public async Task<IList<ApiScopeData>> QueryEnabled(IList<string> names, CancellationToken cancellationToken = default)
        {
            var scopeList = await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _apiScopeDataRepository.QueryAllEnabled(cancellationToken);
                },
                "All"
                );

            var result = (from item in scopeList
                          where names.Contains(item.Name)
                          select item).ToList();
            return result;
        }

        public async Task<ApiScopeData?> QueryEnabled(string name, CancellationToken cancellationToken = default)
        {
            var scopeList = await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _apiScopeDataRepository.QueryAllEnabled(cancellationToken);
                },
                "All"
                );

            var result = (from item in scopeList
                          where item.Name == name
                          select item).FirstOrDefault();
            return result;
        }
    }
}
