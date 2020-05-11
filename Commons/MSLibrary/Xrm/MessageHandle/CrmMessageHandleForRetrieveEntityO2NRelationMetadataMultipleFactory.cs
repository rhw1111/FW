using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveEntityO2NRelationMetadataMultipleFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveEntityO2NRelationMetadataMultipleFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveEntityO2NRelationMetadataMultiple _crmMessageHandleForRetrieveEntityO2NRelationMetadataMultiple;

        public CrmMessageHandleForRetrieveEntityO2NRelationMetadataMultipleFactory(CrmMessageHandleForRetrieveEntityO2NRelationMetadataMultiple crmMessageHandleForRetrieveEntityO2NRelationMetadataMultiple)
        {
            _crmMessageHandleForRetrieveEntityO2NRelationMetadataMultiple = crmMessageHandleForRetrieveEntityO2NRelationMetadataMultiple;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveEntityO2NRelationMetadataMultiple;
        }
    }
}
