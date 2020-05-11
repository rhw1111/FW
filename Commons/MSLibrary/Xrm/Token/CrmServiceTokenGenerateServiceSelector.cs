using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Token
{
    [Injection(InterfaceType = typeof(ICrmServiceTokenGenerateServiceSelector), Scope = InjectionScope.Singleton)]
    public class CrmServiceTokenGenerateServiceSelector : ICrmServiceTokenGenerateServiceSelector
    {
        private static Dictionary<string, IFactory<ICrmServiceTokenGenerateService>> _serviceFactories = new Dictionary<string, IFactory<ICrmServiceTokenGenerateService>>();

        public static Dictionary<string, IFactory<ICrmServiceTokenGenerateService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }
        public ICrmServiceTokenGenerateService Choose(string name)
        {
            if (!_serviceFactories.TryGetValue(name,out IFactory<ICrmServiceTokenGenerateService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCrmServiceTokenGenerateServiceByName,
                    DefaultFormatting = "找不到名称为{0}的Crm服务令牌生成服务，位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.Choose" }
                };

                throw new UtilityException((int)Errors.NotFoundCrmServiceTokenGenerateServiceByName, fragment);
            }

            return serviceFactory.Create();
        }
    }
}
