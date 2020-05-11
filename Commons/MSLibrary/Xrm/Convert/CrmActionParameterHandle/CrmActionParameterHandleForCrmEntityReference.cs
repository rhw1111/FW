using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmActionParameterHandleForCrmEntityReference), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterHandleForCrmEntityReference : ICrmActionParameterHandle
    {
        public async Task<CrmActionParameterHandleResult> Convert(string name, object parameter)
        {
            var realParameter = parameter as CrmEntityReference;


            CrmActionParameterHandleResult result = new CrmActionParameterHandleResult()
            {
                Name = $"{name}@odata.bind",
                Value = JToken.FromObject($"/{realParameter.EntityName.ToPlural()}({realParameter.Id.ToString()})")
            };
            return await Task.FromResult(result);
        }
    }
}
