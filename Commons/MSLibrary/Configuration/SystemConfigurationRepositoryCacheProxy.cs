using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Configuration
{
    [Injection(InterfaceType = typeof(ISystemConfigurationRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class SystemConfigurationRepositoryCacheProxy : ISystemConfigurationRepositoryCacheProxy
    {

        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_SystemConfigurationRepository",
            ExpireSeconds = 600,
            MaxLength = 500
        };


        private ISystemConfigurationRepository _systemConfigurationRepository;

        public SystemConfigurationRepositoryCacheProxy(ISystemConfigurationRepository systemConfigurationRepository)
        {
            _systemConfigurationRepository= systemConfigurationRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public SystemConfiguration QueryByName(string name)
        {
            return _kvcacheVisitor.GetSync(
                (k)=>
                {
                    return _systemConfigurationRepository.QueryByName(name);
                },name
                );
        }
    }
}
