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
    /// GetSpaceServiceFactories键值对中的键为
    /// EngineType
    /// </summary>
    [Injection(InterfaceType = typeof(ISelector<IFactory<IGetSpaceService>>), Scope = InjectionScope.Singleton)]
    public class GetSpaceServiceSelector : ISelector<IFactory<IGetSpaceService>>
    {
        public static IDictionary<string, IFactory<IGetSpaceService>> GetSpaceServiceFactories { get; } = new Dictionary<string, IFactory<IGetSpaceService>>();

        public IFactory<IGetSpaceService> Choose(string name)
        {
            if (!GetSpaceServiceFactories.TryGetValue(name, out IFactory<IGetSpaceService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundGetSpaceServiceByName,
                    DefaultFormatting = "找不到名称为{0}的获取空格服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.GetSpaceServiceFactories" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundGetSpaceServiceByName, fragment, 1, 0);
            }
            return serviceFactory;
        }
    }
}
