using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveRelationMetadataMultipleFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveRelationMetadataMultipleFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveRelationMetadataMultiple _crmMessageHandleForRetrieveRelationMetadataMultiple;

        public CrmMessageHandleForRetrieveRelationMetadataMultipleFactory(CrmMessageHandleForRetrieveRelationMetadataMultiple crmMessageHandleForRetrieveRelationMetadataMultiple)
        {
            _crmMessageHandleForRetrieveRelationMetadataMultiple = crmMessageHandleForRetrieveRelationMetadataMultiple;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveRelationMetadataMultiple;
        }
    }
}
