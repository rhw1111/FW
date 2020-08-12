using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Schedule;

namespace FW.TestPlatform.Main.Schedule.Actions
{
    [Injection(InterfaceType = typeof(ScheduleActionServiceForNetGateway), Scope = InjectionScope.Singleton)]
    public class ScheduleActionServiceForNetGateway : IScheduleActionService
    {
        public async Task<IScheduleActionResult> Execute(string configuration)
        {
            
            return await Task.FromResult(new ScheduleActionResultDefault());
        }
    }
}
