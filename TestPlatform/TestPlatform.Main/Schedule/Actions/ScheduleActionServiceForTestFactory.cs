using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Schedule;

namespace FW.TestPlatform.Main.Schedule.Actions
{
    [Injection(InterfaceType = typeof(ScheduleActionServiceForTestFactory), Scope = InjectionScope.Singleton)]
    public class ScheduleActionServiceForTestFactory : IFactory<IScheduleActionService>
    {
        private readonly ScheduleActionServiceForTest _scheduleActionServiceForTest;

        public ScheduleActionServiceForTestFactory(ScheduleActionServiceForTest scheduleActionServiceForTest)
        {
            _scheduleActionServiceForTest = scheduleActionServiceForTest;
        }
        public IScheduleActionService Create()
        {
            return _scheduleActionServiceForTest;
        }
    }
}
