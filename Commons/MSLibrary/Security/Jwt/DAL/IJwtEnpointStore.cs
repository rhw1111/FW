using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.Jwt.DAL
{
    /// <summary>
    /// Jwt终结点数据操作
    /// </summary>
    public interface IJwtEnpointStore
    {
        Task Add(JwtEnpoint endpoint);
        Task Updtae(JwtEnpoint endpoint);
        Task Delete(Guid id);

        Task<JwtEnpoint> QueryByID(Guid id);

        Task<JwtEnpoint> QueryByName(string name);

        Task<QueryResult<JwtEnpoint>> QueryByPage(string name, int page, int pageSize);
    }
}
