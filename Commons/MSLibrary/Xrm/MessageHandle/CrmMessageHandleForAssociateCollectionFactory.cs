using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForAssociateCollectionFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForAssociateCollectionFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForAssociateCollection _crmMessageHandleForAssociateCollection;

        public CrmMessageHandleForAssociateCollectionFactory(CrmMessageHandleForAssociateCollection crmMessageHandleForAssociateCollection)
        {
            _crmMessageHandleForAssociateCollection = crmMessageHandleForAssociateCollection;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForAssociateCollection;
        }
    }
}
