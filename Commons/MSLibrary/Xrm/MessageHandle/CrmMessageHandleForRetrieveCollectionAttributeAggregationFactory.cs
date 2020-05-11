using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveCollectionAttributeAggregationFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveCollectionAttributeAggregationFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveCollectionAttributeAggregation _crmMessageHandleForRetrieveCollectionAttributeAggregation;

        public CrmMessageHandleForRetrieveCollectionAttributeAggregationFactory(CrmMessageHandleForRetrieveCollectionAttributeAggregation crmMessageHandleForRetrieveCollectionAttributeAggregation)
        {
            _crmMessageHandleForRetrieveCollectionAttributeAggregation = crmMessageHandleForRetrieveCollectionAttributeAggregation;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveCollectionAttributeAggregation;
        }
    }
}
