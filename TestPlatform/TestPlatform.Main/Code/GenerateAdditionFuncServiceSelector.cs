using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Code
{
    /// <summary>
    /// 附件函数生成服务的服务选择器
    /// GenerateDataSourceFuncServiceFactories键值对中的键为
    /// EngineType+FunName
    /// </summary>
    [Injection(InterfaceType = typeof(ISelector<IFactory<IGenerateAdditionFuncService>>), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceSelector : ISelector<IFactory<IGenerateAdditionFuncService>>
    {
        public static IDictionary<string, IFactory<IGenerateAdditionFuncService>> ServiceFactories { get; } = new Dictionary<string, IFactory<IGenerateAdditionFuncService>>();

        public IFactory<IGenerateAdditionFuncService> Choose(string name)
        {
            if (!ServiceFactories.TryGetValue(name, out IFactory<IGenerateAdditionFuncService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundGenerateAdditionFuncServiceByName,
                    DefaultFormatting = "找不到名称为{0}的附件函数生成服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundGenerateAdditionFuncServiceByName, fragment, 1, 0);
            }
            return serviceFactory;
        }
    }
}
