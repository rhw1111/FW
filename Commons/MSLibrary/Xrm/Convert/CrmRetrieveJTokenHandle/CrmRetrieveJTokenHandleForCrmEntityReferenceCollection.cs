using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Convert.CrmRetrieveJTokenHandle
{
    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntityReferenceCollection), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntityReferenceCollection : ICrmRetrieveJTokenHandle
    {
        private CrmRetrieveJTokenHandleForCrmEntityReferenceListFactory _crmRetrieveJTokenHandleForCrmEntityReferenceListFactory;
        public CrmRetrieveJTokenHandleForCrmEntityReferenceCollection(CrmRetrieveJTokenHandleForCrmEntityReferenceListFactory crmRetrieveJTokenHandleForCrmEntityReferenceListFactory)
        {
            _crmRetrieveJTokenHandleForCrmEntityReferenceListFactory = crmRetrieveJTokenHandleForCrmEntityReferenceListFactory;
        }

        public async Task<object> Execute(JToken json, Dictionary<string, object> extensionParameters = null)
        {
            CrmEntityReferenceCollection result = new CrmEntityReferenceCollection();

            JToken valueJToken = json["value"];

            if (valueJToken == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmJTokenConvertEntityNotFoundAttribute,
                    DefaultFormatting = "在Crm的JToken转换服务{0}中，传入的JToken中找不到属性{1}，实体名称为{2}，JToken值为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, "Value", string.Empty, json.ToString() }
                };

                throw new UtilityException((int)Errors.CrmJTokenConvertEntityNotFoundAttribute, fragment);
            }

            var entityReferenceList = await _crmRetrieveJTokenHandleForCrmEntityReferenceListFactory.Create().Execute(valueJToken);

            result.Results = (List<CrmEntityReference>)entityReferenceList;

            if (json["@odata.nextLink"] != null)
            {
                result.MoreRecords = true;
                result.NextLinkExpression = json["@odata.nextLink"].Value<string>();
            }
            else
            {
                result.MoreRecords = false;
            }

            if (json["@odata.count"] != null)
            {
                result.Count = json["@odata.count"].Value<int>();
            }

            if (json["@Microsoft.Dynamics.CRM.fetchxmlpagingcookie"] != null)
            {
                result.PagingCookie = json["@Microsoft.Dynamics.CRM.fetchxmlpagingcookie"].Value<string>();
            }

            return result;
        }
    }
}
