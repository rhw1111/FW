using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Code.GetSeparatorServices
{
    [Injection(InterfaceType = typeof(GetSeparatorServiceForLocust), Scope = InjectionScope.Singleton)]
    public class GetSeparatorServiceForLocust : IGetSeparatorService
    {
        public async Task<string> GetFuncSeparator()
        {
            return await Task.FromResult("\r\n");
            //return "\r\n";
        }
    }
}
