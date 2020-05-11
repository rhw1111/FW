using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmActionParameterHandleForCrmExecuteEntityFactory), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterHandleForCrmExecuteEntityFactory : IFactory<ICrmActionParameterHandle>
    {
        private CrmActionParameterHandleForCrmExecuteEntity _crmActionParameterHandleForCrmExecuteEntity;

        public CrmActionParameterHandleForCrmExecuteEntityFactory(CrmActionParameterHandleForCrmExecuteEntity crmActionParameterHandleForCrmExecuteEntity)
        {
            _crmActionParameterHandleForCrmExecuteEntity = crmActionParameterHandleForCrmExecuteEntity;
        }
        public ICrmActionParameterHandle Create()
        {
            return _crmActionParameterHandleForCrmExecuteEntity;
        }
    }
}
