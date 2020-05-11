using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForUnBoundActionFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForUnBoundActionFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForUnBoundAction _crmMessageHandleForUnBoundAction;

        public CrmMessageHandleForUnBoundActionFactory(CrmMessageHandleForUnBoundAction crmMessageHandleForUnBoundAction)
        {
            _crmMessageHandleForUnBoundAction = crmMessageHandleForUnBoundAction;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForUnBoundAction;
        }
    }
}
