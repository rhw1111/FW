using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Schedule
{
    [Injection(InterfaceType = typeof(IScheduleActionRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class ScheduleActionRepositoryCacheProxy : IScheduleActionRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_ScheduleActionRepository",
            ExpireSeconds = -1,
            MaxLength = 1000
        };

        private IScheduleActionRepository _scheduleActionRepository;

        private KVCacheVisitor _kvcacheVisitor;

        public ScheduleActionRepositoryCacheProxy(IScheduleActionRepository scheduleActionRepository)
        {
            _scheduleActionRepository = scheduleActionRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }



        public async Task<ScheduleAction> QueryByName(string name)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _scheduleActionRepository.QueryByName(name);
                },
                name
                );
        }
    }
}
