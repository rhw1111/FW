using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForFileAttributeDeleteDataFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForFileAttributeDeleteDataFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForFileAttributeDeleteData _crmMessageHandleForFileAttributeDeleteData;

        public CrmMessageHandleForFileAttributeDeleteDataFactory(CrmMessageHandleForFileAttributeDeleteData crmMessageHandleForFileAttributeDeleteData)
        {
            _crmMessageHandleForFileAttributeDeleteData = crmMessageHandleForFileAttributeDeleteData;

        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForFileAttributeDeleteData;
        }
    }
}
