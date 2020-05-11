using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmFunctionParameterHandleForBool), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterHandleForBool : ICrmFunctionParameterHandle
    {
        public async Task<CrmFunctionParameterHandleResult> Convert(string name,object parameter)
        {
            CrmFunctionParameterHandleResult result = new CrmFunctionParameterHandleResult();
            result.Name = name;
            result.Value= $"{parameter.ToString()}";

            return await Task.FromResult(result);
        }
    }
}
