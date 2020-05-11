using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle;

namespace MSLibrary.Xrm.Convert
{
    [Injection(InterfaceType = typeof(ICrmExecuteEntityConvertJObjectService), Scope = InjectionScope.Singleton)]
    public class CrmExecuteEntityConvertJObjectService : ICrmExecuteEntityConvertJObjectService
    {
        private ICrmExecuteEntityTypeHandle _crmExecuteEntityTypeHandle;
        
        public CrmExecuteEntityConvertJObjectService(ICrmExecuteEntityTypeHandle crmExecuteEntityTypeHandle)
        {
            _crmExecuteEntityTypeHandle = crmExecuteEntityTypeHandle;
        }
        public async Task<JObject> Convert(CrmExecuteEntity entity)
        {
            var convertResult=await _crmExecuteEntityTypeHandle.Convert(string.Empty, entity);
            var result = JsonConvert.DeserializeObject<JObject>(convertResult.Value.ToString());
            return result;
        }
    }
}
