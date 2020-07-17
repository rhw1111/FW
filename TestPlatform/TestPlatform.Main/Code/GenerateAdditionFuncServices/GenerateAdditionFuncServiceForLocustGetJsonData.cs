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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustGetJsonData), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustGetJsonData : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def GetJsonData(data, gettype, endtype):");
            sbCode.AppendLine("    # print(\"GetJsonData\")");
            sbCode.AppendLine("    import random");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if data and type(data) == dict:");
            sbCode.AppendLine("        return data");
            sbCode.AppendLine("    elif data and len(data) > 0:");
            sbCode.AppendLine("        if gettype == 0 or gettype == 1:");
            sbCode.AppendLine("            if endtype == 0:");
            sbCode.AppendLine("                index = 0");
            sbCode.AppendLine("                row = data[index]");
            sbCode.AppendLine("                del data[index]");
            sbCode.AppendLine("");
            sbCode.AppendLine("                return row");
            sbCode.AppendLine("            elif endtype == 1:");
            sbCode.AppendLine("                index = 0");
            sbCode.AppendLine("                row = data[index]");
            sbCode.AppendLine("                del data[index]");
            sbCode.AppendLine("                data.append(row)");
            sbCode.AppendLine("");
            sbCode.AppendLine("                return row");
            sbCode.AppendLine("            elif endtype == 2:");
            sbCode.AppendLine("                index = 0");
            sbCode.AppendLine("                row = data[index]");
            sbCode.AppendLine("");
            sbCode.AppendLine("                if len(data) > 1:");
            sbCode.AppendLine("                    del data[index]");
            sbCode.AppendLine("");
            sbCode.AppendLine("                return row");
            sbCode.AppendLine("        elif gettype == 2:");
            sbCode.AppendLine("            index = random.randint(0, len(data) - 1)");
            sbCode.AppendLine("            row = data[index]");
            sbCode.AppendLine("");
            sbCode.AppendLine("            return row");
            sbCode.AppendLine("    elif not data and len(data) == 0:");
            sbCode.AppendLine("        if endtype == 0:");
            sbCode.AppendLine("            TcpTestUser.environment.runner.quit()");
            sbCode.AppendLine("            return None");
            sbCode.AppendLine("");
            sbCode.AppendLine("    return None");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
