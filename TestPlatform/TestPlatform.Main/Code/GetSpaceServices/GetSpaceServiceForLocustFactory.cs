using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GetSpaceServices
{
    [Injection(InterfaceType = typeof(GetSpaceServiceForLocustFactory), Scope = InjectionScope.Singleton)]
    public class GetSpaceServiceForLocustFactory : IFactory<IGetSpaceService>
    {
        private readonly GetSpaceServiceForLocust _getSpaceServiceForLocust;

        public GetSpaceServiceForLocustFactory(GetSpaceServiceForLocust getSpaceServiceForLocust)
        {
            _getSpaceServiceForLocust = getSpaceServiceForLocust;
        }

        public IGetSpaceService Create()
        {
            return _getSpaceServiceForLocust;
        }
    }
}
