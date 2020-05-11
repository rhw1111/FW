using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;


namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForRetrieve _crmMessageHandleForRetrieve;

        public CrmMessageHandleForRetrieveFactory(CrmMessageHandleForRetrieve crmMessageHandleForRetrieve)
        {
            _crmMessageHandleForRetrieve = crmMessageHandleForRetrieve;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForRetrieve;
        }
    }
}
