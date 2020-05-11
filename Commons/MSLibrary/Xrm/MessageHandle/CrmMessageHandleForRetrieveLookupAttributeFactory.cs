using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveLookupAttributeFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveLookupAttributeFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveLookupAttribute _crmMessageHandleForRetrieveLookupAttribute;

        public CrmMessageHandleForRetrieveLookupAttributeFactory(CrmMessageHandleForRetrieveLookupAttribute crmMessageHandleForRetrieveLookupAttribute)
        {
            _crmMessageHandleForRetrieveLookupAttribute = crmMessageHandleForRetrieveLookupAttribute;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveLookupAttribute;
        }
    }
}
