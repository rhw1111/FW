using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Context.DAL;

namespace MSLibrary.Context
{
    /// <summary>
    /// 基于环境的声明生成器仓储
    /// </summary>
    [Injection(InterfaceType = typeof(IEnvironmentClaimGeneratorRepository), Scope = InjectionScope.Singleton)]
    public class EnvironmentClaimGeneratorRepository : IEnvironmentClaimGeneratorRepository
    {
        private IEnvironmentClaimGeneratorStore _environmentClaimGeneratorStore;

        public EnvironmentClaimGeneratorRepository(IEnvironmentClaimGeneratorStore environmentClaimGeneratorStore)
        {
            _environmentClaimGeneratorStore = environmentClaimGeneratorStore;
        }
        /// <summary>
        /// 根据名称查询
        /// 直接新建声明生成器赋值后返回
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<EnvironmentClaimGenerator> QueryByName(string name)
        {
            return await _environmentClaimGeneratorStore.QueryByName(name);
        }
    }
}
