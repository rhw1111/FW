using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForEnumType), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForEnumType : ICrmFunctionParameterHandle
    {
        public async Task<CrmFunctionParameterHandleResult> Convert(string name,object parameter)
        {
            var realparameter = (CrmFunctionEnumTypeParameter)parameter;

            CrmFunctionParameterHandleResult result = new CrmFunctionParameterHandleResult();
            result.Name = name;
            result.Value = $"Microsoft.Dynamics.CRM.{realparameter.Name}'{realparameter.Value}'";
            return await Task.FromResult(result);
        }
    }
}
