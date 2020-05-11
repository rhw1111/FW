using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Security.BusinessSecurityRule
{
    [Injection(InterfaceType = typeof(IBusinessActionGroupRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class BusinessActionGroupRepositoryCacheProxy : IBusinessActionGroupRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_BusinessActionGroupRepository",
            ExpireSeconds = -1,
            MaxLength = 2000
        };

        private IBusinessActionGroupRepository _businessActionGroupRepository;

        public BusinessActionGroupRepositoryCacheProxy(IBusinessActionGroupRepository businessActionGroupRepository)
        {
            _businessActionGroupRepository = businessActionGroupRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public async Task<BusinessActionGroup> QueryByName(string name)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _businessActionGroupRepository.QueryByName(name);
                }, name
                );
        }
    }
}
