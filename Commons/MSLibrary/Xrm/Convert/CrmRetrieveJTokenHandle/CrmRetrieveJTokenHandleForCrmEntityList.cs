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
    /// <summary>
    /// Crm查询结果JToken转换成Crm实体列表
    /// </summary>
    [Injection(InterfaceType = typeof(CrmRetrieveJTokenHandleForCrmEntityList), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenHandleForCrmEntityList : ICrmRetrieveJTokenHandle
    {
        private CrmRetrieveJTokenHandleForCrmEntityFactory _crmRetrieveJTokenHandleForCrmEntityFactory;
        public CrmRetrieveJTokenHandleForCrmEntityList(CrmRetrieveJTokenHandleForCrmEntityFactory crmRetrieveJTokenHandleForCrmEntityFactory)
        {
            _crmRetrieveJTokenHandleForCrmEntityFactory = crmRetrieveJTokenHandleForCrmEntityFactory;
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

                throw new UtilityException((int)Errors.CrmRetrieveJTokenHandleParameterTypeNotMatch, fragment);
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

            List<CrmEntity> result = new List<CrmEntity>();

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

            

            foreach(var itemJson in json)
            {
                var entity=await _crmRetrieveJTokenHandleForCrmEntityFactory.Create().Execute(itemJson, new Dictionary<string, object>() { { CrmRetrieveJTokenHandleExtensionParameterNames.EntityName, entityName } });
                result.Add((CrmEntity)entity);
            }

            return result;
        }
    }
}
