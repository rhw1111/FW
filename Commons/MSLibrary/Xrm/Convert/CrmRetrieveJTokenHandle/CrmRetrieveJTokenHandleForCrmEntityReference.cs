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
    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntityReference), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntityReference : ICrmRetrieveJTokenHandle
    {
        public async Task<object> Execute(JToken json, Dictionary<string, object> extensionParameters = null)
        {
            if (json.Type== JTokenType.Null)
            {
                return await Task.FromResult(default(CrmEntityReference));
            }

            if (json["@odata.id"] == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmJTokenConvertEntityReferenceNotFoundAttribute,
                    DefaultFormatting = "在Crm的JToken转换EntityReference服务{0}中，传入的JToken中找不到属性{1}，JToken值为{2}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, "@odata.id", json.ToString() }
                };

                throw new UtilityException((int)Errors.CrmJTokenConvertEntityReferenceNotFoundAttribute, fragment);
            }

            if (json["@odata.id"].Type== JTokenType.Null)
            {
                return await Task.FromResult(default(CrmEntityReference));
            }

            var strEntityReferenceId = json["@odata.id"].Value<string>();

            Regex reg = new Regex(@"/([A-Za-z0-9_]+)\((\{?[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}}?)\)");
            var matchs = reg.Match(strEntityReferenceId);

            CrmEntityReference result = null;
            if (matchs.Groups.Count>=3)
            {
                result = new CrmEntityReference(matchs.Groups[1].Value, Guid.Parse(matchs.Groups[2].Value));
            }

            return result;
        }
    }
}
