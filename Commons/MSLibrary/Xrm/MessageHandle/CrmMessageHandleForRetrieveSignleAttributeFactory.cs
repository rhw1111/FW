using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveSignleAttributeFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveSignleAttributeFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieveSignleAttribute _crmMessageHandleForRetrieveSignleAttribute;

        public CrmMessageHandleForRetrieveSignleAttributeFactory(CrmMessageHandleForRetrieveSignleAttribute crmMessageHandleForRetrieveSignleAttribute)
        {
            _crmMessageHandleForRetrieveSignleAttribute = crmMessageHandleForRetrieveSignleAttribute;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieveSignleAttribute;
        }
    }
}
