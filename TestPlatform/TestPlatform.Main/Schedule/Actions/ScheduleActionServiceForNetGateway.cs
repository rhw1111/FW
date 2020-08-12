using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Schedule;
using FW.TestPlatform.Main.NetGateway;

namespace FW.TestPlatform.Main.Schedule.Actions
{
    [Injection(InterfaceType = typeof(ScheduleActionServiceForNetGateway), Scope = InjectionScope.Singleton)]
    public class ScheduleActionServiceForNetGateway : IScheduleActionService
    {
        private readonly INetGatewayDataHandleService _netGatewayDataHandleService;

        public ScheduleActionServiceForNetGateway(INetGatewayDataHandleService netGatewayDataHandleService)
        {
            _netGatewayDataHandleService = netGatewayDataHandleService;
        }
        public async Task<IScheduleActionResult> Execute(string configuration)
        {
            var handleResult=await _netGatewayDataHandleService.Execute();
            return new ScheduleActionResult(handleResult);
        }

        private class ScheduleActionResult : IScheduleActionResult
        {
            private readonly INetGatewayDataHandleResult _netGatewayDataHandleResult;

            public ScheduleActionResult(INetGatewayDataHandleResult netGatewayDataHandleResult)
            {
                _netGatewayDataHandleResult = netGatewayDataHandleResult;
                Polling = true;
            }
            public bool Polling { get; private set; }

            public async Task Stop()
            {
                await _netGatewayDataHandleResult.Stop();
            }
        }
    }
}
