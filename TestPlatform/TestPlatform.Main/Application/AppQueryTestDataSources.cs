using FW.TestPlatform.Main.DTOModel;
using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQueryTestDataSources), Scope = InjectionScope.Singleton)]
    public class AppQueryTestDataSources : IAppQueryTestDataSources
    {
        private readonly ITestDataSourceRepository _testDataSourceRepository;

        public AppQueryTestDataSources(ITestDataSourceRepository testDataSourceRepository)
        {
            _testDataSourceRepository = testDataSourceRepository;
        }
        public async Task<List<TestDataSourceNameAndIDList>> Do(CancellationToken cancellationToken = default)
        {
            List<TestDataSourceNameAndIDList> result = new List<TestDataSourceNameAndIDList>();
            var queryResult= _testDataSourceRepository.GetDataSources(cancellationToken);

            await foreach (var item in queryResult)
            {
                result.Add(
                    new TestDataSourceNameAndIDList()
                    {
                        ID = item.ID,
                        Name = item.Name
                    }
                    );
            }
            return result;
        }
    }
}
