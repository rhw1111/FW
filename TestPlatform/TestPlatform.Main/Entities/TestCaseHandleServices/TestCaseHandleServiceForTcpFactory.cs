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
    [Injection(InterfaceType = typeof(TestCaseHandleServiceForTcpFactory), Scope = InjectionScope.Singleton)]
    public class TestCaseHandleServiceForTcpFactory : IFactory<ITestCaseHandleService>
    {
        private readonly TestCaseHandleServiceForTcp _testCaseHandleServiceForTcp;

        public TestCaseHandleServiceForTcpFactory(TestCaseHandleServiceForTcp testCaseHandleServiceForTcp)
        {
            _testCaseHandleServiceForTcp = testCaseHandleServiceForTcp;
        }

        public ITestCaseHandleService Create()
        {
            return _testCaseHandleServiceForTcp;
        }
    }
}
