using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Context
{
    /// <summary>
    /// 基于声明的上下文生成器仓储
    /// </summary>
    public interface IClaimContextGeneratorRepository
    {
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        Task<ClaimContextGenerator?> QueryByName(string name);
    }
}
