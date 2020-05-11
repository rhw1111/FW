using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveO2NRelationMetadataFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveO2NRelationMetadataFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveO2NRelationMetadata _crmMessageHandleForRetrieveO2NRelationMetadata;
        public CrmMessageHandleForRetrieveO2NRelationMetadataFactory(CrmMessageHandleForRetrieveO2NRelationMetadata crmMessageHandleForRetrieveO2NRelationMetadata)
        {
            _crmMessageHandleForRetrieveO2NRelationMetadata = crmMessageHandleForRetrieveO2NRelationMetadata;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveO2NRelationMetadata;
        }
    }
}
