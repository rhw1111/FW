using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle
{
    [Injection(InterfaceType = typeof(ICrmExecuteEntityTypeHandle), Scope = InjectionScope.Singleton)]
    public class CrmExecuteEntityTypemMainHandle : ICrmExecuteEntityTypeHandle
    {
        private static Dictionary<Type, IFactory<ICrmExecuteEntityTypeHandle>> _serviceFactories = new Dictionary<Type, IFactory<ICrmExecuteEntityTypeHandle>>();

        public static Dictionary<Type, IFactory<ICrmExecuteEntityTypeHandle>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }
        public async Task<CrmExecuteEntityTypeHandleResult> Convert(string name, object value)
        {         
            if (value==null)
            {
                CrmExecuteEntityTypeHandleResult result = new CrmExecuteEntityTypeHandleResult();
                result.Name = name;
                result.Value = JToken.Parse("null");
                return result;
            }
            if (!_serviceFactories.TryGetValue(value.GetType(),out IFactory<ICrmExecuteEntityTypeHandle> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCrmExecuteEntityTypeHandleByTypeName,
                    DefaultFormatting = "找不到类型名称为{0}的Crm执行实体属性值转换处理，位置为{1}",
                    ReplaceParameters = new List<object>() { value.GetType().FullName, $"{ this.GetType().FullName }.Convert" }
                };

                throw new UtilityException((int)Errors.NotFoundCrmExecuteEntityTypeHandleByTypeName, fragment);
            }

            return await serviceFactory.Create().Convert(name, value);
        }
    }
}
