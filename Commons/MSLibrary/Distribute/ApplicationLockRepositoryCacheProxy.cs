using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Distribute
{
    [Injection(InterfaceType = typeof(IApplicationLockRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class ApplicationLockRepositoryCacheProxy : IApplicationLockRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_ApplicationLockRepository",
            ExpireSeconds = -1,
            MaxLength = 1000
        };

        private IApplicationLockRepository _applicationLockRepository;

        private KVCacheVisitor _kvcacheVisitor;

        public ApplicationLockRepositoryCacheProxy(IApplicationLockRepository applicationLockRepository)
        {
            _applicationLockRepository = applicationLockRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }


        public async Task<ApplicationLock> QueryByName(string name)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _applicationLockRepository.QueryByName(name);
                },
                name
                );
        }

        public ApplicationLock QueryByNameSync(string name)
        {
            return _kvcacheVisitor.GetSync(
               (k) =>
               {
                   return _applicationLockRepository.QueryByNameSync(name);
               },
              name
              );
        }
    }
}
