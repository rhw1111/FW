using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveO2NRelationMetadataMultipleFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveO2NRelationMetadataMultipleFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveO2NRelationMetadataMultiple _crmMessageHandleForRetrieveO2NRelationMetadataMultiple;

        public CrmMessageHandleForRetrieveO2NRelationMetadataMultipleFactory(CrmMessageHandleForRetrieveO2NRelationMetadataMultiple crmMessageHandleForRetrieveO2NRelationMetadataMultiple)
        {
            _crmMessageHandleForRetrieveO2NRelationMetadataMultiple = crmMessageHandleForRetrieveO2NRelationMetadataMultiple;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveO2NRelationMetadataMultiple;
        }
    }
}
