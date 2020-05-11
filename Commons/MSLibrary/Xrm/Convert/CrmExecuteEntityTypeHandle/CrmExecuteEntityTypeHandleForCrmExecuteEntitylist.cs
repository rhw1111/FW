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
    [Injection(InterfaceType = typeof(CrmExecuteEntityTypeHandleForCrmExecuteEntityList), Scope = InjectionScope.Singleton)]
    public class CrmExecuteEntityTypeHandleForCrmExecuteEntityList : ICrmExecuteEntityTypeHandle
    {
        private ICrmExecuteEntityTypeHandle _crmExecuteEntityTypeHandle;

        public CrmExecuteEntityTypeHandleForCrmExecuteEntityList(ICrmExecuteEntityTypeHandle crmExecuteEntityTypeHandle)
        {
            _crmExecuteEntityTypeHandle = crmExecuteEntityTypeHandle;
        }
        public async Task<CrmExecuteEntityTypeHandleResult> Convert(string name, object value)
        {
            if (!(value is IList<CrmExecuteEntity>))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmExecuteEntityTypeHandleTypeNotMatch,
                    DefaultFormatting = "Crm执行实体属性值转换处理类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(IList<CrmExecuteEntity>).FullName, value.GetType().FullName, $"{ this.GetType().FullName }.Convert" }
                };

                throw new UtilityException((int)Errors.CrmExecuteEntityTypeHandleTypeNotMatch, fragment);
            }

            var realValue = value as IList<CrmExecuteEntity>;

            JObject result = new JObject();
            List<JToken> jTokenList = new List<JToken>();
            foreach(var entityItem in realValue)
            {
                if (entityItem == null)
                {
                    jTokenList.Add(JToken.Parse("null"));
                }
                else
                {
                    var entityItemResult = await _crmExecuteEntityTypeHandle.Convert(string.Empty, entityItem);
                    jTokenList.Add(entityItemResult.Value);
                }
            }


            CrmExecuteEntityTypeHandleResult finalResult = new CrmExecuteEntityTypeHandleResult();
            finalResult.Name = name;
            finalResult.Value = JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(jTokenList));
            return finalResult;
        }
    }
}
