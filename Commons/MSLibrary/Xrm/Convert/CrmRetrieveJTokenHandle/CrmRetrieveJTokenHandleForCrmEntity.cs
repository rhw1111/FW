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
    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntity), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntity : ICrmRetrieveJTokenHandle
    {
        public async Task<object> Execute(JToken json, Dictionary<string, object> extensionParameters)
        {
            if (extensionParameters==null || !extensionParameters.ContainsKey(CrmRetrieveJTokenHandleExtensionParameterNames.EntityName))
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
            if (objEntityName==null)
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

            if (json.Type== JTokenType.Null)
            {
                return await Task.FromResult(default(CrmEntity));
            }

            if (json.Type != JTokenType.Object && json.Type != JTokenType.Property)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmJTokenConvertNotMatch,
                    DefaultFormatting = "在Crm的JToken转换服务{0}中，传入的JToken类型不匹配，期待的类型为{1}，实际类型为{2}，JToken值为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, "Object,Property", json.Type.ToString(), json.ToString() }
                };

                throw new UtilityException((int)Errors.CrmJTokenConvertNotMatch, fragment);
            }

            Guid entityId = Guid.Empty;
            bool isActivity = false;
            if (json["activityid"] != null)
            {
                entityId = Guid.Parse(json["activityid"].Value<string>());
                isActivity = true;
            }
            else if (json[$"{entityName}id"] != null)
            {
                entityId = Guid.Parse(json[$"{entityName}id"].Value<string>());
            }
            else
            {
                entityId = Guid.Empty;
                /*var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmJTokenConvertEntityNotFoundAttribute,
                    DefaultFormatting = "在Crm的JToken转换服务{0}中，传入的JToken中找不到属性{1}，实体名称为{2}，JToken值为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, "EntityId", entityName, json.ToString() }
                };

                throw new UtilityException((int)Errors.CrmJTokenConvertEntityNotFoundAttribute, fragment);
                */
            }
            string version = null;
            if (json["@odata.etag"] != null)
            {
                //throw new UtilityException((int)Errors.CrmJTokenConvertEntityNotFoundAttribute, string.Format(StringLanguageTranslate.Translate(TextCodes.CrmJTokenConvertEntityNotFoundAttribute, "在Crm的JToken转换服务{0}中，传入的JToken中找不到属性{1}，实体名称为{2}，JToken值为{3}"), this.GetType().FullName, "@odata.etag", entityName, json.ToString()));
                version = json["@odata.etag"].Value<string>();
            }

           


            CrmEntity entity = new CrmEntity(entityName, entityId);
            entity.Version = version;
            entity.IsActivity = isActivity;
            entity.Attributes = json.Value<JObject>();

            return await Task.FromResult(entity);
        }
    }
}
