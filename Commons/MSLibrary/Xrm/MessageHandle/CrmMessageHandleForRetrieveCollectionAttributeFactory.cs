using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveCollectionAttributeFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveCollectionAttributeFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveCollectionAttribute _crmMessageHandleForRetrieveCollectionAttribute;

        public CrmMessageHandleForRetrieveCollectionAttributeFactory(CrmMessageHandleForRetrieveCollectionAttribute crmMessageHandleForRetrieveCollectionAttribute)
        {
            _crmMessageHandleForRetrieveCollectionAttribute = crmMessageHandleForRetrieveCollectionAttribute;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveCollectionAttribute;
        }
    }
}
