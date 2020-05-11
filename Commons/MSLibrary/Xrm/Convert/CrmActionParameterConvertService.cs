using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Xrm.Convert.CrmActionParameterHandle;

namespace MSLibrary.Xrm.Convert
{
    [Injection(InterfaceType = typeof(ICrmActionParameterConvertService), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterConvertService : ICrmActionParameterConvertService
    {
        private ICrmActionParameterHandle _crmActionParameterHandle;

        public CrmActionParameterConvertService(ICrmActionParameterHandle crmActionParameterHandle)
        {
            _crmActionParameterHandle = crmActionParameterHandle;
        }

        public async Task<JToken> Convert(object parameter)
        {
            var handleResult = await _crmActionParameterHandle.Convert(string.Empty, parameter);
            return handleResult.Value;
        }
    }
}
