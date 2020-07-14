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
    [Injection(InterfaceType = typeof(IAppQueryTestHostByPage), Scope = InjectionScope.Singleton)]
    public class AppQueryTestHostByPage : IAppQueryTestHostByPage
    {
        private readonly ITestHostRepository _testHostRepository;
        public AppQueryTestHostByPage(ITestHostRepository testHostRepository)
        {
            _testHostRepository = testHostRepository;
        }
        public async Task<QueryResult<TestHostViewData>> Do(string matchAddress, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TestHostViewData> result = new QueryResult<TestHostViewData>();
            var queryResult = await _testHostRepository.QueryByPage(matchAddress, page, pageSize , cancellationToken);
            result.CurrentPage = queryResult.CurrentPage;
            result.TotalCount = queryResult.TotalCount;
            if (queryResult.Results != null && queryResult.Results.Count > 0)
            {
                foreach (var item in queryResult.Results)
                {
                    result.Results.Add(
                        new TestHostViewData()
                        {
                            ID = item.ID,
                            Address = item.Address,
                            SSHEndpointID = item.SSHEndpointID,
                            SSHEndpointName = item.SSHEndpoint.Name,
                            CreateTime = item.CreateTime.ToCurrentUserTimeZone(),
                            ModifyTime = item.ModifyTime.ToCurrentUserTimeZone()
                        }
                        );
                }
            }
            return result;
        }
    }
}
