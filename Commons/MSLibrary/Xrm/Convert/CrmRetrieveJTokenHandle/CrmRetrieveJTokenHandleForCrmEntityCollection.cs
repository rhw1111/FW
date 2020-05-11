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
    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntityCollection), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntityCollection: ICrmRetrieveJTokenHandle
    {
        private CrmRetrieveJTokenHandleForCrmEntityListFactory _crmRetrieveJTokenHandleForCrmEntityListFactory;
        public CrmRetrieveJTokenHandleForCrmEntityCollection(CrmRetrieveJTokenHandleForCrmEntityListFactory crmRetrieveJTokenHandleForCrmEntityListFactory)
        {
            _crmRetrieveJTokenHandleForCrmEntityListFactory = crmRetrieveJTokenHandleForCrmEntityListFactory;
        }

        public async Task<object> Execute(JToken json, Dictionary<string, object> extensionParameters = null)
        {
            if (extensionParameters == null || !extensionParameters.ContainsKey(CrmRetrieveJTokenHandleExtensionParameterNames.EntityName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRetrieveJTokenHandleMissParameter,
                    DefaultFormatting = "类型为{0}的Crm查询结果JToken处理缺少参数{1}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmRetrieveJTokenHandleExtensionParameterNames.EntityName }
                };

                throw new UtilityException((int)Errors.CrmRetrieveJTokenHandleMissParameter, fragment);
            }

            var objEntityName = extensionParameters[CrmRetrieveJTokenHandleExtensionParameterNames.EntityName];
            if (objEntityName == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRetrieveJTokenHandleParameterTypeNotMatch,
                    DefaultFormatting = "类型为{0}的Crm查询结果JToken处理的参数{1}的类型不匹配，期待的类型为{2}，实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmRetrieveJTokenHandleExtensionParameterNames.EntityName, typeof(string).FullName, "null" }
                };

                throw new UtilityException((int)Errors.CrmRetrieveJTokenHandleParameterTypeNotMatch,fragment);
            }

            if (!(objEntityName is string))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRetrieveJTokenHandleParameterTypeNotMatch,
                    DefaultFormatting = "类型为{0}的Crm查询结果JToken处理的参数{1}的类型不匹配，期待的类型为{2}，实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmRetrieveJTokenHandleExtensionParameterNames.EntityName, typeof(string).FullName, objEntityName.GetType().FullName }
                };

                throw new UtilityException((int)Errors.CrmRetrieveJTokenHandleParameterTypeNotMatch, fragment);
            }

            var entityName = (string)objEntityName;

            CrmEntityCollection result = new CrmEntityCollection();

            JToken valueJToken = json["value"];

            if (valueJToken == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmJTokenConvertEntityNotFoundAttribute,
                    DefaultFormatting = "在Crm的JToken转换服务{0}中，传入的JToken中找不到属性{1}，实体名称为{2}，JToken值为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, "Value", entityName, json.ToString() }
                };

                throw new UtilityException((int)Errors.CrmJTokenConvertEntityNotFoundAttribute, fragment);
            }

            var entityList= await _crmRetrieveJTokenHandleForCrmEntityListFactory.Create().Execute(valueJToken, new Dictionary<string, object>() { { CrmRetrieveJTokenHandleExtensionParameterNames.EntityName, entityName } });

            result.Results = (List<CrmEntity>)entityList;

            if (json["@odata.nextLink"]!=null)
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

            if (json["@Microsoft.Dynamics.CRM.fetchxmlpagingcookie"]!=null)
            {
                result.PagingCookie = json["@Microsoft.Dynamics.CRM.fetchxmlpagingcookie"].Value<string>();
            }

            return result;
        }
    }
}
