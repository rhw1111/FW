using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle
{
    public interface ICrmExecuteEntityTypeHandle
    {
        Task<CrmExecuteEntityTypeHandleResult> Convert(string name,object value);
    }

    public class CrmExecuteEntityTypeHandleResult
    {
        public string Name { get; set; }

        public JToken Value { get; set; }
    }
}
