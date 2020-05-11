using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveGlobalOptionSetMetadataFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveGlobalOptionSetMetadataFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveGlobalOptionSetMetadata _crmMessageHandleForRetrieveGlobalOptionSetMetadata;
        public CrmMessageHandleForRetrieveGlobalOptionSetMetadataFactory(CrmMessageHandleForRetrieveGlobalOptionSetMetadata crmMessageHandleForRetrieveGlobalOptionSetMetadata)
        {
            _crmMessageHandleForRetrieveGlobalOptionSetMetadata = crmMessageHandleForRetrieveGlobalOptionSetMetadata;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveGlobalOptionSetMetadata;
        }
    }
}
