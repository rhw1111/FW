using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveCollectionAttributeUserQueryFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveCollectionAttributeUserQueryFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveCollectionAttributeUserQuery _crmMessageHandleForRetrieveCollectionAttributeUserQuery;

        public CrmMessageHandleForRetrieveCollectionAttributeUserQueryFactory(CrmMessageHandleForRetrieveCollectionAttributeUserQuery crmMessageHandleForRetrieveCollectionAttributeUserQuery)
        {
            _crmMessageHandleForRetrieveCollectionAttributeUserQuery = crmMessageHandleForRetrieveCollectionAttributeUserQuery;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveCollectionAttributeUserQuery;
        }
    }
}
