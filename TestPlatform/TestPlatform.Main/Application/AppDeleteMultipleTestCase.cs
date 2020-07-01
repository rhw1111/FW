using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using FW.TestPlatform.Main.Configuration;
using System.Linq;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppDeleteMultipleTestCase), Scope = InjectionScope.Singleton)]
    public class AppDeleteMultipleTestCase : IAppDeleteMultipleTestCase
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppDeleteMultipleTestCase(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task Do(List<Guid> list, CancellationToken cancellationToken = default)
        {
            await _testCaseRepository.DeleteMutiple(list, cancellationToken);
        }

    }
}
