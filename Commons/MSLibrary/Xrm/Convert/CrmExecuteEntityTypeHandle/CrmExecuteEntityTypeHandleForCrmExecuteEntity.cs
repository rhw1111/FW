using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle
{
    [Injection(InterfaceType = typeof(CrmExecuteEntityTypeHandleForCrmExecuteEntity), Scope = InjectionScope.Singleton)]
    public class CrmExecuteEntityTypeHandleForCrmExecuteEntity : ICrmExecuteEntityTypeHandle
    {
        private ICrmExecuteEntityTypeHandle _crmExecuteEntityTypeHandle;

        public CrmExecuteEntityTypeHandleForCrmExecuteEntity(ICrmExecuteEntityTypeHandle crmExecuteEntityTypeHandle)
        {
            _crmExecuteEntityTypeHandle = crmExecuteEntityTypeHandle;
        }

        public async Task<CrmExecuteEntityTypeHandleResult> Convert(string name, object value)
        {
            if (!(value is CrmExecuteEntity))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmExecuteEntityTypeHandleTypeNotMatch,
                    DefaultFormatting = "Crm执行实体属性值转换处理类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmExecuteEntity).FullName, value.GetType().FullName, $"{ this.GetType().FullName }.Convert" }
                };

                throw new UtilityException((int)Errors.CrmExecuteEntityTypeHandleTypeNotMatch, fragment);
            }

            var realValue = value as CrmExecuteEntity;

            JObject result = new JObject();
    
            foreach (var entityItem in realValue.Attributes)
            {
                if (entityItem.Value==null)
                {
                    result[entityItem.Key] = null;
                    continue;
                }

                switch(entityItem.Value)
                {
                    case int v:
                        result[entityItem.Key] = JToken.Parse(JsonConvert.SerializeObject(v));
                        break;
                    case float v:
                        result[entityItem.Key] = JToken.Parse(JsonConvert.SerializeObject(v));
                        break;
                    case DateTime v:
                        result[entityItem.Key] = JToken.Parse(JsonConvert.SerializeObject(v));
                        break;
                    case bool v:
                        result[entityItem.Key] = JToken.Parse(JsonConvert.SerializeObject(v));
                        break;
                    case string v:
                        result[entityItem.Key] = JToken.Parse(JsonConvert.SerializeObject(v));
                        break;
                    case decimal v:
                        result[entityItem.Key] = JToken.Parse(JsonConvert.SerializeObject(v));
                        break;
                    case double v:
                        result[entityItem.Key] = JToken.Parse(JsonConvert.SerializeObject(v));
                        break;
                    case long v:
                        result[entityItem.Key] = JToken.Parse(JsonConvert.SerializeObject(v));
                        break;
                    default:
                        var convertResult = await _crmExecuteEntityTypeHandle.Convert(entityItem.Key, entityItem.Value);
                        result[convertResult.Name] = convertResult.Value;
                        break;
                }
            }

            if (realValue.Id != Guid.Empty)
            {
                if (realValue.IsActivity)
                {
                    result["activityid"] = JToken.Parse(JsonConvert.SerializeObject(realValue.Id));
                }
                else
                {
                    result[$"{realValue.EntityName.ToLower()}id"] = JToken.Parse(JsonConvert.SerializeObject(realValue.Id));
                }
            }

            CrmExecuteEntityTypeHandleResult finalResult = new CrmExecuteEntityTypeHandleResult();
            finalResult.Name = name;
            finalResult.Value = JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(result));
            return finalResult;
        }
    }
}
