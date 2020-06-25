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
    [Injection(InterfaceType = typeof(ISelector<IFactory<IGenerateDataVarDeclareService>>), Scope = InjectionScope.Singleton)]
    public class GenerateDataVarDeclareServiceSelector : ISelector<IFactory<IGenerateDataVarDeclareService>>
    {
        public static IDictionary<string, IFactory<IGenerateDataVarDeclareService>> ServiceFactories { get; } = new Dictionary<string, IFactory<IGenerateDataVarDeclareService>>();

        public IFactory<IGenerateDataVarDeclareService> Choose(string name)
        {
            if (!ServiceFactories.TryGetValue(name, out IFactory<IGenerateDataVarDeclareService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundGenerateDataVarDeclareServiceByName,
                    DefaultFormatting = "找不到名称为{1}的数据变量声明代码块生成服务,发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundGenerateDataVarDeclareServiceByName, fragment, 1, 0);
            }
            return serviceFactory;
        }
    }
}
