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
            sbCode.AppendLine("    ");
            sbCode.AppendLine("    bufsize = 2048");
            sbCode.AppendLine("    ");
            sbCode.AppendLine("    try:");
            sbCode.AppendLine("        connect.send(str(senddata).encode())");
            sbCode.AppendLine("        ");
            sbCode.AppendLine("        data = connect.recv(bufsize).decode()");
            sbCode.AppendLine("        ");
            sbCode.AppendLine("        p = re.compile(receivereg, re.S)");
            sbCode.AppendLine("        result = re.findall(p, data)");
            sbCode.AppendLine("        ");
            sbCode.AppendLine("        if len(result) > 0:");
            sbCode.AppendLine("            return result[0]");
            sbCode.AppendLine("        else:");
            sbCode.AppendLine("            return \"\"");
            sbCode.AppendLine("    except Exception as e:");
            sbCode.AppendLine("        print(str(e))");
            sbCode.AppendLine("        return None");
            sbCode.AppendLine("        ");
            sbCode.AppendLine("    return \"\"");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
