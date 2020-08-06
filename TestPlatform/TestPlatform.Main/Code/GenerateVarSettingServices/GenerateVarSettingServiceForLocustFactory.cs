using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateVarSettingServices
{
    [Injection(InterfaceType = typeof(GenerateVarSettingServiceForLocustFactory), Scope = InjectionScope.Singleton)]
    public class GenerateVarSettingServiceForLocustFactory : IFactory<IGenerateVarSettingService>
    {
        private readonly GenerateVarSettingServiceForLocust _generateVarSettingServiceForLocust;

        public GenerateVarSettingServiceForLocustFactory(GenerateVarSettingServiceForLocust generateVarSettingServiceForLocust)
        {
            _generateVarSettingServiceForLocust = generateVarSettingServiceForLocust;
        }

        public IGenerateVarSettingService Create()
        {
            return _generateVarSettingServiceForLocust;
        }
    }
}
