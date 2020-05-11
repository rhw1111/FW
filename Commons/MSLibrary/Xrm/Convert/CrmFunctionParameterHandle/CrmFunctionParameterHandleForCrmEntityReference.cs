using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForCrmEntityReference), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForCrmEntityReference : ICrmFunctionParameterHandle
    {

        public async Task<CrmFunctionParameterHandleResult> Convert(string name,object parameter)
        {
            var parameterReference = (CrmEntityReference)parameter;

            CrmFunctionParameterHandleResult result = new CrmFunctionParameterHandleResult();
            result.Name = name;
            result.Value = $"{{'@odata.id':'{parameterReference.EntityName.ToPlural()}({parameterReference.Id.ToString()})'}}";

            return await Task.FromResult(result);
        }
    }
}
