using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForStringFactory), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForStringFactory : IFactory<ICrmFunctionParameterHandle>
    {
        private CrmFunctionParameterHandleForString _crmFunctionParameterHandleForString;

        public CrmFunctionParameterHandleForStringFactory(CrmFunctionParameterHandleForString crmFunctionParameterHandleForString)
        {
            _crmFunctionParameterHandleForString = crmFunctionParameterHandleForString;
        }
        public ICrmFunctionParameterHandle Create()
        {
            return _crmFunctionParameterHandleForString;
        }
    }
}
