using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Xrm.Convert.CrmFunctionParameterHandle;

namespace MSLibrary.Xrm.Convert
{
    [Injection(InterfaceType = typeof(ICrmFunctionParameterConvertService), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterConvertService : ICrmFunctionParameterConvertService
    {
        private ICrmFunctionParameterHandle _crmFunctionParameterHandle;

        public CrmFunctionParameterConvertService(ICrmFunctionParameterHandle crmFunctionParameterHandle)
        {
            _crmFunctionParameterHandle = crmFunctionParameterHandle;
        }
        public async Task<string> Convert(object parameter)
        {
            var result= await _crmFunctionParameterHandle.Convert(string.Empty,parameter);
            return result.Value;
        }
    }
}
