using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Context
{
    [Injection(InterfaceType = typeof(IInternationalizationHandleServiceFactorySelector), Scope = InjectionScope.Singleton)]
    public class InternationalizationHandleServiceFactorySelector : IInternationalizationHandleServiceFactorySelector
    {
        public static IDictionary<string, IFactory<IInternationalizationHandleService>> InternationalizationHandleServiceFactories { get; } = new Dictionary<string, IFactory<IInternationalizationHandleService>>();
  
        public IFactory<IInternationalizationHandleService> Choose(string name)
        {
            if (!InternationalizationHandleServiceFactories.TryGetValue(name, out IFactory<IInternationalizationHandleService> factory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFountInternationalizationHandleServiceFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的国际化处理服务工厂，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.InternationalizationHandleServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFountInternationalizationHandleServiceFactoryByName, fragment);
            }

            return factory;
        }

    }
}
