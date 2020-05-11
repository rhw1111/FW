using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmActionParameterHandleForCrmExecuteEntityListFactory), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterHandleForCrmExecuteEntityListFactory : IFactory<ICrmActionParameterHandle>
    {
        private CrmActionParameterHandleForCrmExecuteEntityList _crmActionParameterHandleForCrmExecuteEntityList;

        public CrmActionParameterHandleForCrmExecuteEntityListFactory(CrmActionParameterHandleForCrmExecuteEntityList crmActionParameterHandleForCrmExecuteEntityList)
        {
            _crmActionParameterHandleForCrmExecuteEntityList = crmActionParameterHandleForCrmExecuteEntityList;
        }
        public ICrmActionParameterHandle Create()
        {
            return _crmActionParameterHandleForCrmExecuteEntityList;
        }
    }
}
