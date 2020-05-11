using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmRetrieveJTokenHandle
{
    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntityCollectionFactory), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntityCollectionFactory : IFactory<ICrmRetrieveJTokenHandle>
    {
        private CrmRetrieveJTokenHandleForCrmEntityCollection _crmRetrieveJTokenHandleForCrmEntityCollection;

        public CrmRetrieveJTokenHandleForCrmEntityCollectionFactory(CrmRetrieveJTokenHandleForCrmEntityCollection crmRetrieveJTokenHandleForCrmEntityCollection)
        {
            _crmRetrieveJTokenHandleForCrmEntityCollection = crmRetrieveJTokenHandleForCrmEntityCollection;
        }
        public ICrmRetrieveJTokenHandle Create()
        {
            return _crmRetrieveJTokenHandleForCrmEntityCollection;
        }
    }
}
