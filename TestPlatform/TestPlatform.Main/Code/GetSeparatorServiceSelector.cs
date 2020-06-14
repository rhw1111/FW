using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Code
{
    /// <summary>
    /// 获取分隔符服务的服务选择器
    /// GetSeparatorServiceFactories键值对中的键为
    /// EngineType
    /// </summary>
    [Injection(InterfaceType = typeof(ISelector<IFactory<IGetSeparatorService>>), Scope = InjectionScope.Singleton)]
    public class GetSeparatorServiceSelector : ISelector<IFactory<IGetSeparatorService>>
    {
        public static IDictionary<string, IFactory<IGetSeparatorService>> GetSeparatorServiceFactories { get; } = new Dictionary<string, IFactory<IGetSeparatorService>>();

        public IFactory<IGetSeparatorService> Choose(string name)
        {
            if (!GetSeparatorServiceFactories.TryGetValue(name, out IFactory<IGetSeparatorService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundGetSeparatorServiceByName,
                    DefaultFormatting = "找不到名称为{0}的获取分隔符服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.GetSeparatorServiceFactories" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundGetSeparatorServiceByName, fragment, 1, 0);
            }
            return serviceFactory;
        }
    }
}
