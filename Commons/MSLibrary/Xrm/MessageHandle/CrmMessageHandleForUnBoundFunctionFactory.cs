using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForUnBoundFunctionFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForUnBoundFunctionFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForUnBoundFunction _crmMessageHandleForUnBoundFunction;

        public CrmMessageHandleForUnBoundFunctionFactory(CrmMessageHandleForUnBoundFunction crmMessageHandleForUnBoundFunction)
        {
            _crmMessageHandleForUnBoundFunction = crmMessageHandleForUnBoundFunction;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForUnBoundFunction;
        }
    }
}
