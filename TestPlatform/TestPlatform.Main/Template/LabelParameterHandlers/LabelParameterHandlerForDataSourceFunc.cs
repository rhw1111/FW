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
    ///针对数据源函数的标签参数处理
    ///格式:{$datasourcefunc()}
    ///要求context中的Parameters中
    ///包含EngineType参数，参数类型为string
    ///包含DataSourceFuncs参数，参数类型为IDictionary<string,DataSourceFuncConfigurationItem>
    [Injection(InterfaceType = typeof(LabelParameterHandlerForDataSourceFunc), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForDataSourceFunc : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGenerateDataSourceFuncService>> _generateDataSourceFuncServiceSelector;
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;
       

        public LabelParameterHandlerForDataSourceFunc(ISelector<IFactory<IGenerateDataSourceFuncService>> generateDataSourceFuncServiceSelector, ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector)
        {
            _generateDataSourceFuncServiceSelector = generateDataSourceFuncServiceSelector;
            _getSeparatorServiceSelector = getSeparatorServiceSelector;
        }
        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
   
         if (!context.Parameters.TryGetValue(TemplateContextParameterNames.EngineType,out object? objEngineType))
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

            var dataSourceFuncs = (IDictionary<string,DataSourceFuncConfigurationItem>)objDataSourceFuncs;
            StringBuilder strCode = new StringBuilder();
            var separatorService=_getSeparatorServiceSelector.Choose(engineType).Create();
            var strFuncSeparator = separatorService.GetFuncSeparator();
            //为数据源函数配置项生成代码
            foreach (var item in dataSourceFuncs)
            {
                var funService=_generateDataSourceFuncServiceSelector.Choose($"{engineType}-{item.Value.FuncType}").Create();
                strCode.Append(await funService.Generate(item.Value.FuncUniqueName, item.Value.Data));
                strCode.Append(strFuncSeparator);
            }

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(true);
        }
    }

    /// <summary>
    /// 数据源函数配置项
    /// </summary>
    public class DataSourceFuncConfigurationItem
    {
        /// <summary>
        /// 函数名称
        /// 运行时将根据名称匹配函数
        /// </summary>
        public string FuncName { get; set; } = null!;
        /// <summary>
        /// 函数实际名称，保证唯一
        /// </summary>
        public string FuncUniqueName { get; set; } = null!;
        /// <summary>
        /// 函数类型
        /// 运行时将根据该类型构建表达式字符串
        /// </summary>
        public string FuncType { get; set; } = null!;
        /// <summary>
        /// 函数使用的数据
        /// 运行时将使用此数据固化到函数体中
        /// </summary>
        public string Data { get; set; } = null!;
    }


}
