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
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustDesSecurity), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustDesSecurity : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def DesSecurity(data, key):");
            sbCode.AppendLine("    # print(\"DesSecurity\")");
            sbCode.AppendLine("    from Crypto.Cipher import DES3");
            sbCode.AppendLine("    import base64");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if len(key) % 16 > 0:");
            sbCode.AppendLine("        return \"\"");
            sbCode.AppendLine("");
            sbCode.AppendLine("    key = key");
            sbCode.AppendLine("    mode = DES3.MODE_CBC");
            sbCode.AppendLine("    iv = b\"12345678\"");
            sbCode.AppendLine("    length = DES3.block_size");
            sbCode.AppendLine("");
            sbCode.AppendLine("    text = str(data)");
            sbCode.AppendLine("    text = text + (length - len(text) % length) * chr(length - len(text) % length)");
            sbCode.AppendLine("    cryptor = DES3.new(key, mode, iv)");
            sbCode.AppendLine("    x = len(text) % 8");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if x != 0:");
            sbCode.AppendLine("        text = text + \"\\0\" * (8 - x)");
            sbCode.AppendLine("");
            sbCode.AppendLine("    ciphertext = cryptor.encrypt(text)");
            sbCode.AppendLine("    result = base64.standard_b64encode(ciphertext).decode(\"utf - 8\")");
            sbCode.AppendLine("");
            sbCode.AppendLine("    return result");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
