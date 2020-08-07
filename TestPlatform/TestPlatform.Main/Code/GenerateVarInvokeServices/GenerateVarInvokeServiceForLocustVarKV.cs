using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Code.GenerateVarInvokeServices
{
    /// <summary>
    /// 针对LocustVarKV的变量配置声明代码块生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(GenerateVarInvokeServiceForLocustVarKV), Scope = InjectionScope.Singleton)]
    public class GenerateVarInvokeServiceForLocustVarKV : IGenerateVarInvokeService
    {
        public virtual async Task<string> Generate(string name, string[] parameters)
        {
            StringBuilder sbCode = new StringBuilder();

            sbCode.Append($"({parameters[0]}[{parameters[1]}] if {parameters[0]} else None)");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
