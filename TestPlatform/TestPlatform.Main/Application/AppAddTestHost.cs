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
    [Injection(InterfaceType = typeof(IAppAddTestHost), Scope = InjectionScope.Singleton)]
    public class AppAddTestHost : IAppAddTestHost
    {
        private readonly ITestHostRepository _testHostRepository;
        public AppAddTestHost(ITestHostRepository testHostRepository)
        {
            _testHostRepository = testHostRepository;
        }
        public async Task<TestHostViewData> Do(TestHostAddModel model, CancellationToken cancellationToken = default)
        {
            //检查是否有名称重复的
            var newId = await _testHostRepository.QueryByNameNoLock(model.Address, cancellationToken);
            if (newId != null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.ExistTestHostByName,
                    DefaultFormatting = "已经存在地址为{0}的主机",
                    ReplaceParameters = new List<object>() { model.Address }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.ExistTestHostByName, fragment, 1, 0);
            }
            TestHost testHost = new TestHost()
            {
                Address = model.Address,
                SSHEndpointID = model.SSHEndpointID
            };
            await testHost.Add(cancellationToken);

            return new TestHostViewData()
            {
                ID = testHost.ID,
                Address = testHost.Address,
                SSHEndpointID = testHost.SSHEndpointID,
                CreateTime = testHost.CreateTime
            };
        }
    }
}
