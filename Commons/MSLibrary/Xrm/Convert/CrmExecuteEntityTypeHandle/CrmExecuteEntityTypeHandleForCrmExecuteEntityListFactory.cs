using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle
{
    [Injection(InterfaceType = typeof(CrmExecuteEntityTypeHandleForCrmExecuteEntityListFactory), Scope = InjectionScope.Singleton)]
    public class CrmExecuteEntityTypeHandleForCrmExecuteEntityListFactory : IFactory<ICrmExecuteEntityTypeHandle>
    {
        private CrmExecuteEntityTypeHandleForCrmExecuteEntityList _crmExecuteEntityTypeHandleForCrmExecuteEntityList;

        public CrmExecuteEntityTypeHandleForCrmExecuteEntityListFactory(CrmExecuteEntityTypeHandleForCrmExecuteEntityList crmExecuteEntityTypeHandleForCrmExecuteEntityList)
        {
            _crmExecuteEntityTypeHandleForCrmExecuteEntityList = crmExecuteEntityTypeHandleForCrmExecuteEntityList;
        }
        public ICrmExecuteEntityTypeHandle Create()
        {
            return _crmExecuteEntityTypeHandleForCrmExecuteEntityList;
        }
    }
}
