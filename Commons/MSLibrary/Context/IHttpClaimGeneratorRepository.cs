using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Context
{
    /// <summary>
    /// 基于Http的声明生成器仓储
    /// </summary>
    public interface IHttpClaimGeneratorRepository
    {
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<HttpClaimGenerator> QueryByName(string name);
    }
}
