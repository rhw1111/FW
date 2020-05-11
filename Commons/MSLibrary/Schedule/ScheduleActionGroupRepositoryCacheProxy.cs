using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Schedule
{
    [Injection(InterfaceType = typeof(IScheduleActionGroupRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class ScheduleActionGroupRepositoryCacheProxy : IScheduleActionGroupRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_ScheduleActionGroupRepository",
            ExpireSeconds = -1,
            MaxLength = 1000
        };

        private IScheduleActionGroupRepository _scheduleActionGroupRepository;

        private KVCacheVisitor _kvcacheVisitor;

        public ScheduleActionGroupRepositoryCacheProxy(IScheduleActionGroupRepository scheduleActionGroupRepository)
        {
            _scheduleActionGroupRepository = scheduleActionGroupRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }


        public async Task<ScheduleActionGroup> QueryByName(string name)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _scheduleActionGroupRepository.QueryByName(name);
                },
                name
                );
        }
    }
}
