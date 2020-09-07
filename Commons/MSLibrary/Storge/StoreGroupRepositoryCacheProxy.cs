using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Storge
{
    [Injection(InterfaceType = typeof(IStoreGroupRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class StoreGroupRepositoryCacheProxy : IStoreGroupRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
             Name= "_StoreGroupRepository"
        };
       

        private IStoreGroupRepository _storeGroupRepository;

        public StoreGroupRepositoryCacheProxy(IStoreGroupRepository storeGroupRepository)
        {
            _storeGroupRepository = storeGroupRepository;

            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;


        public async Task<StoreGroup> QueryByName(string name)
        {
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj= await _storeGroupRepository.QueryByName(name);
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
