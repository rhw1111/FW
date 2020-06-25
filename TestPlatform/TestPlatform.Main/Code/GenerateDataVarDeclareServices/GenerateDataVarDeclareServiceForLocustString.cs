using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Code.GenerateDataVarDeclareServices
{
    /// <summary>
    /// 针对Locust+String的数据变量声明代码块生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(GenerateDataVarDeclareServiceForLocustString), Scope = InjectionScope.Singleton)]
    public class GenerateDataVarDeclareServiceForLocustString : IGenerateDataVarDeclareService
    {
        public async Task<string> Generate(string name, string data)
        {
            return await Task.FromResult(data);
            //return data;
        }
    }
}
