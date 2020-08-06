using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Code
{
    /// <summary>
    /// 生成数据变量声明代码块服务的服务选择器
    /// ServiceFactories键值对中的键为
    /// EngineType+VarType
    /// </summary>
    [Injection(InterfaceType = typeof(ISelector<IFactory<IGenerateVarSettingService>>), Scope = InjectionScope.Singleton)]
    public class GenerateVarSettingServiceSelector : ISelector<IFactory<IGenerateVarSettingService>>
    {
        public static IDictionary<string, IFactory<IGenerateVarSettingService>> ServiceFactories { get; } = new Dictionary<string, IFactory<IGenerateVarSettingService>>();

        public IFactory<IGenerateVarSettingService> Choose(string name)
        {
            if (!ServiceFactories.TryGetValue(name, out IFactory<IGenerateVarSettingService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundGenerateVarSettingServiceByName,
                    DefaultFormatting = "找不到名称为{1}的变量设置声明代码块生成服务,发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundGenerateVarSettingServiceByName, fragment, 1, 0);
            }
            return serviceFactory;
        }
    }
}
