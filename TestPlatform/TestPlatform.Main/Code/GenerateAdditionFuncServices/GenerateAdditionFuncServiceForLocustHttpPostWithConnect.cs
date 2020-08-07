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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustHttpPostWithConnect), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustHttpPostWithConnect : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def HttpPostWithConnect(connect, url, headers, senddata, receivereg, name=None, self=None):");
            sbCode.AppendLine("    # print(\"HttpPostWithConnect\")");
            sbCode.AppendLine("    import re");
            sbCode.AppendLine("    import traceback");
            sbCode.AppendLine("    import time");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if not url:");
            sbCode.AppendLine("        return \"\"");
            sbCode.AppendLine("");
            sbCode.AppendLine("    start_time = time.time()");
            sbCode.AppendLine("");
            sbCode.AppendLine("    try:");
            sbCode.AppendLine("        if not headers:");
            sbCode.AppendLine("            headers = {\"User-Agent\": \"Mozilla/5.0(Windows NT 10.0; WOW64) AppleWebKit/537.36(KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36\"}");
            sbCode.AppendLine("");
            sbCode.AppendLine("        if type(senddata) == dict:");
            sbCode.AppendLine("            headers = {\"Content-Type\": \"application/json\"}");
            sbCode.AppendLine("            response = connect.post(url, headers=headers, json=senddata)");
            sbCode.AppendLine("        else:");
            sbCode.AppendLine("            response = connect.post(url, headers=headers, data=senddata)");
            sbCode.AppendLine("");
            sbCode.AppendLine("        if response.status_code == 200:");
            sbCode.AppendLine("            if name and self:");
            sbCode.AppendLine("                total_time = int((time.time() - start_time) * second_unit)");
            sbCode.AppendLine("                self.environment.events.request_success.fire(");
            sbCode.AppendLine("                    request_type=request_type, name=name,");
            sbCode.AppendLine("                    response_time=total_time, response_length=0)");
            sbCode.AppendLine("");
            sbCode.AppendLine("            Print(\"Http Post Success, Url, %s%s, StatusCode, %s, Text, %s\" % (connect.base_url, url, response.status_code, response.text))");
            sbCode.AppendLine("");
            sbCode.AppendLine("            result = response.text");
            sbCode.AppendLine("");
            sbCode.AppendLine("            p = re.compile(receivereg, re.S)");
            sbCode.AppendLine("            result = re.findall(p, result)");
            sbCode.AppendLine("");
            sbCode.AppendLine("            if len(result) > 0:");
            sbCode.AppendLine("                return result[0]");
            sbCode.AppendLine("            else:");
            sbCode.AppendLine("                return \"\"");
            sbCode.AppendLine("        else:");
            sbCode.AppendLine("            if name and self:");
            sbCode.AppendLine("                total_time = int((time.time() - start_time) * second_unit)");
            sbCode.AppendLine("                self.environment.events.request_failure.fire(");
            sbCode.AppendLine("                    request_type=request_type, name=name,");
            sbCode.AppendLine("                    response_time=total_time, response_length=0,");
            sbCode.AppendLine("                    exception=None)");
            sbCode.AppendLine(""); 
            sbCode.AppendLine("            print(\"[%s] [%s]: Http Post Fail, Url, %s%s, StatusCode, %s, Text, %s.\" % (datetime.datetime.now().strftime(datetime_format), client_id, connect.base_url, url, response.status_code, response.text))");
            sbCode.AppendLine("");
            sbCode.AppendLine("            return \"\"");
            sbCode.AppendLine("    except Exception as e:");
            sbCode.AppendLine("        if name and self:");
            sbCode.AppendLine("            total_time = int((time.time() - start_time) * second_unit)");
            sbCode.AppendLine("            self.environment.events.request_failure.fire(");
            sbCode.AppendLine("                request_type=request_type, name=name,");
            sbCode.AppendLine("                response_time=total_time, response_length=len(str(e)),");
            sbCode.AppendLine("                exception=e)");
            sbCode.AppendLine("");
            sbCode.AppendLine("        print(\"[%s] [%s]: Error, % s.\" % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))");
            sbCode.AppendLine("        print(\"[%s] [%s]: Error, % s.\" % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))");
            sbCode.AppendLine("");
            sbCode.AppendLine("        return \"\"");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
