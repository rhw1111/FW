using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle
{
    [Injection(InterfaceType = typeof(CrmExecuteEntityTypeHandleForCrmEntityReference), Scope = InjectionScope.Singleton)]
    public class CrmExecuteEntityTypeHandleForCrmEntityReference : ICrmExecuteEntityTypeHandle
    {
        public async Task<CrmExecuteEntityTypeHandleResult> Convert(string name,object value)
        {
            if (!(value is CrmEntityReference))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmExecuteEntityTypeHandleTypeNotMatch,
                    DefaultFormatting = "Crm执行实体属性值转换处理类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmEntityReference).FullName, value.GetType().FullName, $"{ this.GetType().FullName }.Convert" }
                };

                throw new UtilityException((int)Errors.CrmExecuteEntityTypeHandleTypeNotMatch, fragment);
            }

            var realValue = value as CrmEntityReference;
            CrmExecuteEntityTypeHandleResult result = new CrmExecuteEntityTypeHandleResult();
            result.Name = $"{name}@odata.bind";
            result.Value=JToken.Parse($@"""/{realValue.EntityName.ToLower().ToPlural()}({realValue.Id.ToString()})""");

            return await Task.FromResult(result);
        }
    }
}
