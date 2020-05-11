using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveMultipleFetchFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveMultipleFetchFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveMultipleFetch _crmMessageHandleForRetrieveMultipleFetch;

        public CrmMessageHandleForRetrieveMultipleFetchFactory(CrmMessageHandleForRetrieveMultipleFetch crmMessageHandleForRetrieveMultipleFetch)
        {
            _crmMessageHandleForRetrieveMultipleFetch = crmMessageHandleForRetrieveMultipleFetch;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveMultipleFetch;
        }
    }
}
