using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveEntityMetadataFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveEntityMetadataFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveEntityMetadata _crmMessageHandleForRetrieveEntityMetadata;
        public CrmMessageHandleForRetrieveEntityMetadataFactory(CrmMessageHandleForRetrieveEntityMetadata crmMessageHandleForRetrieveEntityMetadata)
        {
            _crmMessageHandleForRetrieveEntityMetadata = crmMessageHandleForRetrieveEntityMetadata;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveEntityMetadata;
        }
    }
}
