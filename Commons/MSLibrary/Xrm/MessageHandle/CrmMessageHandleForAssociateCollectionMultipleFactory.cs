using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForAssociateCollectionMultipleFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForAssociateCollectionMultipleFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForAssociateCollectionMultiple _crmMessageHandleForAssociateCollectionMultiple;

        public CrmMessageHandleForAssociateCollectionMultipleFactory(CrmMessageHandleForAssociateCollectionMultiple crmMessageHandleForAssociateCollectionMultiple)
        {
            _crmMessageHandleForAssociateCollectionMultiple = crmMessageHandleForAssociateCollectionMultiple;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForAssociateCollectionMultiple;
        }
    }
}
