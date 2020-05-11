using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForUpsertFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForUpsertFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForUpsert _crmMessageHandleForUpsert;

        public CrmMessageHandleForUpsertFactory(CrmMessageHandleForUpsert crmMessageHandleForUpsert)
        {
            _crmMessageHandleForUpsert = crmMessageHandleForUpsert;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForUpsert;
        }
    }
}
