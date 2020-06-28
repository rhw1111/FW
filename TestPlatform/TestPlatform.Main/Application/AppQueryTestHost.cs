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

        public async Task<List<TestHostViewData>> GetHosts(CancellationToken cancellationToken = default)
        {
            List<TestHostViewData> result = new List<TestHostViewData>();
            var queryResult = _testHostRepository.GetHosts(cancellationToken);
            await foreach (var item in queryResult)
            {
                result.Add(
                    new TestHostViewData()
                    {
                        ID = item.ID,
                        Address = item.Address,
                        SSHEndpoint = item.SSHEndpoint,
                        SSHEndpointID = item.SSHEndpointID,
                        CreateTime = item.CreateTime.ToCurrentUserTimeZone(),
                        ModifyTime = item.ModifyTime.ToCurrentUserTimeZone()
                    }
                    );
            }
            return result;
        }
    }
}
