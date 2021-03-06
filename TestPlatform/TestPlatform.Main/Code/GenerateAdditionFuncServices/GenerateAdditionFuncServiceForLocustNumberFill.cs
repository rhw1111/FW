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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustNumberFill), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustNumberFill : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def NumberFill(number, direct, length):");
            sbCode.AppendLine("    # print(\"NumberFill\")");
            sbCode.AppendLine("    str_number = str(number)");
            sbCode.AppendLine("    result = \"\"");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if direct == 0:");
            sbCode.AppendLine("        result = str_number.zfill(length)");
            sbCode.AppendLine("    elif direct == 1:");
            sbCode.AppendLine("        if(\".\" in str_number):");
            sbCode.AppendLine("            result = str_number + \"0\" * (length - len(str_number))");
            sbCode.AppendLine("        else:");
            sbCode.AppendLine("            result = str_number");
            sbCode.AppendLine("    else:");
            sbCode.AppendLine("        result = str_number");
            sbCode.AppendLine("    return result");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
