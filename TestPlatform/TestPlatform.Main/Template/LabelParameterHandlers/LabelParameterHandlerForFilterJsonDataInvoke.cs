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
using FW.TestPlatform.Main.Entities.TestCaseHandleServices;

namespace FW.TestPlatform.Main.Template.LabelParameterHandlers
{
    /// <summary>
    ///过滤数据
    ///格式:{$filterjsondatainvoke(data,name,value)}
    ///参数：1，Json字符串 2，属性名称 3，属性值
    ///返回值：过滤后数据
    [Injection(InterfaceType = typeof(LabelParameterHandlerForFilterJsonDataInvoke), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForFilterJsonDataInvoke : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGenerateFuncInvokeService>> _generateFuncInvokeServiceSelector;
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;

        public LabelParameterHandlerForFilterJsonDataInvoke(ISelector<IFactory<IGenerateFuncInvokeService>> generateFuncInvokeServiceSelector, ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector)
        {
            _generateFuncInvokeServiceSelector = generateFuncInvokeServiceSelector;
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

            if (parameters.Length != 3)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "{$filterjsondatainvoke(data,name,value)}", 3, parameters.Length }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment, 1, 0);
            }

            var funService = _generateFuncInvokeServiceSelector.Choose($"{engineType}").Create();
            string strTemp = await funService.Generate("FilterJsonData", parameters);
            strCode.Append(strTemp);

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
