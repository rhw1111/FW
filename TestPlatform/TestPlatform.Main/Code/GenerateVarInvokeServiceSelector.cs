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
    /// GenerateVarInvokeServiceFactories键值对中的键为
    /// EngineType+FunName
    /// </summary>
    [Injection(InterfaceType = typeof(ISelector<IFactory<IGenerateVarInvokeService>>), Scope = InjectionScope.Singleton)]
    public class GenerateVarInvokeServiceSelector : ISelector<IFactory<IGenerateVarInvokeService>>
    {
        public static IDictionary<string, IFactory<IGenerateVarInvokeService>> ServiceFactories { get; } = new Dictionary<string, IFactory<IGenerateVarInvokeService>>();

        public IFactory<IGenerateVarInvokeService> Choose(string name)
        {
            if (!ServiceFactories.TryGetValue(name, out IFactory<IGenerateVarInvokeService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundGenerateVarInvokeServiceByName,
                    DefaultFormatting = "找不到名称为{0}的变量调用生成服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundGenerateVarInvokeServiceByName, fragment, 1, 0);
            }
            return serviceFactory;
        }
    }
}
