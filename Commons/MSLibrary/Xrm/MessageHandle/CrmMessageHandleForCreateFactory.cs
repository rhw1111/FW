using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForCreateFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForCreateFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForCreate _crmMessageHandleForCreate;

        public CrmMessageHandleForCreateFactory(CrmMessageHandleForCreate crmMessageHandleForCreate)
        {
            _crmMessageHandleForCreate = crmMessageHandleForCreate;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForCreate;
        }
    }
}
