using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using MSLibrary.CommandLine.SSH;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQuerySingleTestHost), Scope = InjectionScope.Singleton)]
    public class AppQuerySingleTestHost : IAppQuerySingleTestHost
    {
        private readonly ITestHostRepository _testHostRepository;
        public AppQuerySingleTestHost(ITestHostRepository testHostRepository)
        {
            _testHostRepository = testHostRepository;
        }
        public async Task<TestHostViewData> Do(Guid id, CancellationToken cancellationToken = default)
        {
            var testHost = await _testHostRepository.QueryByID(id, cancellationToken);
            if (testHost == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestHostByID,
                    DefaultFormatting = "找不到id为{0}的测试主机",
                    ReplaceParameters = new List<object>() { id.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestHostByID, fragment, 1, 0);
            }

            return new TestHostViewData() { 
                ID = testHost.ID,
                Address = testHost.Address,
                SSHEndpointID = testHost.SSHEndpointID,
                SSHEndpointName = testHost.SSHEndpoint.Name,
                CreateTime = testHost.CreateTime,
                ModifyTime = testHost.ModifyTime
            };
        }
    }
}
