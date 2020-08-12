using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Schedule;

namespace FW.TestPlatform.Main.Schedule.Actions
{
    [Injection(InterfaceType = typeof(ScheduleActionServiceForNetGatewayFactory), Scope = InjectionScope.Singleton)]
    public class ScheduleActionServiceForNetGatewayFactory : IFactory<IScheduleActionService>
    {
        private readonly ScheduleActionServiceForNetGateway _scheduleActionServiceForTest;

        public ScheduleActionServiceForNetGatewayFactory(ScheduleActionServiceForNetGateway scheduleActionServiceForTest)
        {
            _scheduleActionServiceForTest = scheduleActionServiceForTest;
        }
        public IScheduleActionService Create()
        {
            return _scheduleActionServiceForTest;
        }
    }
}
