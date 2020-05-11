using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveRelationMetadataFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveRelationMetadataFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveRelationMetadata _crmMessageHandleForRetrieveRelationMetadata;

        public CrmMessageHandleForRetrieveRelationMetadataFactory(CrmMessageHandleForRetrieveRelationMetadata crmMessageHandleForRetrieveRelationMetadata)
        {
            _crmMessageHandleForRetrieveRelationMetadata = crmMessageHandleForRetrieveRelationMetadata;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveRelationMetadata;
        }
    }
}
