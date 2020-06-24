using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GetSeparatorServices
{
    [Injection(InterfaceType = typeof(GetSeparatorServiceForLocustFactory), Scope = InjectionScope.Singleton)]
    public class GetSeparatorServiceForLocustFactory : IFactory<IGetSeparatorService>
    {
        private readonly GetSeparatorServiceForLocust _getSeparatorServiceForLocust;

        public GetSeparatorServiceForLocustFactory(GetSeparatorServiceForLocust getSeparatorServiceForLocust)
        {
            _getSeparatorServiceForLocust = getSeparatorServiceForLocust;
        }

        public IGetSeparatorService Create()
        {
            return _getSeparatorServiceForLocust;
        }
    }
}
