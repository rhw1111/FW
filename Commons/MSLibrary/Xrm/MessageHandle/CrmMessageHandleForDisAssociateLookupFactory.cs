using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForDisAssociateLookupFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForDisAssociateLookupFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForDisAssociateLookup _crmMessageHandleForDisAssociateLookup;

        public CrmMessageHandleForDisAssociateLookupFactory(CrmMessageHandleForDisAssociateLookup crmMessageHandleForDisAssociateLookup)
        {
            _crmMessageHandleForDisAssociateLookup = crmMessageHandleForDisAssociateLookup;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForDisAssociateLookup;
        }
    }
}
