using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime;
using Newtonsoft.Json.Linq;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Entities.TestCaseHandleServices
{
    [Injection(InterfaceType = typeof(TestCaseHandleServiceForWebSocketFactory), Scope = InjectionScope.Singleton)]
    public class TestCaseHandleServiceForWebSocketFactory : IFactory<ITestCaseHandleService>
    {
        private readonly TestCaseHandleServiceForWebSocket _testCaseHandleServiceForWebSocket;

        public TestCaseHandleServiceForWebSocketFactory(TestCaseHandleServiceForWebSocket testCaseHandleServiceForWebSocket)
        {
            _testCaseHandleServiceForWebSocket = testCaseHandleServiceForWebSocket;
        }

        public ITestCaseHandleService Create()
        {
            return _testCaseHandleServiceForWebSocket;
        }
    }
}
