using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.Logger;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Logger
{
    [Injection(InterfaceType = typeof(ICommonLogLocalEnvInfoGeneratorService), Scope = InjectionScope.Singleton)]
    public class CommonLogLocalEnvInfoGeneratorService : ICommonLogLocalEnvInfoGeneratorService
    {
        public string GenerateUserInfo()
        {
            return string.Empty;
        }
    }
}
