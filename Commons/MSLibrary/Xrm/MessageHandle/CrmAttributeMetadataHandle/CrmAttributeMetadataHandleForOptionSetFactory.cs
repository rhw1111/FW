using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle.CrmAttributeMetadataHandle
{
    [Injection(InterfaceType = typeof(CrmAttributeMetadataHandleForOptionSetFactory), Scope = InjectionScope.Singleton)]
    public class CrmAttributeMetadataHandleForOptionSetFactory : IFactory<ICrmAttributeMetadataHandle>
    {
        private CrmAttributeMetadataHandleForOptionSet _crmAttributeMetadataHandleForOptionSet;

        public CrmAttributeMetadataHandleForOptionSetFactory(CrmAttributeMetadataHandleForOptionSet crmAttributeMetadataHandleForOptionSet)
        {
            _crmAttributeMetadataHandleForOptionSet = crmAttributeMetadataHandleForOptionSet;
        }
        public ICrmAttributeMetadataHandle Create()
        {
            return _crmAttributeMetadataHandleForOptionSet;
        }
    }
}
