using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle
{
    [Injection(InterfaceType = typeof(CrmExecuteEntityTypeHandleForCrmExecuteEntityFactory), Scope = InjectionScope.Singleton)]
    public class CrmExecuteEntityTypeHandleForCrmExecuteEntityFactory : IFactory<ICrmExecuteEntityTypeHandle>
    {
        private CrmExecuteEntityTypeHandleForCrmExecuteEntity _crmExecuteEntityTypeHandleForCrmExecuteEntity;

        public CrmExecuteEntityTypeHandleForCrmExecuteEntityFactory(CrmExecuteEntityTypeHandleForCrmExecuteEntity crmExecuteEntityTypeHandleForCrmExecuteEntity)
        {
            _crmExecuteEntityTypeHandleForCrmExecuteEntity = crmExecuteEntityTypeHandleForCrmExecuteEntity;
        }
        public ICrmExecuteEntityTypeHandle Create()
        {
            return _crmExecuteEntityTypeHandleForCrmExecuteEntity;
        }
    }
}
