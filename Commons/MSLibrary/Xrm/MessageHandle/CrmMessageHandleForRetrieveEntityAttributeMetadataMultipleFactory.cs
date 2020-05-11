using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveEntityAttributeMetadataMultipleFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveEntityAttributeMetadataMultipleFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveEntityAttributeMetadataMultiple _crmMessageHandleForRetrieveEntityAttributeMetadataMultiple;

        public CrmMessageHandleForRetrieveEntityAttributeMetadataMultipleFactory(CrmMessageHandleForRetrieveEntityAttributeMetadataMultiple crmMessageHandleForRetrieveEntityAttributeMetadataMultiple)
        {
            _crmMessageHandleForRetrieveEntityAttributeMetadataMultiple = crmMessageHandleForRetrieveEntityAttributeMetadataMultiple;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveEntityAttributeMetadataMultiple;
        }
    }
}
