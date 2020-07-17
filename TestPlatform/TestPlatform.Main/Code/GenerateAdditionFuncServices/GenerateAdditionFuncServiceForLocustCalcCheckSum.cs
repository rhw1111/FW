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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustCalcCheckSum), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustCalcCheckSum : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def CalcCheckSum(msg):");
            sbCode.AppendLine("    # print(\"CalcCheckSum\")");
            sbCode.AppendLine("    sum = 0");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if msg:");
            sbCode.AppendLine("        for letter in msg:");
            sbCode.AppendLine("            sum += ord(letter)");
            sbCode.AppendLine("");
            sbCode.AppendLine("    return sum & 255");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
