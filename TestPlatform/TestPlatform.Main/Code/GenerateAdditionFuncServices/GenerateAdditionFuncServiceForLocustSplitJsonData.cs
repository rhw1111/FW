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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustSplitJsonData), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustSplitJsonData : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def SplitJsonData(data, piece):");
            sbCode.AppendLine("    # print(\"SplitJsonData\")");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if data and type(data) == dict:");
            sbCode.AppendLine("        return data");
            sbCode.AppendLine("    elif piece <= 0:");
            sbCode.AppendLine("        return None");
            sbCode.AppendLine("    elif data and len(data) > 0:");
            sbCode.AppendLine("        if len(data) < piece:");
            sbCode.AppendLine("            return None");
            sbCode.AppendLine("        else:");
            sbCode.AppendLine("            length = len(data)");
            sbCode.AppendLine("            count = length // piece");
            sbCode.AppendLine("            result = []");
            sbCode.AppendLine("");
            sbCode.AppendLine("            for row in data:");
            sbCode.AppendLine("                if not row.get(\"_SplitJsonData_\"):");
            sbCode.AppendLine("                    row[\"_SplitJsonData_\"] = True");
            sbCode.AppendLine("                    result.append(row)");
            sbCode.AppendLine("                ");
            sbCode.AppendLine("                if(len(result) >= count):");
            sbCode.AppendLine("                    break");
            sbCode.AppendLine("");
            sbCode.AppendLine("            return result");
            sbCode.AppendLine("");
            sbCode.AppendLine("    return None");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
