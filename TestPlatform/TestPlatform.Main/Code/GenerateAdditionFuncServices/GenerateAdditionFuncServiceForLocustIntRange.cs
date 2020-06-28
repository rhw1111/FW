using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    /// <summary>
    /// 针对Locust的附件函数生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustIntRange), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustIntRange : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def IntRange(min, max):");
            sbCode.AppendLine(" print(\"IntRange\")");
            sbCode.AppendLine(" return \"\"");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
