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
    ///计算校验和
    ///格式:{$calcchecksuminvoke(msg)}
    ///参数：1，从"8=........"开始一直到"10="之前的部分，不包含10=
    ///返回值：校验和
    [Injection(InterfaceType = typeof(LabelParameterHandlerForCalcCheckSumInvoke), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForCalcCheckSumInvoke : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;

        public LabelParameterHandlerForCalcCheckSumInvoke(ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector)
        {
            _getSeparatorServiceSelector = getSeparatorServiceSelector;
        }

        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            StringBuilder strCode = new StringBuilder();

            if (parameters.Length < 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "{$calcchecksuminvoke(msg)}", 1, parameters.Length }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment, 1, 0);
            }

            strCode.Append($"CalcCheckSum({parameters[0]})");

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
