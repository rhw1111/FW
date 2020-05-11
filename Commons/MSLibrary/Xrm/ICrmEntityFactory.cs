using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm
{
    /// <summary>
    /// Crm实体记录工厂
    /// </summary>
    public interface ICrmEntityFactory
    {
        CrmEntity Create(string entityName, JObject entity);
    }
}
