using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.Jwt
{
    public interface IJwtEnpointRepository
    {
        Task<JwtEnpoint> QueryByID(Guid id);

        Task<JwtEnpoint> QueryByName(string name);

        Task<QueryResult<JwtEnpoint>> QueryByPage(string name, int page, int pageSize);
    }
}
