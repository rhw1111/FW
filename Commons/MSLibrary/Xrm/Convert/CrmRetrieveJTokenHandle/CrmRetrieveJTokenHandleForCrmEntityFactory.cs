using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmRetrieveJTokenHandle
{

    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntityFactory), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntityFactory : IFactory<ICrmRetrieveJTokenHandle>
    {
        private CrmRetrieveJTokenHandleForCrmEntity _crmRetrieveJTokenHandleForCrmEntity;

        public CrmRetrieveJTokenHandleForCrmEntityFactory(CrmRetrieveJTokenHandleForCrmEntity crmRetrieveJTokenHandleForCrmEntity)
        {
            _crmRetrieveJTokenHandleForCrmEntity = crmRetrieveJTokenHandleForCrmEntity;
        }
        public ICrmRetrieveJTokenHandle Create()
        {
            return _crmRetrieveJTokenHandleForCrmEntity;
        }
    }
}
