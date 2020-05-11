using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForString), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForString : ICrmFunctionParameterHandle
    {
        public async Task<CrmFunctionParameterHandleResult> Convert(string name,object parameter)
        {
            CrmFunctionParameterHandleResult result = new CrmFunctionParameterHandleResult();
            result.Name = name;
            result.Value = $"'{parameter.ToString()}'";
            return await Task.FromResult(result);
        }
    }
}
