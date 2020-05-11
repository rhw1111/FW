using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveMultipleUserQueryFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveMultipleUserQueryFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveMultipleUserQuery _crmMessageHandleForRetrieveMultipleUserQuery;

        public CrmMessageHandleForRetrieveMultipleUserQueryFactory(CrmMessageHandleForRetrieveMultipleUserQuery crmMessageHandleForRetrieveMultipleUserQuery)
        {
            _crmMessageHandleForRetrieveMultipleUserQuery = crmMessageHandleForRetrieveMultipleUserQuery;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveMultipleUserQuery;
        }
    }
}
