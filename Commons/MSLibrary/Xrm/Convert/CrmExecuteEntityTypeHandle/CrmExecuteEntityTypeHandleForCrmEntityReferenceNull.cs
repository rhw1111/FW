using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle
{
    [Injection(InterfaceType = typeof(CrmExecuteEntityTypeHandleForCrmEntityReferenceNull), Scope = InjectionScope.Singleton)]
    public class CrmExecuteEntityTypeHandleForCrmEntityReferenceNull : ICrmExecuteEntityTypeHandle
    {
        public async Task<CrmExecuteEntityTypeHandleResult> Convert(string name, object value)
        {
            if (!(value is CrmEntityReferenceNull))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmExecuteEntityTypeHandleTypeNotMatch,
                    DefaultFormatting = "Crm执行实体属性值转换处理类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmEntityReferenceNull).FullName, value.GetType().FullName, $"{ this.GetType().FullName }.Convert" }
                };

                throw new UtilityException((int)Errors.CrmExecuteEntityTypeHandleTypeNotMatch, fragment);
            }

            var realValue = value as CrmEntityReferenceNull;
            CrmExecuteEntityTypeHandleResult result = new CrmExecuteEntityTypeHandleResult();
            result.Name = $"{realValue.EntityName}@odata.bind";
            result.Value = null;

            return await Task.FromResult(result);
        }
    }
}
