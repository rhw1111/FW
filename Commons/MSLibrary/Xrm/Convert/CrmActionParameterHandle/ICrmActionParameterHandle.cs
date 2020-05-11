using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    public interface ICrmActionParameterHandle
    {
        Task<CrmActionParameterHandleResult> Convert(string name,object parameter);
    }

    public class CrmActionParameterHandleResult
    {
        public string Name { get; set; }

        public JToken Value { get; set; }
    }
}
