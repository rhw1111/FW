using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Context
{
    [Injection(InterfaceType = typeof(IHttpExtensionContextHandleServiceFactorySelector), Scope = InjectionScope.Singleton)]
    public class HttpExtensionContextHandleServiceFactorySelector : IHttpExtensionContextHandleServiceFactorySelector
    {
        public static IDictionary<string, IFactory<IHttpExtensionContextHandleService>> HttpExtensionContextHandleServiceFactories { get; } = new Dictionary<string, IFactory<IHttpExtensionContextHandleService>>();

        public IFactory<IHttpExtensionContextHandleService> Choose(string name)
        {
            if (!HttpExtensionContextHandleServiceFactories.TryGetValue(name, out IFactory<IHttpExtensionContextHandleService> factory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFountHttpExtensionContextHandleServiceFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Http请求扩展上下文处理服务工厂，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.HttpExtensionContextHandleServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFountHttpExtensionContextHandleServiceFactoryByName, fragment);
            }

            return factory;
        }

    }
}
