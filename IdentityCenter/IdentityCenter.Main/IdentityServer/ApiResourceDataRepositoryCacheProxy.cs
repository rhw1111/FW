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
    [Injection(InterfaceType = typeof(IApiResourceDataRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class ApiResourceDataRepositoryCacheProxy : IApiResourceDataRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_ApiResourceDataRepository",
            ExpireSeconds = -1,
            MaxLength = 5
        };

        private IApiResourceDataRepository _apiResourceDataRepository;

        public ApiResourceDataRepositoryCacheProxy(IApiResourceDataRepository apiResourceDataRepository)
        {
            _apiResourceDataRepository = apiResourceDataRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public async Task<IList<ApiResourceData>> QueryAllEnabled(CancellationToken cancellationToken = default)
        {

            var resourceList=await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _apiResourceDataRepository.QueryAllEnabled(cancellationToken);
                },
                "All"
                );

            return resourceList;
        }

        public async Task<IList<ApiResourceData>> QueryEnabled(IList<string> names, CancellationToken cancellationToken = default)
        {
            var resourceList = await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _apiResourceDataRepository.QueryAllEnabled(cancellationToken);
                },
                "All"
                );

            var result = (from item in resourceList
                          where names.Contains(item.Name)
                          select item).ToList();
            return result;
        }

        public async Task<ApiResourceData?> QueryEnabled(string name, CancellationToken cancellationToken = default)
        {
            var resourceList = await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _apiResourceDataRepository.QueryAllEnabled(cancellationToken);
                },
                "All"
                );

            var result = (from item in resourceList
                          where item.Name==name
                          select item).FirstOrDefault();
            return result;
        }
    }
}
