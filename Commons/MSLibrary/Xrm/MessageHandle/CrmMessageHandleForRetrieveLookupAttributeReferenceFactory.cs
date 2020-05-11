using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveLookupAttributeReferenceFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveLookupAttributeReferenceFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveLookupAttributeReference _crmMessageHandleForRetrieveLookupAttributeReference;

        public CrmMessageHandleForRetrieveLookupAttributeReferenceFactory(CrmMessageHandleForRetrieveLookupAttributeReference crmMessageHandleForRetrieveLookupAttributeReference)
        {
            _crmMessageHandleForRetrieveLookupAttributeReference = crmMessageHandleForRetrieveLookupAttributeReference;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveLookupAttributeReference;
        }
    }
}
