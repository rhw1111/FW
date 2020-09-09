using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.MessageQueue
{
    [Injection(InterfaceType = typeof(ISMessageExecuteTypeRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class SMessageExecuteTypeRepositoryCacheProxy : ISMessageExecuteTypeRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_SMessageExecuteTypeRepository",
            ExpireSeconds = -1,
            MaxLength = 5000
        };

        private ISMessageExecuteTypeRepository _sMessageExecuteTypeRepository;

        public SMessageExecuteTypeRepositoryCacheProxy(ISMessageExecuteTypeRepository sMessageExecuteTypeRepository)
        {
            _sMessageExecuteTypeRepository = sMessageExecuteTypeRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public async Task<SMessageExecuteType> QueryByName(string name)
        {
            return (await _kvcacheVisitor.Get(
                async(k)=>
                {
                    var obj= await _sMessageExecuteTypeRepository.QueryByName(name);
                    if (obj == null)
                    {
                        return (obj, false);
                    }
                    else
                    {
                        return (obj, true);
                    }
                },name
                )).Item1;
        }
    }
}
