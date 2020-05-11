using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForEnumTypeFactory), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForEnumTypeFactory : IFactory<ICrmFunctionParameterHandle>
    {
        private CrmFunctionParameterHandleForEnumType _crmFunctionParameterHandleForEnumType;

        public CrmFunctionParameterHandleForEnumTypeFactory(CrmFunctionParameterHandleForEnumType crmFunctionParameterHandleForEnumType)
        {
            _crmFunctionParameterHandleForEnumType = crmFunctionParameterHandleForEnumType;
        }

        public ICrmFunctionParameterHandle Create()
        {
            return _crmFunctionParameterHandleForEnumType;
        }
    }
}
