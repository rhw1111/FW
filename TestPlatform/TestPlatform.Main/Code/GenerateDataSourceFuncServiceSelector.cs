using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Code
{
    /// <summary>
    /// 生成基于数据源的函数服务的服务选择器
    /// GenerateDataSourceFuncServiceFactories键值对中的键为
    /// EngineType+FuncType
    /// </summary>
    [Injection(InterfaceType = typeof(ISelector<IFactory<IGenerateDataSourceFuncService>>), Scope = InjectionScope.Singleton)]
    public class GenerateDataSourceFuncServiceSelector : ISelector<IFactory<IGenerateDataSourceFuncService>>
    {
        public static IDictionary<string, IFactory<IGenerateDataSourceFuncService>> GenerateDataSourceFuncServiceFactories { get; } = new Dictionary<string, IFactory<IGenerateDataSourceFuncService>>();

        public IFactory<IGenerateDataSourceFuncService> Choose(string name)
        {
            if (!GenerateDataSourceFuncServiceFactories.TryGetValue(name,out IFactory<IGenerateDataSourceFuncService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundGenerateDataSourceFuncServiceByName,
                    DefaultFormatting = "找不到名称为{0}的数据源函数生成服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name,$"{this.GetType().FullName}.GenerateDataSourceFuncServiceFactories" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundGenerateDataSourceFuncServiceByName, fragment, 1, 0);
            }
            return serviceFactory;
        }

    }
}
