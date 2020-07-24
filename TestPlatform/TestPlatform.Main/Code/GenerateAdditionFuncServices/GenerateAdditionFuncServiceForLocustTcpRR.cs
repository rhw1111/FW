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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustTcpRR), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustTcpRR : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def TcpRR(address, port, senddata, receivereg):");
            sbCode.AppendLine("    # print(\"TcpRR\")");
            sbCode.AppendLine("    import socket");
            sbCode.AppendLine("    import re");
            sbCode.AppendLine("    import traceback");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if address is None or address == \"\" or port is None or port == \"\":");
            sbCode.AppendLine("        return \"\"");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if senddata is None or senddata == \"\":");
            sbCode.AppendLine("        return \"\"");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if type(senddata) != str:");
            sbCode.AppendLine("        senddata = str(senddata)");
            sbCode.AppendLine("");
            sbCode.AppendLine("    host = address");
            sbCode.AppendLine("    port = port");
            sbCode.AppendLine("    ADDR = (host, port)");
            sbCode.AppendLine("    buffsize = 10240");
            sbCode.AppendLine("");
            sbCode.AppendLine("    connect = socket.socket(socket.AF_INET, socket.SOCK_STREAM)");
            sbCode.AppendLine("");
            sbCode.AppendLine("    try:");
            sbCode.AppendLine("        connect.connect(ADDR)");
            sbCode.AppendLine("        Print(\"Connect Success, % s:% s\" % (address, port))");
            sbCode.AppendLine("");
            sbCode.AppendLine("        connect.send(senddata.encode())");
            sbCode.AppendLine("        Print(\"Send Success\")");
            sbCode.AppendLine("        Print(\"SendData, %s\" % senddata)");
            sbCode.AppendLine("");
            sbCode.AppendLine("        if sync_type:");
            sbCode.AppendLine("            Print(\"Recv Waitting...\")");
            sbCode.AppendLine("            data = connect.recv(buffsize).decode()");
            sbCode.AppendLine("            Print(\"RecvData, %s\" % data)");
            sbCode.AppendLine("");
            sbCode.AppendLine("            p = re.compile(receivereg, re.S)");
            sbCode.AppendLine("            result = re.findall(p, data)");
            sbCode.AppendLine("");
            sbCode.AppendLine("            if len(result) > 0:");
            sbCode.AppendLine("                return result[0]");
            sbCode.AppendLine("            else:");
            sbCode.AppendLine("                return \"\"");
            sbCode.AppendLine("        else:");
            sbCode.AppendLine("            return \"OK\"");
            sbCode.AppendLine("    except Exception as e:");
            sbCode.AppendLine("        print(\"[%s] [%s]: Error, % s.\" % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))");
            sbCode.AppendLine("        print(\"[%s] [%s]: Error, % s.\" % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))");
            sbCode.AppendLine("");
            sbCode.AppendLine("        return \"\"");
            sbCode.AppendLine("    finally:");
            sbCode.AppendLine("        # Print(\"Connect Closeing...\")");
            sbCode.AppendLine("        connect.close()");
            sbCode.AppendLine("        # Print(\"Connect Close Success\")");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
