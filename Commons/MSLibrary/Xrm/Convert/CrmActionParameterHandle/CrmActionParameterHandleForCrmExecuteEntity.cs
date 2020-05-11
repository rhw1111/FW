using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmActionParameterHandleForCrmExecuteEntity), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterHandleForCrmExecuteEntity : ICrmActionParameterHandle
    {
        private ICrmActionParameterHandle _crmActionParameterHandle;

        public CrmActionParameterHandleForCrmExecuteEntity(ICrmActionParameterHandle crmActionParameterHandle)
        {
            _crmActionParameterHandle = crmActionParameterHandle;
        }

        public async Task<CrmActionParameterHandleResult> Convert(string name,object parameter)
        {
            CrmExecuteEntity realParameter = parameter as CrmExecuteEntity;

            JObject valueResult = new JObject();

            if (realParameter.Id != Guid.Empty)
            {
                if (realParameter.IsActivity)
                {
                    valueResult["activityid"] = JToken.FromObject(realParameter);
                }
                else
                {
                    valueResult[$"{realParameter.EntityName}id"] = JToken.FromObject(realParameter);
                }
            }

            valueResult["@odata.type"] = JToken.FromObject($"Microsoft.Dynamics.CRM.{realParameter.EntityName}");

            foreach (var attributeItem in realParameter.Attributes)
            {
                var attributeResult = await _crmActionParameterHandle.Convert(attributeItem.Key, attributeItem.Value);
                valueResult[attributeResult.Name] = attributeResult.Value;
            }

            CrmActionParameterHandleResult result = new CrmActionParameterHandleResult()
            {
                Name = name,
                Value = valueResult
            };
            return result;
        }
    }
}
