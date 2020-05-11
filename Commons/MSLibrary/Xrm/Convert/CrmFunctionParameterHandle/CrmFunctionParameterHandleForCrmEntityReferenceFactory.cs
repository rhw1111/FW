using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForCrmEntityReferenceFactory), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForCrmEntityReferenceFactory : IFactory<ICrmFunctionParameterHandle>
    {
        private CrmFunctionParameterHandleForCrmEntityReference _crmFunctionParameterHandleForCrmEntityReference;

        public CrmFunctionParameterHandleForCrmEntityReferenceFactory(CrmFunctionParameterHandleForCrmEntityReference crmFunctionParameterHandleForCrmEntityReference)
        {
            _crmFunctionParameterHandleForCrmEntityReference = crmFunctionParameterHandleForCrmEntityReference;
        }
        public ICrmFunctionParameterHandle Create()
        {
            return _crmFunctionParameterHandleForCrmEntityReference;
        }
    }
}
