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
            sbCode.AppendLine("    if data is not None and type(data).__name__ == \"dict\":");
            sbCode.AppendLine("        if data[name] == value:");
            sbCode.AppendLine("            return [data]");
            sbCode.AppendLine("        else:");
            sbCode.AppendLine("            return []");
            sbCode.AppendLine("    elif data is not None and len(data) > 0:");
            sbCode.AppendLine("        filterJsonData = []");
            sbCode.AppendLine("");
            sbCode.AppendLine("        for row in data:");
            sbCode.AppendLine("            if row[name] == value:");
            sbCode.AppendLine("                filterJsonData.append(row)");
            sbCode.AppendLine("");
            sbCode.AppendLine("        return filterJsonData");
            sbCode.AppendLine("");
            sbCode.AppendLine("    return None");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
