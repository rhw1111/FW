using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmRetrieveJTokenHandle
{
    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntityListFactory), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntityListFactory : IFactory<ICrmRetrieveJTokenHandle>
    {
        private CrmRetrieveJTokenHandleForCrmEntityList _crmRetrieveJTokenHandleForCrmEntityList;

        public CrmRetrieveJTokenHandleForCrmEntityListFactory(CrmRetrieveJTokenHandleForCrmEntityList crmRetrieveJTokenHandleForCrmEntityList)
        {
            _crmRetrieveJTokenHandleForCrmEntityList = crmRetrieveJTokenHandleForCrmEntityList;
        }
        public ICrmRetrieveJTokenHandle Create()
        {
            return _crmRetrieveJTokenHandleForCrmEntityList;
        }
    }
}
