using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveMultiplePageFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveMultiplePageFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveMultiplePage _crmMessageHandleForRetrieveMultiplePage;

        public CrmMessageHandleForRetrieveMultiplePageFactory(CrmMessageHandleForRetrieveMultiplePage crmMessageHandleForRetrieveMultiplePage)
        {
            _crmMessageHandleForRetrieveMultiplePage = crmMessageHandleForRetrieveMultiplePage;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveMultiplePage;
        }
    }
}
