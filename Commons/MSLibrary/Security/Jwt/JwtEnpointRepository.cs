using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Security.Jwt.DAL;
using MSLibrary.DI;

namespace MSLibrary.Security.Jwt
{
    [Injection(InterfaceType = typeof(IJwtEnpointRepository), Scope = InjectionScope.Singleton)]
    public class JwtEnpointRepository : IJwtEnpointRepository
    {
        private IJwtEnpointStore _jwtEnpointStore;

        public JwtEnpointRepository(IJwtEnpointStore jwtEnpointStore)
        {
            _jwtEnpointStore = jwtEnpointStore;
        }
        public async Task<JwtEnpoint> QueryByID(Guid id)
        {
            return await _jwtEnpointStore.QueryByID(id);
        }

        public async Task<JwtEnpoint> QueryByName(string name)
        {
            return await _jwtEnpointStore.QueryByName(name);
        }

        public async Task<QueryResult<JwtEnpoint>> QueryByPage(string name, int page, int pageSize)
        {
            return await _jwtEnpointStore.QueryByPage(name, page, pageSize);
        }
    }
}
