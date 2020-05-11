using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmRetrieveJTokenHandle
{
    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntityReferenceCollectionFactory), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntityReferenceCollectionFactory : IFactory<ICrmRetrieveJTokenHandle>
    {
        private CrmRetrieveJTokenHandleForCrmEntityReferenceCollection _crmRetrieveJTokenHandleForCrmEntityReferenceCollection;

        public CrmRetrieveJTokenHandleForCrmEntityReferenceCollectionFactory(CrmRetrieveJTokenHandleForCrmEntityReferenceCollection crmRetrieveJTokenHandleForCrmEntityReferenceCollection)
        {
            _crmRetrieveJTokenHandleForCrmEntityReferenceCollection = crmRetrieveJTokenHandleForCrmEntityReferenceCollection;
        }
        public ICrmRetrieveJTokenHandle Create()
        {
            return _crmRetrieveJTokenHandleForCrmEntityReferenceCollection;
        }
    }
}
