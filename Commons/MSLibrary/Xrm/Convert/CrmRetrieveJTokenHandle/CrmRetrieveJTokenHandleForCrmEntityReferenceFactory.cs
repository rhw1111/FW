using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmRetrieveJTokenHandle
{
    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntityReferenceFactory), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntityReferenceFactory : IFactory<ICrmRetrieveJTokenHandle>
    {
        private CrmRetrieveJTokenHandleForCrmEntityReference _crmRetrieveJTokenHandleForCrmEntityReference;

        public CrmRetrieveJTokenHandleForCrmEntityReferenceFactory(CrmRetrieveJTokenHandleForCrmEntityReference crmRetrieveJTokenHandleForCrmEntityReference)
        {
            _crmRetrieveJTokenHandleForCrmEntityReference = crmRetrieveJTokenHandleForCrmEntityReference;
        }
        public ICrmRetrieveJTokenHandle Create()
        {
            return _crmRetrieveJTokenHandleForCrmEntityReference;
        }
    }
}
