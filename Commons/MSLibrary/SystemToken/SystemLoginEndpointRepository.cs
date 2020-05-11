using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MSLibrary.Configuration;
using MSLibrary.DI;
using MSLibrary.SystemToken.DAL;

namespace MSLibrary.SystemToken
{
    [Injection(InterfaceType = typeof(ISystemLoginEndpointRepository), Scope = InjectionScope.Singleton)]

    public class SystemLoginEndpointRepository : ISystemLoginEndpointRepository
    {
        private ISystemLoginEndpointStore _systemLoginEndpointStore;

        public SystemLoginEndpointRepository(ISystemLoginEndpointStore systemLoginEndpointStore)
        {
            _systemLoginEndpointStore = systemLoginEndpointStore;
        }

        public async Task<SystemLoginEndpoint> QueryById(Guid id)
        {
            return await _systemLoginEndpointStore.QueryById(id);
        }

        public async Task<SystemLoginEndpoint> QueryByName(string sysName)
        {
            return await _systemLoginEndpointStore.QueryByName(sysName);
        }

        public async Task<QueryResult<SystemLoginEndpoint>> QueryByPage(string name, int page, int pageSize)
        {
            return await _systemLoginEndpointStore.QueryByPage(name,page,pageSize);
        }
    }


}
