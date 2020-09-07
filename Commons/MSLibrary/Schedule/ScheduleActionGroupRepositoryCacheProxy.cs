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
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj = await _scheduleActionGroupRepository.QueryByName(name);

                    if (obj == null)
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
