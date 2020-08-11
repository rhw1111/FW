using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Code.GenerateVarSettingServices
{
    /// <summary>
    /// 针对Locust的变量配置声明代码块生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(GenerateVarSettingServiceForLocust), Scope = InjectionScope.Singleton)]
    public class GenerateVarSettingServiceForLocust : IGenerateVarSettingService
    {
        public virtual async Task<string> Generate(string name, string content)
        {
            StringBuilder sbCode = new StringBuilder();

            if (string.IsNullOrEmpty(name))
            {
                sbCode.Append($"{content}");
            }
            else
            {
                sbCode.Append($"{name} = {content}");
            }

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
