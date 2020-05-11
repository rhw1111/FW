using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Context.DAL;

namespace MSLibrary.Context
{
    /// <summary>
    /// 基于声明的上下文生成器仓储
    /// </summary>
    [Injection(InterfaceType = typeof(IClaimContextGeneratorRepository), Scope = InjectionScope.Singleton)]
    public class ClaimContextGeneratorRepository : IClaimContextGeneratorRepository
    {
        private IClaimContextGeneratorStore _claimContextGeneratorStore;

        public ClaimContextGeneratorRepository(IClaimContextGeneratorStore claimContextGeneratorStore)
        {
            _claimContextGeneratorStore = claimContextGeneratorStore;
        }
        /// <summary>
        /// 根据名称查询生成器
        /// 这里的实现方式是直接创建生成器返回
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ClaimContextGenerator> QueryByName(string name)
        { 
            return await _claimContextGeneratorStore.QueryByName(name);
        }
    }
}
