﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Template;
using MSLibrary.LanguageTranslate;
using FW.TestPlatform.Main.Code;
using FW.TestPlatform.Main.Entities;
using FW.TestPlatform.Main.Entities.TestCaseHandleServices;

namespace FW.TestPlatform.Main.Template.LabelParameterHandlers
{
    /// <summary>
    ///针对全局数据变量声明的标签参数处理
    ///格式:{$additionfunc()}
    ///要求context中的Parameters中
    ///包含EngineType参数，参数类型为string
    ///包含DataSourceVars参数，参数类型为List<ConfigurationDataForDataSourceVar>
    [Injection(InterfaceType = typeof(LabelParameterHandlerForAdditionFunc), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForAdditionFunc : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGenerateDataVarDeclareService>> _generateDataVarDeclareServiceFactorySelector;
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;

        public LabelParameterHandlerForAdditionFunc(ISelector<IFactory<IGenerateDataVarDeclareService>> generateDataVarDeclareServiceFactorySelector, ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector)
        {
            _generateDataVarDeclareServiceFactorySelector = generateDataVarDeclareServiceFactorySelector;
            _getSeparatorServiceSelector = getSeparatorServiceSelector;
        }

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

            if (!context.Parameters.TryGetValue(TemplateContextParameterNames.AdditionFuncNames, out object? objDataSourceVars))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInTemplateContextByName,
                    DefaultFormatting = "在模板上下文中找不到名称为{0}的参数",
                    ReplaceParameters = new List<object>() { TemplateContextParameterNames.AdditionFuncNames }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInTemplateContextByName, fragment, 1, 0);
            }

            var dataSourceVars = (List<ConfigurationDataForDataSourceVar>)objDataSourceVars;


            StringBuilder strCode = new StringBuilder();
            var separatorService = _getSeparatorServiceSelector.Choose(engineType).Create();
            var strFuncSeparator = await separatorService.GetFuncSeparator();

            foreach (var item in dataSourceVars)
            {
                var funService = _generateDataVarDeclareServiceFactorySelector.Choose($"{engineType}-{item.Type}").Create();
                strCode.Append(await funService.Generate(item.Name, item.Data));
                strCode.Append(strFuncSeparator);
            }

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
