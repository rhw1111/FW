using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveEntityMetadataMultipleFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveEntityMetadataMultipleFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveEntityMetadataMultiple _crmMessageHandleForRetrieveEntityMetadataMultiple;

        public CrmMessageHandleForRetrieveEntityMetadataMultipleFactory(CrmMessageHandleForRetrieveEntityMetadataMultiple crmMessageHandleForRetrieveEntityMetadataMultiple)
        {
            _crmMessageHandleForRetrieveEntityMetadataMultiple = crmMessageHandleForRetrieveEntityMetadataMultiple;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveEntityMetadataMultiple;
        }
    }
}
