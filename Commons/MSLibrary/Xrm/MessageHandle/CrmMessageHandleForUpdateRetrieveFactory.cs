using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForUpdateRetrieveFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForUpdateRetrieveFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForUpdateRetrieve _crmMessageHandleForUpdateRetrieve;

        public CrmMessageHandleForUpdateRetrieveFactory(CrmMessageHandleForUpdateRetrieve crmMessageHandleForUpdateRetrieve)
        {
            _crmMessageHandleForUpdateRetrieve = crmMessageHandleForUpdateRetrieve;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForUpdateRetrieve;
        }
    }
}
