using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateDataVarDeclareServices
{
    [Injection(InterfaceType = typeof(GenerateDataVarDeclareServiceForLocustFactory), Scope = InjectionScope.Singleton)]
    public class GenerateDataVarDeclareServiceForLocustFactory : IFactory<IGenerateDataVarDeclareService>
    {
        private readonly GenerateDataVarDeclareServiceForLocust _generateDataVarDeclareServiceForLocust;

        public GenerateDataVarDeclareServiceForLocustFactory(GenerateDataVarDeclareServiceForLocust generateDataVarDeclareServiceForLocust)
        {
            _generateDataVarDeclareServiceForLocust = generateDataVarDeclareServiceForLocust;
        }

        public IGenerateDataVarDeclareService Create()
        {
            return _generateDataVarDeclareServiceForLocust;
        }
    }
}
