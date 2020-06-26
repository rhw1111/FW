using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Code.GenerateDataVarDeclareServices
{
    /// <summary>
    /// 针对Locust的数据变量声明代码块生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(GenerateDataVarDeclareServiceForLocust), Scope = InjectionScope.Singleton)]
    public class GenerateDataVarDeclareServiceForLocust : IGenerateDataVarDeclareService
    {
        public virtual async Task<string> Generate(string name, string data)
        {
            string result = $"{name} = {data}";

            return await Task.FromResult(result);
        }
    }

}
