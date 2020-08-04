using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustFireEventRequestFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustFireEventRequestFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustFireEventRequest _generateAdditionFuncServiceForLocustFireEventRequest;

        public GenerateAdditionFuncServiceForLocustFireEventRequestFactory(GenerateAdditionFuncServiceForLocustFireEventRequest generateAdditionFuncServiceForLocustFireEventRequest)
        {
            _generateAdditionFuncServiceForLocustFireEventRequest = generateAdditionFuncServiceForLocustFireEventRequest;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustFireEventRequest;
        }
    }
}
