using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle
{
    [Injection(InterfaceType = typeof(CrmAlternateKeyTypeHandleForNumber), Scope = InjectionScope.Singleton)]
    public class CrmAlternateKeyTypeHandleForNumber : ICrmAlternateKeyTypeHandle
    {
        public async Task<string> Convert(object value)
        {
            if (!(value is int) && !(value is float) && !(value is decimal))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmAlternateKeyTypeHandleTypeNotMatch,
                    DefaultFormatting = "Crm唯一键属性值转换处理类型不匹配，期待的类型为{0}，实际类型为{1}，发生的位置：{2}",
                    ReplaceParameters = new List<object>() { $"{typeof(int).FullName},{typeof(float).FullName},{typeof(decimal).FullName}", value.GetType().FullName, $"{this.GetType().FullName}.Convert" }
                };

                throw new UtilityException((int)Errors.CrmAlternateKeyTypeHandleTypeNotMatch, fragment);
            }
            if (value == null)
            {
                return await Task.FromResult("null");
            }
            return await Task.FromResult(value.ToString());
        }
    }
}
