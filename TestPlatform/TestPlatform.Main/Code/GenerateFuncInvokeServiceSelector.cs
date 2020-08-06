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
    [Injection(InterfaceType = typeof(ISelector<IFactory<IGenerateFuncInvokeService>>), Scope = InjectionScope.Singleton)]
    public class GenerateFuncInvokeServiceSelector : ISelector<IFactory<IGenerateFuncInvokeService>>
    {
        public static IDictionary<string, IFactory<IGenerateFuncInvokeService>> ServiceFactories { get; } = new Dictionary<string, IFactory<IGenerateFuncInvokeService>>();

        public IFactory<IGenerateFuncInvokeService> Choose(string name)
        {
            if (!ServiceFactories.TryGetValue(name, out IFactory<IGenerateFuncInvokeService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundGenerateFuncInvokeServiceByName,
                    DefaultFormatting = "找不到名称为{0}的附件函数生成服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundGenerateFuncInvokeServiceByName, fragment, 1, 0);
            }
            return serviceFactory;
        }
    }
}
