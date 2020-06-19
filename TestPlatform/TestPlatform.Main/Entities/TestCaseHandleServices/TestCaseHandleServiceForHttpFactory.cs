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
    [Injection(InterfaceType = typeof(TestCaseHandleServiceForHttpFactory), Scope = InjectionScope.Singleton)]
    public class TestCaseHandleServiceForHttpFactory : IFactory<ITestCaseHandleService>
    {
        private readonly TestCaseHandleServiceForHttp _testCaseHandleServiceForHttp;

        public TestCaseHandleServiceForHttpFactory(TestCaseHandleServiceForHttp testCaseHandleServiceForHttp)
        {
            _testCaseHandleServiceForHttp = testCaseHandleServiceForHttp;
        }

        public ITestCaseHandleService Create()
        {
            return _testCaseHandleServiceForHttp;
        }
    }
}
