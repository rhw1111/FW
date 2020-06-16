using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Context
{
    /// <summary>
    /// 基于环境的声明生成器仓储
    /// </summary>
    public interface IEnvironmentClaimGeneratorRepository
    {
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<EnvironmentClaimGenerator?> QueryByName(string name);
    }
}
