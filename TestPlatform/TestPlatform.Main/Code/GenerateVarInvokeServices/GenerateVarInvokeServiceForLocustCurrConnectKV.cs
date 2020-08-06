using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Code.GenerateVarInvokeServices
{
    /// <summary>
    /// 针对LocustCurrConnectKV的变量配置声明代码块生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(GenerateVarInvokeServiceForLocustCurrConnectKV), Scope = InjectionScope.Singleton)]
    public class GenerateVarInvokeServiceForLocustCurrConnectKV : IGenerateVarInvokeService
    {
        public virtual async Task<string> Generate(string name, string[] parameters)
        {
            StringBuilder sbCode = new StringBuilder();

            sbCode.Append($"self.currconnectkv_user_id[{parameters[0]}]");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
