using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.SystemToken.DAL;

namespace MSLibrary.SystemToken
{
    /// <summary>
    /// 验证终结点仓储
    /// </summary>
    [Injection(InterfaceType = typeof(IAuthorizationEndpointRepository), Scope = InjectionScope.Singleton)]
    public class AuthorizationEndpointRepository : IAuthorizationEndpointRepository
    {
        private IAuthorizationEndpointStore _authorizationEndpointStore;

        public AuthorizationEndpointRepository(IAuthorizationEndpointStore authorizationEndpointStore)
        {
            _authorizationEndpointStore = authorizationEndpointStore;
        }

        public async Task<AuthorizationEndpoint> QueryById(Guid id)
        {
            return await _authorizationEndpointStore.QueryById(id);
        }

        public async Task<AuthorizationEndpoint> QueryByName(string name)
        {
            return await _authorizationEndpointStore.QueryByName(name);
        }

        public async Task<QueryResult<AuthorizationEndpoint>> QueryByPage(string authorizationName, int page, int pageSize)
        {
            return await _authorizationEndpointStore.QueryByPage(authorizationName, page, pageSize);
        }
    }
}
