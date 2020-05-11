using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForBatchFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForBatchFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForBatch _crmMessageHandleForBatch;

        public CrmMessageHandleForBatchFactory(CrmMessageHandleForBatch crmMessageHandleForBatch)
        {
            _crmMessageHandleForBatch = crmMessageHandleForBatch;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForBatch;
        }
    }
}
