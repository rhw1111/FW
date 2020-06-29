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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustDecimalRange), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustDecimalRange : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def DecimalRange(min, max):");
            sbCode.AppendLine("    # print(\"DecimalRange\")");
            sbCode.AppendLine("    import random");
            sbCode.AppendLine("    ");
            sbCode.AppendLine("    if min <= max:");
            sbCode.AppendLine("        ran = random.uniform(min, max)");
            sbCode.AppendLine("        str_min = str(min)");
            sbCode.AppendLine("        str_max = str(max)");
            sbCode.AppendLine("        dot = \".\"");
            sbCode.AppendLine("        ");
            sbCode.AppendLine("        if(dot in str_min):");
            sbCode.AppendLine("            index = str_min.index(dot)");
            sbCode.AppendLine("            length = len(str_min) - index");
            sbCode.AppendLine("            ran = float(round(ran, length))");
            sbCode.AppendLine("        elif(dot in str_max):");
            sbCode.AppendLine("            index = str_max.index(dot)");
            sbCode.AppendLine("            length = len(str_max) - index");
            sbCode.AppendLine("            ran = float(round(ran, length))");
            sbCode.AppendLine("        ");
            sbCode.AppendLine("        return ran");
            sbCode.AppendLine("    else:");
            sbCode.AppendLine("        return min");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
