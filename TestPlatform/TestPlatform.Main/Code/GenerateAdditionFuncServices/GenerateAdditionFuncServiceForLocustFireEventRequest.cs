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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustFireEventRequest), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustFireEventRequest : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def FireEventRequest(self, is_success, name, start_time, e):");
            sbCode.AppendLine("    # print(\"FireEventRequest\")");
            sbCode.AppendLine("");
            sbCode.AppendLine("    import time");
            sbCode.AppendLine("");
            sbCode.AppendLine("    total_time = int((time.time() - start_time) * second_unit)");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if is_success:");
            sbCode.AppendLine("        self.environment.events.request_success.fire(");
            sbCode.AppendLine("            request_type=request_type, name=name,");
            sbCode.AppendLine("            response_time=total_time, response_length=0)");
            sbCode.AppendLine("    else:");
            sbCode.AppendLine("        self.environment.events.request_failure.fire(");
            sbCode.AppendLine("            request_type=request_type, name=name,");
            sbCode.AppendLine("            response_time=total_time, response_length=len(str(e)),");
            sbCode.AppendLine("            exception=e)");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
