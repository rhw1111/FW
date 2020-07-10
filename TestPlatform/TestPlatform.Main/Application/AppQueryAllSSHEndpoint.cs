using FW.TestPlatform.Main.DTOModel;
using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities;
using MSLibrary.CommandLine.SSH;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQueryAllSSHEndpoint), Scope = InjectionScope.Singleton)]
    public class AppQueryAllSSHEndpoint : IAppQueryAllSSHEndpoint
    {
        private readonly ISSHEndpointRepository _sSHEndpointRepository;

        public AppQueryAllSSHEndpoint(ISSHEndpointRepository sSHEndpointRepository)
        {
            _sSHEndpointRepository = sSHEndpointRepository;
        }

        public async Task<List<SSHEndPointViewData>> Do(CancellationToken cancellationToken = default)
        {
            List<SSHEndPointViewData> result = new List<SSHEndPointViewData>();
            var queryResult = _sSHEndpointRepository.GetSSHEndpoints(cancellationToken);
            await foreach (var item in queryResult)
            {
                result.Add(
                    new SSHEndPointViewData()
                    {
                        ID = item.ID,
                        Type = item.Type,
                        Configuration = item.Configuration,
                        Name = item.Configuration
                    }
                    );
            }
            return result;
        }
    }
}
