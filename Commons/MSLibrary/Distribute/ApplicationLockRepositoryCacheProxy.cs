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
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj = await _applicationLockRepository.QueryByName(name);
                    if (obj == null)
                    {
                        return (obj, false);
                    }
                    else
                    {
                        return (obj, true);
                    }
                },
                name
                )).Item1;
        }

        public ApplicationLock QueryByNameSync(string name)
        {
            return _kvcacheVisitor.GetSync(
               (k) =>
               {
                   var obj = _applicationLockRepository.QueryByNameSync(name);

                   if (obj == null)
                   {
                       return (obj, false);
                   }
                   else
                   {
                       return (obj, true);
                   }
               },
              name
              ).Item1;
        }
    }
}
