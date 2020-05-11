using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController
{
    /// <summary>
    /// 原始参数信息过滤处理服务接口
    /// </summary>
    public interface IOriginalParametersFilterService
    {
        Task<Dictionary<string, object>> Execute(Dictionary<string, object> originalParameters);
    }
}
