using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle
{
    [Injection(InterfaceType = typeof(ICrmAlternateKeyTypeHandle), Scope = InjectionScope.Singleton)]
    public class CrmAlternateKeyTypeMainHandle : ICrmAlternateKeyTypeHandle
    {
        private static Dictionary<Type, IFactory<ICrmAlternateKeyTypeHandle>> _serviceFactories = new Dictionary<Type, IFactory<ICrmAlternateKeyTypeHandle>>();

        public static Dictionary<Type, IFactory<ICrmAlternateKeyTypeHandle>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }
        public async Task<string> Convert(object value)
        {
            if (value==null)
            {
                return await Task.FromResult("null");
            }
            if (!_serviceFactories.TryGetValue(value.GetType(), out IFactory<ICrmAlternateKeyTypeHandle> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCrmAlternateKeyTypeHandleByTypeName,
                    DefaultFormatting = "找不到类型名称为{0}的Crm唯一键属性值转换处理，位置为{1}",
                    ReplaceParameters = new List<object>() { value.GetType().FullName, $"{ this.GetType().FullName }.Convert" }
                };
                throw new UtilityException((int)Errors.NotFoundCrmAlternateKeyTypeHandleByTypeName,fragment);
            }

            return await serviceFactory.Create().Convert(value);
        }
    }
}
