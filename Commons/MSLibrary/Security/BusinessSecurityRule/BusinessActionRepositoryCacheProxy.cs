using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Security.BusinessSecurityRule
{
    [Injection(InterfaceType = typeof(IBusinessActionRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class BusinessActionRepositoryCacheProxy : IBusinessActionRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_BusinessActionRepository",
            ExpireSeconds = -1,
            MaxLength = 2000
        };

        private IBusinessActionRepository _businessActionRepository;

        public BusinessActionRepositoryCacheProxy(IBusinessActionRepository businessActionRepository)
        {
            _businessActionRepository = businessActionRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;


        public async Task<BusinessAction> QueryByName(string name)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _businessActionRepository.QueryByName(name);
                }, name
                );
        }
    }
}
