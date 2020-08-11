using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Code.GenerateFuncInvokeServices
{
    /// <summary>
    /// 针对Locust的变量配置声明代码块生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(GenerateFuncInvokeServiceForLocust), Scope = InjectionScope.Singleton)]
    public class GenerateFuncInvokeServiceForLocust : IGenerateFuncInvokeService
    {
        public virtual async Task<string> Generate(string name, string[] parameters)
        {
            string strParameters = String.Join("\\,", parameters);

            StringBuilder sbCode = new StringBuilder();

            if (string.IsNullOrEmpty(strParameters))
            {
                sbCode.Append($"{name}()");
            }
            else
            {
                sbCode.Append($"{name}({strParameters})");
            }

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
