using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(ICrmActionParameterHandle), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterMainHandle : ICrmActionParameterHandle
    {
        private static Dictionary<Type, IFactory<ICrmActionParameterHandle>> _handleFactories = new Dictionary<Type, IFactory<ICrmActionParameterHandle>>();

        public static Dictionary<Type, IFactory<ICrmActionParameterHandle>> HandleFactories
        {
            get
            {
                return _handleFactories;
            }
        }
        public async Task<CrmActionParameterHandleResult> Convert(string name, object parameter)
        {
            CrmActionParameterHandleResult result;
            if (parameter == null)
            {
                result = new CrmActionParameterHandleResult()
                {
                    Name = name,
                    Value = JToken.Parse("null")
                };

                return await Task.FromResult(result);
            }

            if (!_handleFactories.TryGetValue(parameter.GetType(), out IFactory<ICrmActionParameterHandle> handleFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCrmActionParameterHandleByType,
                    DefaultFormatting = "找不到类型为{0}的Crm动作参数处理，发生位置：{1}",
                    ReplaceParameters = new List<object>() { parameter.GetType().FullName, $"{ this.GetType().FullName }.Convert" }
                };

                throw new UtilityException((int)Errors.NotFoundCrmActionParameterHandleByType, fragment);
            }
            result= await handleFactory.Create().Convert(name,parameter);
            return result;
        }
    }
}
