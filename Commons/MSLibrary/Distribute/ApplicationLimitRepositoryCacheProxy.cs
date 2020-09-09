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
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj= await _applicationLimitRepository.QueryByName(name);
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

        public  ApplicationLimit QueryByNameSync(string name)
        {
            return _kvcacheVisitor.GetSync(
               (k) =>
               {
                   var obj= _applicationLimitRepository.QueryByNameSync(name);
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
