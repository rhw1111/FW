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
    [Injection(InterfaceType = typeof(IAppDeleteMultipleTestDataSource), Scope = InjectionScope.Singleton)]
    public class AppDeleteMultipleTestDataSource : IAppDeleteMultipleTestDataSource
    {
        private readonly ITestDataSourceRepository _testDataSourceRepository;
        public AppDeleteMultipleTestDataSource(ITestDataSourceRepository testDataSourceRepository)
        {
            _testDataSourceRepository = testDataSourceRepository;
        }
        public async Task Do(List<Guid> list, CancellationToken cancellationToken = default)
        {
            await _testDataSourceRepository.DeleteMutipleTestDataSource(list, cancellationToken);
        }

    }
}
