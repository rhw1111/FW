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
    [Injection(InterfaceType = typeof(TestCaseHandleServiceForJmeterFactory), Scope = InjectionScope.Singleton)]
    public class TestCaseHandleServiceForJmeterFactory : IFactory<ITestCaseHandleService>
    {
        private readonly TestCaseHandleServiceForJmeter _testCaseHandleServiceForJmeter;

        public TestCaseHandleServiceForJmeterFactory(TestCaseHandleServiceForJmeter testCaseHandleServiceForJmeter)
        {
            _testCaseHandleServiceForJmeter = testCaseHandleServiceForJmeter;
        }

        public ITestCaseHandleService Create()
        {
            return _testCaseHandleServiceForJmeter;
        }
    }
}
