using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Code.GenerateVarInvokeServices
{
    /// <summary>
    /// 针对LocustNow的变量配置声明代码块生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(GenerateVarInvokeServiceForLocustNow), Scope = InjectionScope.Singleton)]
    public class GenerateVarInvokeServiceForLocustNow : IGenerateVarInvokeService
    {
        public virtual async Task<string> Generate(string name, string[] parameters)
        {
            StringBuilder sbCode = new StringBuilder();

            if (parameters.Length == 0 || string.IsNullOrEmpty(parameters[0]))
            {
                sbCode.Append($"datetime.datetime.now()");
            }
            else
            {
                sbCode.Append($"datetime.datetime.now().strftime({parameters[0]})");
            }

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
