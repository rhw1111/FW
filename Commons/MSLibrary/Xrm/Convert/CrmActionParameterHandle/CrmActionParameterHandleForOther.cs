using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmActionParameterHandleForOther), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterHandleForOther : ICrmActionParameterHandle
    {
        public async Task<CrmActionParameterHandleResult> Convert(string name, object parameter)
        {
            CrmActionParameterHandleResult result = new CrmActionParameterHandleResult()
            {
                Name = name,
                Value = JToken.FromObject(parameter)
            };
            return await Task.FromResult(result);
        }
    }
}
