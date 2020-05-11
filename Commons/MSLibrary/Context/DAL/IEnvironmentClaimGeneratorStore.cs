using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Context.DAL
{
    /// <summary>
    /// 环境声明生成器数据操作
    /// </summary>
    public interface IEnvironmentClaimGeneratorStore
    {
        Task Add(EnvironmentClaimGenerator generator);
        Task Update(EnvironmentClaimGenerator generator);
        Task Delete(Guid id);

        Task<EnvironmentClaimGenerator> QueryByName(string name);
        Task<EnvironmentClaimGenerator> QueryByID(Guid id);

        Task<QueryResult<EnvironmentClaimGenerator>> QueryByPage(string name, int page, int pageSize);
    }
}
