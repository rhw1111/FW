using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveN2NRelationMetadataFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveN2NRelationMetadataFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveN2NRelationMetadata _crmMessageHandleForRetrieveN2NRelationMetadata;

        public CrmMessageHandleForRetrieveN2NRelationMetadataFactory(CrmMessageHandleForRetrieveN2NRelationMetadata crmMessageHandleForRetrieveN2NRelationMetadata)
        {
            _crmMessageHandleForRetrieveN2NRelationMetadata = crmMessageHandleForRetrieveN2NRelationMetadata;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveN2NRelationMetadata;
        }
    }
}
