using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveEntityAttributeMetadataFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveEntityAttributeMetadataFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveEntityAttributeMetadata _crmMessageHandleForRetrieveEntityAttributeMetadata;

        public CrmMessageHandleForRetrieveEntityAttributeMetadataFactory(CrmMessageHandleForRetrieveEntityAttributeMetadata crmMessageHandleForRetrieveEntityAttributeMetadata)
        {
            _crmMessageHandleForRetrieveEntityAttributeMetadata = crmMessageHandleForRetrieveEntityAttributeMetadata;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveEntityAttributeMetadata;
        }
    }
}
