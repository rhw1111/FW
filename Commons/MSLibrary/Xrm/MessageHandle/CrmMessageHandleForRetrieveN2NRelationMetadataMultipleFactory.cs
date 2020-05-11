using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveN2NRelationMetadataMultipleFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveN2NRelationMetadataMultipleFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveN2NRelationMetadataMultiple _crmMessageHandleForRetrieveN2NRelationMetadataMultiple;

        public CrmMessageHandleForRetrieveN2NRelationMetadataMultipleFactory(CrmMessageHandleForRetrieveN2NRelationMetadataMultiple crmMessageHandleForRetrieveN2NRelationMetadataMultiple)
        {
            _crmMessageHandleForRetrieveN2NRelationMetadataMultiple = crmMessageHandleForRetrieveN2NRelationMetadataMultiple;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveN2NRelationMetadataMultiple;
        }
    }
}
