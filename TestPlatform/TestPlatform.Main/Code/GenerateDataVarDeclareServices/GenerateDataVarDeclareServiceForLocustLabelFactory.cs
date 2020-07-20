using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateDataVarDeclareServices
{
    [Injection(InterfaceType = typeof(GenerateDataVarDeclareServiceForLocustLabelFactory), Scope = InjectionScope.Singleton)]
    public class GenerateDataVarDeclareServiceForLocustLabelFactory : IFactory<IGenerateDataVarDeclareService>
    {
        private readonly GenerateDataVarDeclareServiceForLocustLabel _generateDataVarDeclareServiceForLocustLabel;

        public GenerateDataVarDeclareServiceForLocustLabelFactory(GenerateDataVarDeclareServiceForLocustLabel generateDataVarDeclareServiceForLocustLabel)
        {
            _generateDataVarDeclareServiceForLocustLabel = generateDataVarDeclareServiceForLocustLabel;
        }

        public IGenerateDataVarDeclareService Create()
        {
            return _generateDataVarDeclareServiceForLocustLabel;
        }
    }
}
