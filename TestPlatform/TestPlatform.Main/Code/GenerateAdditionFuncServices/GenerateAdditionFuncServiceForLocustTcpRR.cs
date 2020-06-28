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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustTcpRR), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustTcpRR : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def TcpRR(address, content, regex):");
            sbCode.AppendLine(" print(\"TcpRR\")");
            sbCode.AppendLine(" return \"\"");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
