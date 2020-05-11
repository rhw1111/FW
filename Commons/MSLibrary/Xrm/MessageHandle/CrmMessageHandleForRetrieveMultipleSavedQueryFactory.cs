using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveMultipleSavedQueryFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveMultipleSavedQueryFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveMultipleSavedQuery _crmMessageHandleForRetrieveMultipleSavedQuery;
        public CrmMessageHandleForRetrieveMultipleSavedQueryFactory(CrmMessageHandleForRetrieveMultipleSavedQuery crmMessageHandleForRetrieveMultipleSavedQuery)
        {
            _crmMessageHandleForRetrieveMultipleSavedQuery = crmMessageHandleForRetrieveMultipleSavedQuery;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveMultipleSavedQuery;
        }
    }
}
