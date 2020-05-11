using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Token
{
    [Injection(InterfaceType = typeof(CrmServiceTokenGenerateServiceForADFactory), Scope = InjectionScope.Singleton)]
    public class CrmServiceTokenGenerateServiceForADFactory : IFactory<ICrmServiceTokenGenerateService>
    {
        private CrmServiceTokenGenerateServiceForAD _crmServiceTokenGenerateServiceForAD;

        public CrmServiceTokenGenerateServiceForADFactory(CrmServiceTokenGenerateServiceForAD crmServiceTokenGenerateServiceForAD)
        {
            _crmServiceTokenGenerateServiceForAD = crmServiceTokenGenerateServiceForAD;
        }
        public ICrmServiceTokenGenerateService Create()
        {
            return _crmServiceTokenGenerateServiceForAD;
        }
    }
}
