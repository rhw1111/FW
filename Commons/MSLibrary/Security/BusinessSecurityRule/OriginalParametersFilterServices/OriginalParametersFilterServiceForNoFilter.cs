using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Security.BusinessSecurityRule.OriginalParametersFilterServices
{
    /// <summary>
    /// 业务动作参数过滤处理
    /// 不对参数做过滤，直接返回原始参数
    /// </summary>
    [Injection(InterfaceType = typeof(OriginalParametersFilterServiceForNoFilter), Scope = InjectionScope.Singleton)]
    public class OriginalParametersFilterServiceForNoFilter : IOriginalParametersFilterService
    {
        public async Task<Dictionary<string, object>> Execute(Dictionary<string, object> originalParameters)
        {
            return await Task.FromResult<Dictionary<string, object>>(originalParameters);
        }
    }
}
