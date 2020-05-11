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
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _hashGroupRepository.QueryById(id);
                }, id
                );
        }

        public async Task<HashGroup> QueryByName(string name)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _hashGroupRepository.QueryByName(name);
                }, name
                );
        }

        public HashGroup QueryByNameSync(string name)
        {
            return _kvcacheVisitor.GetSync(
                (k) =>
                {
                    return  _hashGroupRepository.QueryByNameSync(name);
                }, name
                );
        }
    }
}
