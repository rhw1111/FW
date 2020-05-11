using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Schedule.ScheduleActionInitGeneratorServices
{
    [Injection(InterfaceType = typeof(ScheduleActionInitGeneratorForDefaultFactory), Scope = InjectionScope.Singleton)]
    public class ScheduleActionInitGeneratorForDefaultFactory : IFactory<IScheduleActionInitGeneratorService>
    {
        private ScheduleActionInitGeneratorServiceForDefault _scheduleActionInitGeneratorServiceForDefault;
        public ScheduleActionInitGeneratorForDefaultFactory(ScheduleActionInitGeneratorServiceForDefault scheduleActionInitGeneratorServiceForDefault)
        {
            _scheduleActionInitGeneratorServiceForDefault = scheduleActionInitGeneratorServiceForDefault;
        }
        public IScheduleActionInitGeneratorService Create()
        {
            return _scheduleActionInitGeneratorServiceForDefault;
        }
    }
}
