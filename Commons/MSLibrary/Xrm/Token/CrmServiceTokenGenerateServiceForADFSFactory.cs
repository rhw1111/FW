using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Token
{
    [Injection(InterfaceType = typeof(CrmServiceTokenGenerateServiceForADFSFactory), Scope = InjectionScope.Singleton)]
    public class CrmServiceTokenGenerateServiceForADFSFactory : IFactory<ICrmServiceTokenGenerateService>
    {
        private CrmServiceTokenGenerateServiceForADFS _crmServiceTokenGenerateServiceForADFS;

        public CrmServiceTokenGenerateServiceForADFSFactory(CrmServiceTokenGenerateServiceForADFS crmServiceTokenGenerateServiceForADFS)
        {
            _crmServiceTokenGenerateServiceForADFS = crmServiceTokenGenerateServiceForADFS;
        }
        public ICrmServiceTokenGenerateService Create()
        {
            return _crmServiceTokenGenerateServiceForADFS;
        }
    }
}
