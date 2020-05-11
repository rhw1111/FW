using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    public interface ICrmFunctionParameterHandle
    {
        Task<CrmFunctionParameterHandleResult> Convert(string name,object parameter);
    }

    public class CrmFunctionParameterHandleResult
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
