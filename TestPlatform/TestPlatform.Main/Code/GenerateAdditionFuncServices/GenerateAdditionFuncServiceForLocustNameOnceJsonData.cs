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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustNameOnceJsonData), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustNameOnceJsonData : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def NameOnceJsonData(data):");
            sbCode.AppendLine("    # print(\"NameOnceJsonData\")");
            sbCode.AppendLine("    import random");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if data is not None and len(data) > 0:");
            sbCode.AppendLine("        index = random.randint(0, len(data) - 1)");
            sbCode.AppendLine("        row = data[index]");
            sbCode.AppendLine("        del data[index]");
            sbCode.AppendLine("");
            sbCode.AppendLine("        return row");
            sbCode.AppendLine("");
            sbCode.AppendLine("    return None");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
