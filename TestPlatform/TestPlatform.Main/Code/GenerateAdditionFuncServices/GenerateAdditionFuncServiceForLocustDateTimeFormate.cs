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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustDateTimeFormate), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustDateTimeFormate : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def DateTimeFormate(dt, formate):");
            sbCode.AppendLine("    # print(\"DateTimeFormate\")");
            sbCode.AppendLine("    import datetime");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if not dt or not formate:");
            sbCode.AppendLine("        return None");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if type(dt) == datetime.datetime:");
            sbCode.AppendLine("        return dt.strftime(formate)");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
