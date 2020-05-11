using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmRetrieveJTokenHandle
{
    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntityReferenceListFactory), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntityReferenceListFactory : IFactory<ICrmRetrieveJTokenHandle>
    {
        private CrmRetrieveJTokenHandleForCrmEntityReferenceList _crmRetrieveJTokenHandleForCrmEntityReferenceList;

        public CrmRetrieveJTokenHandleForCrmEntityReferenceListFactory(CrmRetrieveJTokenHandleForCrmEntityReferenceList crmRetrieveJTokenHandleForCrmEntityReferenceList)
        {
            _crmRetrieveJTokenHandleForCrmEntityReferenceList = crmRetrieveJTokenHandleForCrmEntityReferenceList;
        }

        public ICrmRetrieveJTokenHandle Create()
        {
            return _crmRetrieveJTokenHandleForCrmEntityReferenceList;
        }
    }
}
