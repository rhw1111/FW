using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForDateTimeFactory), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForDateTimeFactory : IFactory<ICrmFunctionParameterHandle>
    {
        private CrmFunctionParameterHandleForDateTime _crmFunctionParameterHandleForDateTime;

        public CrmFunctionParameterHandleForDateTimeFactory(CrmFunctionParameterHandleForDateTime crmFunctionParameterHandleForDateTime)
        {
            _crmFunctionParameterHandleForDateTime = crmFunctionParameterHandleForDateTime;
        }
        public ICrmFunctionParameterHandle Create()
        {
            return _crmFunctionParameterHandleForDateTime;
        }
    }
}
