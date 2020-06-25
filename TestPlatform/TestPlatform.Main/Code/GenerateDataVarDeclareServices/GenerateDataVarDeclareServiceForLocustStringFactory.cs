using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateDataVarDeclareServices
{
    [Injection(InterfaceType = typeof(GenerateDataVarDeclareServiceForLocustStringFactory), Scope = InjectionScope.Singleton)]
    public class GenerateDataVarDeclareServiceForLocustStringFactory : IFactory<IGenerateDataVarDeclareService>
    {
        private readonly GenerateDataVarDeclareServiceForLocustString _generateDataVarDeclareServiceForLocustString;

        public GenerateDataVarDeclareServiceForLocustStringFactory(GenerateDataVarDeclareServiceForLocustString generateDataVarDeclareServiceForLocustString)
        {
            _generateDataVarDeclareServiceForLocustString = generateDataVarDeclareServiceForLocustString;
        }

        public IGenerateDataVarDeclareService Create()
        {
            return _generateDataVarDeclareServiceForLocustString;
        }
    }
}
