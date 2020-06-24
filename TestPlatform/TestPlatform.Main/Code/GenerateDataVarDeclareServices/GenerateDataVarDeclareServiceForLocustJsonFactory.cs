using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateDataVarDeclareServices
{
    [Injection(InterfaceType = typeof(GenerateDataVarDeclareServiceForLocustJsonFactory), Scope = InjectionScope.Singleton)]
    public class GenerateDataVarDeclareServiceForLocustJsonFactory : IFactory<IGenerateDataVarDeclareService>
    {
        private readonly GenerateDataVarDeclareServiceForLocustJson _generateDataVarDeclareServiceForLocustJson;

        public GenerateDataVarDeclareServiceForLocustJsonFactory(GenerateDataVarDeclareServiceForLocustJson generateDataVarDeclareServiceForLocustJson)
        {
            _generateDataVarDeclareServiceForLocustJson = generateDataVarDeclareServiceForLocustJson;
        }

        public IGenerateDataVarDeclareService Create()
        {
            return _generateDataVarDeclareServiceForLocustJson;
        }
    }

}
