using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForDeleteFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForDeleteFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForDelete _crmMessageHandleForDelete;

        public CrmMessageHandleForDeleteFactory(CrmMessageHandleForDelete crmMessageHandleForDelete)
        {
            _crmMessageHandleForDelete = crmMessageHandleForDelete;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForDelete;
        }
    }
}
