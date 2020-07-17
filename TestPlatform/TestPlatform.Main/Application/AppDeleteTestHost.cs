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
    [Injection(InterfaceType = typeof(IAppDeleteTestHost), Scope = InjectionScope.Singleton)]
    public class AppDeleteTestHost : IAppDeleteTestHost
    {
        private readonly ITestHostRepository _testHostRepository;
        public AppDeleteTestHost(ITestHostRepository testHostRepository)
        {
            _testHostRepository = testHostRepository;
        }
        public async Task Do(Guid id, CancellationToken cancellationToken = default)
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
            bool result = await testHost.IsUsedByTestHostsOrSlaves(cancellationToken);
            if(!result)
                await testHost.Delete(cancellationToken);
        }
    }
}
