using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveCollectionAttributeSavedQueryFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveCollectionAttributeSavedQueryFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveCollectionAttributeSavedQuery _crmMessageHandleForRetrieveCollectionAttributeSavedQuery;

        public CrmMessageHandleForRetrieveCollectionAttributeSavedQueryFactory(CrmMessageHandleForRetrieveCollectionAttributeSavedQuery crmMessageHandleForRetrieveCollectionAttributeSavedQuery)
        {
            _crmMessageHandleForRetrieveCollectionAttributeSavedQuery = crmMessageHandleForRetrieveCollectionAttributeSavedQuery;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveCollectionAttributeSavedQuery;
        }
    }
}
