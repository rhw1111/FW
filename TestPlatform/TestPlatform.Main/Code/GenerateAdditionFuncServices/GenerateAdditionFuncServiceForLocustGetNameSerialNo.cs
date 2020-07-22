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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustGetNameSerialNo), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustGetNameSerialNo : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("serial_no_s = [");
            sbCode.AppendLine("    {");
            sbCode.AppendLine("        \"Name\": \"name\",");
            sbCode.AppendLine("        \"Type\": \"type\",");
            sbCode.AppendLine("        \"SerialNo\": 0");
            sbCode.AppendLine("    }");
            sbCode.AppendLine("]");
            sbCode.AppendLine("");
            sbCode.AppendLine("");
            sbCode.AppendLine("def GetNameSerialNo(name, type, start):");
            sbCode.AppendLine("    # print(\"GetNameSerialNo\")");
            sbCode.AppendLine("");
            sbCode.AppendLine("    for serial_no in serial_no_s:");
            sbCode.AppendLine("        if serial_no[\"Name\"] == name and serial_no[\"Type\"] == type:");
            sbCode.AppendLine("            serial_no[\"SerialNo\"] = serial_no[\"SerialNo\"] + 1");
            sbCode.AppendLine("            return serial_no[\"SerialNo\"]");
            sbCode.AppendLine("");
            sbCode.AppendLine("    serial_no = {");
            sbCode.AppendLine("        \"Name\": name,");
            sbCode.AppendLine("        \"Type\": type,");
            sbCode.AppendLine("        \"SerialNo\": start");
            sbCode.AppendLine("    }");
            sbCode.AppendLine("");
            sbCode.AppendLine("    serial_no_s.append(serial_no)");
            sbCode.AppendLine("    return serial_no[\"SerialNo\"]");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
