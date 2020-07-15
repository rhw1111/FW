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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustTcpRRWithConnect), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustTcpRRWithConnect : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def TcpRRWithConnect(connect, senddata, receivereg):");
            sbCode.AppendLine("    # print(\"TcpRRWithConnect\")");
            sbCode.AppendLine("    import socket");
            sbCode.AppendLine("    import re");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if senddata is None or senddata == \"\":");
            sbCode.AppendLine("        return None");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if type(senddata) != str:");
            sbCode.AppendLine("        senddata = str(senddata)");
            sbCode.AppendLine("");
            sbCode.AppendLine("    buffsize = 10240");
            sbCode.AppendLine("");
            sbCode.AppendLine("    try:");
            sbCode.AppendLine("        connect.send(senddata)");
            sbCode.AppendLine("");
            sbCode.AppendLine("        data = connect.recv(buffsize)");
            sbCode.AppendLine("");
            sbCode.AppendLine("        p = re.compile(receivereg, re.S)");
            sbCode.AppendLine("        result = re.findall(p, data)");
            sbCode.AppendLine("");
            sbCode.AppendLine("        if len(result) > 0:");
            sbCode.AppendLine("            return result[0]");
            sbCode.AppendLine("        else:");
            sbCode.AppendLine("            return \"\"");
            sbCode.AppendLine("    except Exception as e:");
            sbCode.AppendLine("        print(\"[% s] % s: Error, % s.\" % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))");
            sbCode.AppendLine("");
            sbCode.AppendLine("        return None");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
