using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Context.DAL
{
    /// <summary>
    /// 声明上下文生成器数据操作
    /// </summary>
    public interface IClaimContextGeneratorStore
    {
        Task Add(ClaimContextGenerator generator);
        Task Update(ClaimContextGenerator generator);
        Task Delete(Guid id);

        Task<ClaimContextGenerator> QueryByName(string name);
        Task<ClaimContextGenerator> QueryByID(Guid id);

        Task<QueryResult<ClaimContextGenerator>> QueryByPage(string name, int page, int pageSize);

    }
}
