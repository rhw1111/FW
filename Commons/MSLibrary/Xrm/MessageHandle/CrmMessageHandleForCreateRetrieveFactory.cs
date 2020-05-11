using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForCreateRetrieveFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForCreateRetrieveFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForCreateRetrieve _crmMessageHandleForCreateRetrieve;

        public CrmMessageHandleForCreateRetrieveFactory(CrmMessageHandleForCreateRetrieve crmMessageHandleForCreateRetrieve)
        {
            _crmMessageHandleForCreateRetrieve = crmMessageHandleForCreateRetrieve;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForCreateRetrieve;
        }
    }
}
