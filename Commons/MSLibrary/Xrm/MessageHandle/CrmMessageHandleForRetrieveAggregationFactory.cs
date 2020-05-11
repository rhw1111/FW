using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveAggregationFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveAggregationFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveAggregation _crmMessageHandleForRetrieveAggregation;

        public CrmMessageHandleForRetrieveAggregationFactory(CrmMessageHandleForRetrieveAggregation crmMessageHandleForRetrieveAggregation)
        {
            _crmMessageHandleForRetrieveAggregation = crmMessageHandleForRetrieveAggregation;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveAggregation;
        }
    }
}
