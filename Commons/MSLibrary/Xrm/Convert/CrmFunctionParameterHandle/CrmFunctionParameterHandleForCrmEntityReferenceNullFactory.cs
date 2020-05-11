using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForCrmEntityReferenceNullFactory), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForCrmEntityReferenceNullFactory : IFactory<ICrmFunctionParameterHandle>
    {
        private CrmFunctionParameterHandleForCrmEntityReferenceNull _crmFunctionParameterHandleForCrmEntityReferenceNull;

        public CrmFunctionParameterHandleForCrmEntityReferenceNullFactory(CrmFunctionParameterHandleForCrmEntityReferenceNull crmFunctionParameterHandleForCrmEntityReferenceNull)
        {
            _crmFunctionParameterHandleForCrmEntityReferenceNull = crmFunctionParameterHandleForCrmEntityReferenceNull;
        }
        public ICrmFunctionParameterHandle Create()
        {
            return _crmFunctionParameterHandleForCrmEntityReferenceNull;
        }
    }
}
