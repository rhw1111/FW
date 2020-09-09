using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;


namespace MSLibrary.Collections.Hash
{
    [Injection(InterfaceType = typeof(IHashGroupRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class HashGroupRepositoryCacheProxy : IHashGroupRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_HashGroupRepository",
            ExpireSeconds = -1,
            MaxLength = 1000
        };


        private IHashGroupRepository _hashGroupRepository;

        public HashGroupRepositoryCacheProxy(IHashGroupRepository hashGroupRepository)
        {
            _hashGroupRepository = hashGroupRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;



        public async Task<HashGroup> QueryById(Guid id)
        {
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj= await _hashGroupRepository.QueryById(id);
                    if (obj==null)
                    {
                        return (obj, false);
                    }
                    else
                    {
                        return (obj, true);
                    }
                }, id
                )).Item1;
        }

        public async Task<HashGroup> QueryByName(string name)
        {
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj= await _hashGroupRepository.QueryByName(name);
                    if (obj == null)
                    {
                        return (obj, false);
                    }
                    else
                    {
                        return (obj, true);
                    }
                }, name
                )).Item1;
        }

        public HashGroup QueryByNameSync(string name)
        {
            return _kvcacheVisitor.GetSync(
                (k) =>
                {
                    var obj = _hashGroupRepository.QueryByNameSync(name);
                    if (obj == null)
                    {
                        return (obj, false);
                    }
                    else
                    {
                        return (obj, true);
                    }
                }, name
                ).Item1;
        }
    }
}
