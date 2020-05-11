using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm.Convert.CrmRetrieveJTokenHandle
{
    /// <summary>
    /// Crm查询结果JToken处理
    /// </summary>
    public interface ICrmRetrieveJTokenHandle
    {
        Task<object> Execute(JToken json,Dictionary<string,object> extensionParameters=null);
    }
}
