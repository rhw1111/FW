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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustPrint), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustPrint : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def Print(content):");
            sbCode.AppendLine("    # print(\"Print\")");
            sbCode.AppendLine("    if is_print_log:");
            sbCode.AppendLine("        print(\"[%s] [PrintLog] [%s]: %s.\" % (datetime.datetime.now().strftime(datetime_format), client_id, content))");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
