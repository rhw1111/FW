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
            sbCode.AppendLine("    # print(\"IntRange\")");
            sbCode.AppendLine("    import random");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if min <= max:");
            sbCode.AppendLine("        ran = random.randint(min, max)");
            sbCode.AppendLine("");
            sbCode.AppendLine("        return ran");
            sbCode.AppendLine("    else:");
            sbCode.AppendLine("        return min");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
