using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveCollectionAttributeReferenceFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveCollectionAttributeReferenceFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveCollectionAttributeReference _crmMessageHandleForRetrieveCollectionAttributeReference;

        public CrmMessageHandleForRetrieveCollectionAttributeReferenceFactory(CrmMessageHandleForRetrieveCollectionAttributeReference crmMessageHandleForRetrieveCollectionAttributeReference)
        {
            _crmMessageHandleForRetrieveCollectionAttributeReference = crmMessageHandleForRetrieveCollectionAttributeReference;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveCollectionAttributeReference;
        }
    }
}
