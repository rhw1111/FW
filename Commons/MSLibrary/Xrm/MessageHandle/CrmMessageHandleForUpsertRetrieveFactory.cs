using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForUpsertRetrieveFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForUpsertRetrieveFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForUpsertRetrieve _crmMessageHandleForUpsertRetrieve;

        public CrmMessageHandleForUpsertRetrieveFactory(CrmMessageHandleForUpsertRetrieve crmMessageHandleForUpsertRetrieve)
        {
            _crmMessageHandleForUpsertRetrieve = crmMessageHandleForUpsertRetrieve;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForUpsertRetrieve;
        }
    }
}
