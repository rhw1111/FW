using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForDisAssociateCollectionFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForDisAssociateCollectionFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForDisAssociateCollection _crmMessageHandleForDisAssociateCollection;

        public CrmMessageHandleForDisAssociateCollectionFactory(CrmMessageHandleForDisAssociateCollection crmMessageHandleForDisAssociateCollection)
        {
            _crmMessageHandleForDisAssociateCollection = crmMessageHandleForDisAssociateCollection;
        }

        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForDisAssociateCollection;
        }
    }
}
