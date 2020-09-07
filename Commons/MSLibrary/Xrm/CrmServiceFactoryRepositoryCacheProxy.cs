using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Xrm
{
    [Injection(InterfaceType = typeof(ICrmServiceFactoryRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class CrmServiceFactoryRepositoryCacheProxy : ICrmServiceFactoryRepositoryCacheProxy
    {

        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_CrmServiceFactoryRepository",
            ExpireSeconds = 600,
            MaxLength = 500
        };

        private ICrmServiceFactoryRepository _crmServiceFactoryRepository;

        public CrmServiceFactoryRepositoryCacheProxy(ICrmServiceFactoryRepository crmServiceFactoryRepository)
        {
            _crmServiceFactoryRepository = crmServiceFactoryRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public async Task<CrmServiceFactory> QueryByName(string name)
        {
            return (await _kvcacheVisitor.Get(
                async(k)=>
                {
                    var obj= await _crmServiceFactoryRepository.QueryByName(name);
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
    }
}
