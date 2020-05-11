using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveMultipleFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveMultipleFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveMultiple _crmMessageHandleForRetrieveMultiple;

        public CrmMessageHandleForRetrieveMultipleFactory(CrmMessageHandleForRetrieveMultiple crmMessageHandleForRetrieveMultiple)
        {
            _crmMessageHandleForRetrieveMultiple = crmMessageHandleForRetrieveMultiple;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveMultiple;
        }
    }
}
