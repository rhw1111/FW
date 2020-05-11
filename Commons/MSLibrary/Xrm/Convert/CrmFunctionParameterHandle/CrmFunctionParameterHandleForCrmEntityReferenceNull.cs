using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForCrmEntityReferenceNull), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForCrmEntityReferenceNull : ICrmFunctionParameterHandle
    {
        public async Task<CrmFunctionParameterHandleResult> Convert(string name,object parameter)
        {
            var parameterReference = (CrmEntityReferenceNull)parameter;

            CrmFunctionParameterHandleResult result = new CrmFunctionParameterHandleResult();
            result.Name = name;
            result.Value = $"{{'@odata.id':null}}";

            return await Task.FromResult(result);
        }
    }
}
