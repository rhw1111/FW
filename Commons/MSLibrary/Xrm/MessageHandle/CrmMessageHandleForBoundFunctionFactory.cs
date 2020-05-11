using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForBoundFunctionFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForBoundFunctionFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForBoundFunction _crmMessageHandleForBoundFunction;

        public CrmMessageHandleForBoundFunctionFactory(CrmMessageHandleForBoundFunction crmMessageHandleForBoundFunction)
        {
            _crmMessageHandleForBoundFunction = crmMessageHandleForBoundFunction;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForBoundFunction;
        }
    }
}
