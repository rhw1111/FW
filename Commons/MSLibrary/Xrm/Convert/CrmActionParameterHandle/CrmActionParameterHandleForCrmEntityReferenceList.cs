using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmActionParameterHandleForCrmEntityReferenceList), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterHandleForCrmEntityReferenceList : ICrmActionParameterHandle
    {
        private ICrmActionParameterHandle _crmActionParameterHandle;

        public CrmActionParameterHandleForCrmEntityReferenceList(ICrmActionParameterHandle crmActionParameterHandle)
        {
            _crmActionParameterHandle = crmActionParameterHandle;
        }

        public async Task<CrmActionParameterHandleResult> Convert(string name, object parameter)
        {     
            IList<CrmEntityReference> realParameter = parameter as IList<CrmEntityReference>;
            List<JToken> valueList = new List<JToken>();
            foreach (var item in realParameter)
            {
                if (item == null)
                {
                    valueList.Add(JToken.Parse("null"));
                    continue;
                }

                var handleResult = await _crmActionParameterHandle.Convert(string.Empty, item);
                valueList.Add(handleResult.Value);
            }
            CrmActionParameterHandleResult result = new CrmActionParameterHandleResult();
            result.Name = name;
            result.Value = JsonSerializerHelper.Deserialize<JToken>(JsonSerializerHelper.Serializer(valueList));
            return result;
        }
    }
}
