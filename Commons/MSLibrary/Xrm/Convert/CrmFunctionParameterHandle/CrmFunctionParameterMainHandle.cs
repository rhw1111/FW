using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Convert.CrmFunctionParameterHandle
{
    [Injection(InterfaceType = typeof(ICrmFunctionParameterHandle), Scope = InjectionScope.Singleton)]
    public class CrmFunctionParameterMainHandle : ICrmFunctionParameterHandle
    {
        private static Dictionary<Type, IFactory<ICrmFunctionParameterHandle>> _handleFactories = new Dictionary<Type, IFactory<ICrmFunctionParameterHandle>>();

        public static Dictionary<Type, IFactory<ICrmFunctionParameterHandle>> HandleFactories
        {
            get
            {
                return _handleFactories;
            }
        }
        public async Task<CrmFunctionParameterHandleResult> Convert(string name,object parameter)
        {
            CrmFunctionParameterHandleResult result;

            if (parameter == null)
            {
                result = new CrmFunctionParameterHandleResult();
                result.Name = name;
                result.Value = "null";
                return await Task.FromResult(result);
            }
            if (!_handleFactories.TryGetValue(parameter.GetType(), out IFactory<ICrmFunctionParameterHandle> handleFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCrmFunctionParameterHandleByType,
                    DefaultFormatting = "找不到类型为{0}的Crm函数参数处理，发生位置：{1}",
                    ReplaceParameters = new List<object>() { parameter.GetType().FullName, $"{ this.GetType().FullName }.Convert" }
                };

                throw new UtilityException((int)Errors.NotFoundCrmFunctionParameterHandleByType, fragment);
            }
            return await handleFactory.Create().Convert(name,parameter);
        }
    }
}
