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
            sbCode.AppendLine("from Crypto.Cipher import DES3");
            sbCode.AppendLine("import base64");
            sbCode.AppendLine("");
            sbCode.AppendLine("");
            sbCode.AppendLine("def DesSecurity(data, key):");
            sbCode.AppendLine("    # print(\"DesSecurity\")");
            //sbCode.AppendLine("    from Crypto.Cipher import DES3");
            //sbCode.AppendLine("    import base64");
            //sbCode.AppendLine("");
            //sbCode.AppendLine("    if len(key) % 16 > 0:");
            //sbCode.AppendLine("        return \"\"");
            //sbCode.AppendLine("");
            //sbCode.AppendLine("    key = key");
            //sbCode.AppendLine("    mode = DES3.MODE_CBC");
            //sbCode.AppendLine("    iv = b\"12345678\"");
            //sbCode.AppendLine("    length = DES3.block_size");
            //sbCode.AppendLine("");
            //sbCode.AppendLine("    text = str(data)");
            //sbCode.AppendLine("    text = text + (length - len(text) % length) * chr(length - len(text) % length)");
            //sbCode.AppendLine("    cryptor = DES3.new(key, mode, iv)");
            //sbCode.AppendLine("    x = len(text) % 8");
            //sbCode.AppendLine("");
            //sbCode.AppendLine("    if x != 0:");
            //sbCode.AppendLine("        text = text + \"\\0\" * (8 - x)");
            //sbCode.AppendLine("");
            //sbCode.AppendLine("    ciphertext = cryptor.encrypt(text)");
            //sbCode.AppendLine("    result = base64.standard_b64encode(ciphertext).decode(\"utf - 8\")");
            //sbCode.AppendLine("");
            //sbCode.AppendLine("    return result");
            sbCode.AppendLine("");
            sbCode.AppendLine("    ed.setKey(key)");
            sbCode.AppendLine("    return ed.encrypt(data)");
            sbCode.AppendLine("");
            sbCode.AppendLine("");
            sbCode.AppendLine("class EncryptDate():");
            sbCode.AppendLine("    def __init__(self, key):");
            sbCode.AppendLine("        self.key = key");
            sbCode.AppendLine("        self.mode = DES3.MODE_CBC");
            sbCode.AppendLine("        self.iv = b\"12345678\"");
            sbCode.AppendLine("        self.length = DES3.block_size");
            sbCode.AppendLine("");
            sbCode.AppendLine("    def setKey(self, key):");
            sbCode.AppendLine("        self.key = key");
            sbCode.AppendLine("");
            sbCode.AppendLine("    def pad(self, s):");
            sbCode.AppendLine("        return s + (self.length - len(s) % self.length) * chr(self.length - len(s) % self.length)");
            sbCode.AppendLine("");
            sbCode.AppendLine("    # 定义 padding 即 填充 为PKCS7");
            sbCode.AppendLine("    def unpad(self, s):");
            sbCode.AppendLine("        return s[0:-ord(s[-1])]");
            sbCode.AppendLine("");
            sbCode.AppendLine("    # DES3的加密模式为CBC");
            sbCode.AppendLine("    def encrypt(self, text):");
            sbCode.AppendLine("        if type(text) != str:");
            sbCode.AppendLine("            text = str(text)");
            sbCode.AppendLine("");
            sbCode.AppendLine("        text = self.pad(text)");
            sbCode.AppendLine("        cryptor = DES3.new(self.key, self.mode, self.iv)");
            sbCode.AppendLine("        # self.iv 为 IV 即偏移量");
            sbCode.AppendLine("        x = len(text) % 8");
            sbCode.AppendLine("");
            sbCode.AppendLine("        if x != 0:");
            sbCode.AppendLine("            text = text + \"\\0\" * (8 - x)  # 不满16，32，64位补0");
            sbCode.AppendLine("");
            sbCode.AppendLine("        self.ciphertext = cryptor.encrypt(text)");
            sbCode.AppendLine("        return base64.standard_b64encode(self.ciphertext).decode(\"utf - 8\")");
            sbCode.AppendLine("");
            sbCode.AppendLine("    def decrypt(self, text):");
            sbCode.AppendLine("        if type(text) != str:");
            sbCode.AppendLine("            text = str(text)");
            sbCode.AppendLine("");
            sbCode.AppendLine("        cryptor = DES3.new(self.key, self.mode, self.iv)");
            sbCode.AppendLine("        de_text = base64.standard_b64decode(text)");
            sbCode.AppendLine("        plain_text = cryptor.decrypt(de_text)");
            sbCode.AppendLine("        st = str(plain_text.decode(\"utf - 8\")).rstrip(\"\\0\")");
            sbCode.AppendLine("        out = self.unpad(st)");
            sbCode.AppendLine("        return out");
            sbCode.AppendLine("");
            sbCode.AppendLine("");
            sbCode.AppendLine("# 秘钥，密钥的长度必须是16的倍数");
            sbCode.AppendLine("key = \"abcdefghijklmnop\"");
            sbCode.AppendLine("ed = EncryptDate(key)");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
