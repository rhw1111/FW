using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Token
{
    [Injection(InterfaceType = typeof(CrmServiceTokenGenerateServiceForS2SFactory), Scope = InjectionScope.Singleton)]
    public class CrmServiceTokenGenerateServiceForS2SFactory : IFactory<ICrmServiceTokenGenerateService>
    {
        private CrmServiceTokenGenerateServiceForS2S _crmServiceTokenGenerateServiceForS2S;

        public CrmServiceTokenGenerateServiceForS2SFactory(CrmServiceTokenGenerateServiceForS2S crmServiceTokenGenerateServiceForS2S)
        {
            _crmServiceTokenGenerateServiceForS2S = crmServiceTokenGenerateServiceForS2S;
        }
        public ICrmServiceTokenGenerateService Create()
        {
            return _crmServiceTokenGenerateServiceForS2S;
        }
    }
}
