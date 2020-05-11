using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.CrmServiceFactoryServices
{
    [Injection(InterfaceType = typeof(CrmServiceFactoryServiceForCommonFactory), Scope = InjectionScope.Singleton)]
    public class CrmServiceFactoryServiceForCommonFactory : IFactory<ICrmServiceFactoryService>
    {
        private CrmServiceFactoryServiceForCommon _crmServiceFactoryServiceForADFS;

        public CrmServiceFactoryServiceForCommonFactory(CrmServiceFactoryServiceForCommon crmServiceFactoryServiceForADFS)
        {
            _crmServiceFactoryServiceForADFS = crmServiceFactoryServiceForADFS;
        }
        public ICrmServiceFactoryService Create()
        {
            return _crmServiceFactoryServiceForADFS;
        }
    }
}
