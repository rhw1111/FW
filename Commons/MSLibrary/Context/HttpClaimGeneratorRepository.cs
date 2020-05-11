using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Context.DAL;

namespace MSLibrary.Context
{
    /// <summary>
    /// 基于Http的声明生成器仓储
    /// </summary>
    [Injection(InterfaceType = typeof(IHttpClaimGeneratorRepository), Scope = InjectionScope.Singleton)]
    public class HttpClaimGeneratorRepository : IHttpClaimGeneratorRepository
    {
        private IHttpClaimGeneratorStore _httpClaimGeneratorStore;

        public HttpClaimGeneratorRepository(IHttpClaimGeneratorStore httpClaimGeneratorStore)
        {
            _httpClaimGeneratorStore = httpClaimGeneratorStore;
        }
        /// <summary>
        /// 根据名称查询
        /// 直接新建声明生成器赋值后返回
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<HttpClaimGenerator> QueryByName(string name)
        {
            return await _httpClaimGeneratorStore.QueryByName(name);
        }
    }
}
