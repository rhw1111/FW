using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForBoolFactory), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForBoolFactory : IFactory<ICrmFunctionParameterHandle>
    {
        private CrmFunctionParameterHandleForBool _crmFunctionParameterHandleForBool;

        public CrmFunctionParameterHandleForBoolFactory(CrmFunctionParameterHandleForBool crmFunctionParameterHandleForBool)
        {
            _crmFunctionParameterHandleForBool = crmFunctionParameterHandleForBool;
        }
        public ICrmFunctionParameterHandle Create()
        {
            return _crmFunctionParameterHandleForBool;
        }
    }
}
