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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustFilterJsonData), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustFilterJsonData : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def FilterJsonData(data, name, value):");
            sbCode.AppendLine("    # print(\"FilterJsonData\")");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if data and type(data) == dict:");
            sbCode.AppendLine("        if data[name] == value:");
            sbCode.AppendLine("            return [data]");
            sbCode.AppendLine("        else:");
            sbCode.AppendLine("            return []");
            sbCode.AppendLine("    elif data and len(data) > 0:");
            sbCode.AppendLine("        result = []");
            sbCode.AppendLine("");
            sbCode.AppendLine("        for row in data:");
            sbCode.AppendLine("            if row[name] == value:");
            sbCode.AppendLine("                result.append(row)");
            sbCode.AppendLine("");
            sbCode.AppendLine("        return result");
            sbCode.AppendLine("");
            sbCode.AppendLine("    return None");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
