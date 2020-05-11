using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForBoundActionFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForBoundActionFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForBoundAction _crmMessageHandleForBoundAction;

        public CrmMessageHandleForBoundActionFactory(CrmMessageHandleForBoundAction crmMessageHandleForBoundAction)
        {
            _crmMessageHandleForBoundAction = crmMessageHandleForBoundAction;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForBoundAction;
        }
    }
}
