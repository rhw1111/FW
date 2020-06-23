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
    [Injection(InterfaceType = typeof(IAppQueryTestHost), Scope = InjectionScope.Singleton)]
    public class AppQueryTestHost : IAppQueryTestHost
    {
        private readonly ITestHostRepository _testHostRepository;

        public AppQueryTestHost(ITestHostRepository testHostRepository)
        {
            _testHostRepository = testHostRepository;
        }

        public async Task<QueryResult<TestHostViewData>> GetHosts(CancellationToken cancellationToken = default)
        {
            QueryResult<TestHostViewData> result = new QueryResult<TestHostViewData>();
            var queryResult = await _testHostRepository.GetHosts(cancellationToken);
            foreach (var item in queryResult.Results)
            {
                result.Results.Add(
                    new TestHostViewData()
                    {
                        ID = item.ID,
                        Address = item.Address,
                        SSHEndpoint = item.SSHEndpoint,
                        CreateTime = item.CreateTime.ToCurrentUserTimeZone(),
                        ModifyTime = item.ModifyTime.ToCurrentUserTimeZone()
                    }
                    );
            }

            return result;
        }
    }
}
