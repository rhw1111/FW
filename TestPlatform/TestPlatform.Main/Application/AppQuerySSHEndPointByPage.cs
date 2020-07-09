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
    [Injection(InterfaceType = typeof(IAppQuerySSHEndPointByPage), Scope = InjectionScope.Singleton)]
    public class AppQuerySSHEndPointByPage : IAppQuerySSHEndPointByPage
    {
        private readonly ISSHEndpointRepository _sSHEndpointRepository;

        public AppQuerySSHEndPointByPage(ISSHEndpointRepository sSHEndpointRepository)
        {
            _sSHEndpointRepository = sSHEndpointRepository;
        }

        public async Task<QueryResult<SSHEndPointViewData>> Do(string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<SSHEndPointViewData> result = new QueryResult<SSHEndPointViewData>();
            var queryResult=await _sSHEndpointRepository.QueryByPage(matchName, page, pageSize, cancellationToken);

            result.CurrentPage = queryResult.CurrentPage;
            result.TotalCount = queryResult.TotalCount;

            foreach(var item in queryResult.Results)
            {
                result.Results.Add(
                    new SSHEndPointViewData()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Type = item.Type,
                        Configuration = item.Configuration,
                        CreateTime = item.CreateTime
                    }
                    );
            }

            return result;
        }
    }
}
