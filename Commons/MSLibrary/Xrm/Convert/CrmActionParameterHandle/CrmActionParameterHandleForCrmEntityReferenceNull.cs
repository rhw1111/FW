using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmActionParameterHandleForCrmEntityReferenceNull), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterHandleForCrmEntityReferenceNull : ICrmActionParameterHandle
    {
        public async Task<CrmActionParameterHandleResult> Convert(string name, object parameter)
        {
            var realParameter = parameter as CrmEntityReferenceNull;

            CrmActionParameterHandleResult result = new CrmActionParameterHandleResult()
            {
                Name = $"{name}@odata.bind",
                Value = JToken.Parse("null")
            };
            return await Task.FromResult(result);
        }
    }
}
