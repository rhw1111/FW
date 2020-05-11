using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Context.DAL
{
    /// <summary>
    /// Http声明生成器数据操作
    /// </summary>
    public interface IHttpClaimGeneratorStore
    {
        Task Add(HttpClaimGenerator generator);
        Task Update(HttpClaimGenerator generator);
        Task Delete(Guid id);

        Task<HttpClaimGenerator> QueryByName(string name);
        Task<HttpClaimGenerator> QueryByID(Guid id);

        Task<QueryResult<HttpClaimGenerator>> QueryByPage(string name, int page, int pageSize);
    }
}
