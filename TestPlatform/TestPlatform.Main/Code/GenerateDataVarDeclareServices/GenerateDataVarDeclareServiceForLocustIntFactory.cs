using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateDataVarDeclareServices
{
    [Injection(InterfaceType = typeof(GenerateDataVarDeclareServiceForLocustIntFactory), Scope = InjectionScope.Singleton)]
    public class GenerateDataVarDeclareServiceForLocustIntFactory : IFactory<IGenerateDataVarDeclareService>
    {
        private readonly GenerateDataVarDeclareServiceForLocustInt _generateDataVarDeclareServiceForLocustInt;

        public GenerateDataVarDeclareServiceForLocustIntFactory(GenerateDataVarDeclareServiceForLocustInt generateDataVarDeclareServiceForLocustInt)
        {
            _generateDataVarDeclareServiceForLocustInt = generateDataVarDeclareServiceForLocustInt;
        }

        public IGenerateDataVarDeclareService Create()
        {
            return _generateDataVarDeclareServiceForLocustInt;
        }
    }

}
