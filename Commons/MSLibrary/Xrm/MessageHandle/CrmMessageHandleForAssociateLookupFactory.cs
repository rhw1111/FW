using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForAssociateLookupFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForAssociateLookupFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForAssociateLookup _crmMessageHandleForAssociateLookup;

        public CrmMessageHandleForAssociateLookupFactory(CrmMessageHandleForAssociateLookup crmMessageHandleForAssociateLookup)
        {
            _crmMessageHandleForAssociateLookup = crmMessageHandleForAssociateLookup;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForAssociateLookup;
        }
    }
}
