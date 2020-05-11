using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Convert.CrmRetrieveJTokenHandle
{
    /// <summary>
    /// Crm查询结果JToken转换成CrmEntityReference列表
    /// </summary>
    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntityReferenceList), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntityReferenceList : ICrmRetrieveJTokenHandle
    {
        private CrmRetrieveJTokenHandleForCrmEntityReferenceFactory _crmRetrieveJTokenHandleForCrmEntityReferenceFactory;
        public CrmRetrieveJTokenHandleForCrmEntityReferenceList(CrmRetrieveJTokenHandleForCrmEntityReferenceFactory crmRetrieveJTokenHandleForCrmEntityReferenceFactory)
        {
            _crmRetrieveJTokenHandleForCrmEntityReferenceFactory = crmRetrieveJTokenHandleForCrmEntityReferenceFactory;
        }

        public async Task<object> Execute(JToken json, Dictionary<string, object> extensionParameters = null)
        {
            List<CrmEntityReference> result = new List<CrmEntityReference>();

            if (json.Type == JTokenType.Null)
            {
                return await Task.FromResult(result);
            }

            if (json.Type != JTokenType.Array)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmJTokenConvertNotMatch,
                    DefaultFormatting = "在Crm的JToken转换服务{0}中，传入的JToken类型不匹配，期待的类型为{1}，实际类型为{2}，JToken值为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, "Array", json.Type.ToString(), json.ToString() }
                };

                throw new UtilityException((int)Errors.CrmJTokenConvertNotMatch, fragment);
            }

            foreach (var itemJson in json)
            {
                var entityReference = await _crmRetrieveJTokenHandleForCrmEntityReferenceFactory.Create().Execute(itemJson);
                result.Add((CrmEntityReference)entityReference);
            }

            return result;
        }
    }
}
