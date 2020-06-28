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
            sbCode.AppendLine("    print(\"TcpRRWithConnect\")");
            sbCode.AppendLine("    return \"\"");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
