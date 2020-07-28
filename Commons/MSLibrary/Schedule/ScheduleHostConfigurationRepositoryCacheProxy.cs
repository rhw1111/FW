using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.Cache;
using MSLibrary.DI;

namespace MSLibrary.Schedule
{
    [Injection(InterfaceType = typeof(IScheduleHostConfigurationRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class ScheduleHostConfigurationRepositoryCacheProxy : IScheduleHostConfigurationRepositoryCacheProxy
    {
        public static readonly KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_ScheduleHostConfigurationRepository",
            ExpireSeconds = -1,
            MaxLength = 5000
        };

        private readonly IScheduleHostConfigurationRepository _scheduleHostConfigurationRepository;

        public ScheduleHostConfigurationRepositoryCacheProxy(IScheduleHostConfigurationRepository scheduleHostConfigurationRepository)
        {
            _scheduleHostConfigurationRepository = scheduleHostConfigurationRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private readonly KVCacheVisitor _kvcacheVisitor;

        public async Task<ScheduleHostConfiguration> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _scheduleHostConfigurationRepository.QueryByName(name, cancellationToken);
                }, name
                );
        }
    }
}
