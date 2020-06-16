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
        public string GenerateParentUserInfo()
        {
            return string.Empty;
        }

        public string GenerateUserInfo()
        {
            return string.Empty;
        }
    }
}
