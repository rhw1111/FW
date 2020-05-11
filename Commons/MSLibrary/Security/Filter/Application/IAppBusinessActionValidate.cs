using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;

namespace MSLibrary.Security.Filter.Application
{
    /// <summary>
    /// 应用层验证业务动作
    /// </summary>
    public interface IAppBusinessActionValidate
    {
        Task<ValidateResult> Do(string actionName,Dictionary<string,object> parameters);
    }
}
