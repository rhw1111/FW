using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Template;
using MSLibrary.LanguageTranslate;
using FW.TestPlatform.Main.Code;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.Template.LabelParameterHandlers
{
    /// <summary>
    ///针对数据源函数调用的标签参数处理
    ///格式:{$datasourcefuncinvoke(funcname,...)}
    ///要求context中的Parameters中
    ///包含EngineType参数，参数类型为string
    ///包含DataSourceFuncs参数，参数类型为IList<DataSourceFuncConfigurationItem>
    ///funcname：数据源名称
    ///...:不定参数，不同的数据源函数有不同的参数
    [Injection(InterfaceType = typeof(LabelParameterHandlerForDataSourceInvoke), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForDataSourceInvoke : ILabelParameterHandler
    {
        /// <summary>
        /// 数据源函数调用脚本生成服务键值对
        /// 键为EngineType+FuncType
        /// </summary>
        public static IDictionary<string, IFactory<IDataSourceInvokeScriptGenerateService>> DataSourceInvokeScriptGenerateServiceFactories { get; } = new Dictionary<string, IFactory<IDataSourceInvokeScriptGenerateService>>();
        
        
        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            if (!context.Parameters.TryGetValue(TemplateContextParameterNames.EngineType, out object? objEngineType))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInTemplateContextByName,
                    DefaultFormatting = "在模板上下文中找不到名称为{0}的参数",
                    ReplaceParameters = new List<object>() { TemplateContextParameterNames.EngineType }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInTemplateContextByName, fragment, 1, 0);
            }

            var engineType = (string)objEngineType;

            if (!context.Parameters.TryGetValue(TemplateContextParameterNames.DataSourceFuncs, out object? objDataSourceFuncs))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInTemplateContextByName,
                    DefaultFormatting = "在模板上下文中找不到名称为{0}的参数",
                    ReplaceParameters = new List<object>() { TemplateContextParameterNames.DataSourceFuncs }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInTemplateContextByName, fragment, 1, 0);
            }

            if (parameters.Length==0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountRequireMin,
                    DefaultFormatting = "标签{0}要求的参数个数至少为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "datasourcefuncinvoke", "1", parameters.Length.ToString() }
                };

                throw new UtilityException((int)Errors.LabelParameterCountRequireMin, fragment,1,0);
            }

            var funcName = parameters[0];

            var dataSourceFuncs = (IDictionary<string, DataSourceFuncConfigurationItem>)objDataSourceFuncs;

            if (!dataSourceFuncs.TryGetValue(funcName,out DataSourceFuncConfigurationItem? funcConfigurationItem))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundFuncNameInDataSourceFuncsFormContext,
                    DefaultFormatting = "在上下文参数DataSourceFuncs中找不到函数名称为{0}的记录，发生位置为{1}",
                    ReplaceParameters = new List<object>() { funcName,$"{this.GetType().FullName}.Execute" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundFuncNameInDataSourceFuncsFormContext, fragment, 1, 0);
            }

            if (!DataSourceInvokeScriptGenerateServiceFactories.TryGetValue($"{engineType}-{funcConfigurationItem.FuncType}",out IFactory<IDataSourceInvokeScriptGenerateService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundDataSourceInvokeScriptGenerateServiceByKey,
                    DefaultFormatting = "找不到测试引擎类型为{0}、函数类型为{1}的数据源函数调用脚本生成服务，发生位置为{2}",
                    ReplaceParameters = new List<object>() { engineType,funcName, $"{this.GetType().FullName}.Execute" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundDataSourceInvokeScriptGenerateServiceByKey, fragment, 1, 0);
            }

            var service = serviceFactory.Create();
            var result=await service.Generate(funcConfigurationItem.FuncUniqueName, parameters.AsSpan().Slice(1).ToArray());
            return result;
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }

    /// <summary>
    /// 数据源函数调用脚本生成服务
    /// </summary>
    public interface IDataSourceInvokeScriptGenerateService
    {
        Task<string> Generate(string funcUniqueName, string[] parameters);
    }
}
