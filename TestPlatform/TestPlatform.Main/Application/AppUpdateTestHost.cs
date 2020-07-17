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
    [Injection(InterfaceType = typeof(IAppUpdateTestHost), Scope = InjectionScope.Singleton)]
    public class AppUpdateTestHost : IAppUpdateTestHost
    {
        private readonly ITestHostRepository _testHostRepository;
        public AppUpdateTestHost(ITestHostRepository testHostRepository)
        {
            _testHostRepository = testHostRepository;
        }
        public async Task<TestHostViewData> Do(TestHostUpdateModel model, CancellationToken cancellationToken = default)
        {
            //检查是否有名称重复的
            var testHost = await _testHostRepository.QueryByID(model.ID, cancellationToken);
            if (testHost == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestHostByID,
                    DefaultFormatting = "找不到id为{0}的测试主机",
                    ReplaceParameters = new List<object>() { model.ID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestHostByID, fragment, 1, 0);
            }
            //检查是否有名称重复的
            var newId = await _testHostRepository.QueryByNameNoLock(model.Address, cancellationToken);
            if (newId != null && newId != model.ID)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.ExistTestHostByName,
                    DefaultFormatting = "已经存在地址为{0}的主机",
                    ReplaceParameters = new List<object>() { model.Address }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.ExistTestHostByName, fragment, 1, 0);
            }
            testHost.SSHEndpointID = model.SSHEndpointID;
            testHost.Address = model.Address;
            await testHost.Update(cancellationToken);

            return new TestHostViewData()
            {
                ID = testHost.ID,
                Address = testHost.Address,
                SSHEndpointID = testHost.SSHEndpointID,
                CreateTime = testHost.CreateTime,
                ModifyTime = testHost.ModifyTime
            };
        }
    }
}
