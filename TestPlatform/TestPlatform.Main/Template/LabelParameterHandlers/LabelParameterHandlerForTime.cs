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
using System.Text.RegularExpressions;

namespace FW.TestPlatform.Main.Template.LabelParameterHandlers
{
    /// <summary>
    ///针对全局数据变量声明的标签参数处理
    ///格式:{$time(formate)}
    ///要求context中的Parameters中
    ///包含EngineType参数，参数类型为string
    [Injection(InterfaceType = typeof(LabelParameterHandlerForTime), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForTime : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGenerateVarInvokeService>> _generateVarInvokeServiceSelector;
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;

        public LabelParameterHandlerForTime(ISelector<IFactory<IGenerateVarInvokeService>> generateVarInvokeServiceSelector, ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector)
        {
            _generateVarInvokeServiceSelector = generateVarInvokeServiceSelector;
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

            StringBuilder strCode = new StringBuilder();

            if (parameters.Length > 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "{$time(formate)}", 1, parameters.Length }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment, 1, 0);
            }

            var funService = _generateVarInvokeServiceSelector.Choose($"{engineType}-{LabelParameterTypes.Time}").Create();
            string strTemp = await funService.Generate($"{LabelParameterTypes.Time}", parameters);
            strCode.Append(strTemp);

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
