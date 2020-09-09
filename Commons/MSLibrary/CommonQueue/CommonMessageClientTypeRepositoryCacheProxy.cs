using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.CommonQueue
{
    [Injection(InterfaceType = typeof(ICommonMessageClientTypeRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class CommonMessageClientTypeRepositoryCacheProxy : ICommonMessageClientTypeRepositoryCacheProxy
    {
       
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_CommonMessageClientTypeRepository",
            ExpireSeconds = 600,
            MaxLength = 500
        };


        private ICommonMessageClientTypeRepository _commonMessageClientTypeRepository;

        public CommonMessageClientTypeRepositoryCacheProxy(ICommonMessageClientTypeRepository commonMessageClientTypeRepository)
        {
            _commonMessageClientTypeRepository = commonMessageClientTypeRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;


        public async Task<CommonMessageClientType> QueryByName(string name)
        {
            return (await _kvcacheVisitor.Get(
                async(key)=>
                {
                    var obj= await _commonMessageClientTypeRepository.QueryByName(name);
                    if (obj==null)
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
