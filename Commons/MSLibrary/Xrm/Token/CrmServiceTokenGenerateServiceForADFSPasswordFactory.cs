using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Token
{
    [Injection(InterfaceType = typeof(CrmServiceTokenGenerateServiceForADFSPasswordFactory), Scope = InjectionScope.Singleton)]
    public class CrmServiceTokenGenerateServiceForADFSPasswordFactory : IFactory<ICrmServiceTokenGenerateService>
    {
        private CrmServiceTokenGenerateServiceForADFSPassword _crmServiceTokenGenerateServiceForADFSPassword;

        public CrmServiceTokenGenerateServiceForADFSPasswordFactory(CrmServiceTokenGenerateServiceForADFSPassword crmServiceTokenGenerateServiceForADFSPassword)
        {
            _crmServiceTokenGenerateServiceForADFSPassword = crmServiceTokenGenerateServiceForADFSPassword;
        }
        public ICrmServiceTokenGenerateService Create()
        {
            return _crmServiceTokenGenerateServiceForADFSPassword;
        }
    }
}
