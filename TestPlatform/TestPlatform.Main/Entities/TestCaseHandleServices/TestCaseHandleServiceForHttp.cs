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
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.CommandLine.SSH;

namespace FW.TestPlatform.Main.Entities.TestCaseHandleServices
{
    [Injection(InterfaceType = typeof(TestCaseHandleServiceForHttp), Scope = InjectionScope.Singleton)]
    public class TestCaseHandleServiceForHttp : ITestCaseHandleService
    {
        public Task<string> GetMasterLog(TestHost host, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSlaveLog(TestHost host, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEngineRun(TestCase tCase, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Run(TestCase tCase, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Stop(TestCase tCase, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
