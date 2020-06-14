using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Distribute
{
    [Injection(InterfaceType = typeof(IApplicationLimitRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class ApplicationLimitRepositoryCacheProxy : IApplicationLimitRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_ApplicationLimitRepository",
            ExpireSeconds = -1,
            MaxLength = 1000
        };

        private IApplicationLimitRepository _applicationLimitRepository;

        private KVCacheVisitor _kvcacheVisitor;

        public ApplicationLimitRepositoryCacheProxy(IApplicationLimitRepository applicationLimitRepository)
        {
            _applicationLimitRepository = applicationLimitRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        public async Task<ApplicationLimit> QueryByName(string name)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _applicationLimitRepository.QueryByName(name);
                },
                name
                );
        }

        public  ApplicationLimit QueryByNameSync(string name)
        {
            return _kvcacheVisitor.GetSync(
               (k) =>
               {
                   return _applicationLimitRepository.QueryByNameSync(name);
               },
              name
              );
        }
    }
}
