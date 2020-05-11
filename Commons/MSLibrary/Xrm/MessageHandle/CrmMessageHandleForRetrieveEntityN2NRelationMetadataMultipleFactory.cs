using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveEntityN2NRelationMetadataMultipleFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveEntityN2NRelationMetadataMultipleFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveEntityN2NRelationMetadataMultiple _crmMessageHandleForRetrieveEntityN2NRelationMetadataMultiple;

        public CrmMessageHandleForRetrieveEntityN2NRelationMetadataMultipleFactory(CrmMessageHandleForRetrieveEntityN2NRelationMetadataMultiple crmMessageHandleForRetrieveEntityN2NRelationMetadataMultiple)
        {
            _crmMessageHandleForRetrieveEntityN2NRelationMetadataMultiple = crmMessageHandleForRetrieveEntityN2NRelationMetadataMultiple;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveEntityN2NRelationMetadataMultiple;
        }
    }
}
