﻿using MSLibrary.DI;
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
            sbCode.AppendLine("        return \"\"");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if type(senddata) != str:");
            sbCode.AppendLine("        senddata = str(senddata)");
            sbCode.AppendLine("");
            sbCode.AppendLine("    buffsize = 10240");
            sbCode.AppendLine("");
            sbCode.AppendLine("    try:");
            sbCode.AppendLine("        # Print(\"SendData: %s\" % senddata)");
            sbCode.AppendLine("        connect.send(senddata)");
            sbCode.AppendLine("        # Print(\"SendData Success\")");
            sbCode.AppendLine("");
            sbCode.AppendLine("        if sync_type:");
            sbCode.AppendLine("            # Print(\"RecvData Waitting...\")");
            sbCode.AppendLine("            data = connect.recv(buffsize)");
            sbCode.AppendLine("            # Print(\"RecvData: %s\" % data)");
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
            sbCode.AppendLine("        print(\"[%s] %s: Error, %s.\" % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))");
            sbCode.AppendLine("");
            sbCode.AppendLine("        return \"\"");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
