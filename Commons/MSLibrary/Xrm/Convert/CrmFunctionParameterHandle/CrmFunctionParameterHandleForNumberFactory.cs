using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForNumberFactory), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForNumberFactory : IFactory<ICrmFunctionParameterHandle>
    {
        private CrmFunctionParameterHandleForNumber _crmFunctionParameterHandleForNumber;

        public CrmFunctionParameterHandleForNumberFactory(CrmFunctionParameterHandleForNumber crmFunctionParameterHandleForNumber)
        {
            _crmFunctionParameterHandleForNumber = crmFunctionParameterHandleForNumber;
        }
        public ICrmFunctionParameterHandle Create()
        {
            return _crmFunctionParameterHandleForNumber;
        }
    }
}
