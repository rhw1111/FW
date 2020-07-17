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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustGetJsonRowData), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustGetJsonRowData : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def GetJsonRowData(data):");
            sbCode.AppendLine("    # print(\"GetJsonRowData\")");
            sbCode.AppendLine("    import random");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if data and type(data) == dict:");
            sbCode.AppendLine("        return data");
            sbCode.AppendLine("    if data and len(data) > 0:");
            sbCode.AppendLine("        index = random.randint(0, len(data) - 1)");
            sbCode.AppendLine("        row = data[index]");
            sbCode.AppendLine("");
            sbCode.AppendLine("        return row");
            sbCode.AppendLine("");
            sbCode.AppendLine("    return None");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
