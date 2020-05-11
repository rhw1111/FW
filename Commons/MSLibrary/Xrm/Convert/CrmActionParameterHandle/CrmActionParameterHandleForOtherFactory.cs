using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmActionParameterHandleForOtherFactory), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterHandleForOtherFactory : IFactory<ICrmActionParameterHandle>
    {
        private CrmActionParameterHandleForOther _crmActionParameterHandleForOther;

        public CrmActionParameterHandleForOtherFactory(CrmActionParameterHandleForOther crmActionParameterHandleForOther)
        {
            _crmActionParameterHandleForOther = crmActionParameterHandleForOther;
        }
        public ICrmActionParameterHandle Create()
        {
            return _crmActionParameterHandleForOther;
        }
    }
}
