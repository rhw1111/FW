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
using System.Text.RegularExpressions;

namespace FW.TestPlatform.Main.Template.LabelParameterHandlers
{
    /// <summary>
    ///针对全局数据变量声明的标签参数处理
    ///格式:{$intrange(min,max)}
    ///要求context中的Parameters中
    ///包含EngineType参数，参数类型为string
    [Injection(InterfaceType = typeof(LabelParameterHandlerForIntRange), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForIntRange : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;

        public LabelParameterHandlerForIntRange(ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector)
        {
            _getSeparatorServiceSelector = getSeparatorServiceSelector;
        }

        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            StringBuilder strCode = new StringBuilder();

            if (parameters.Length < 2)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "{$intrange(min,max)}", 2, parameters.Length }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment, 1, 0);
            }

            Regex regex = new Regex(@"^(\-|\+)?\d+$");

            if (!regex.IsMatch(parameters[0]))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterTypeError,
                    DefaultFormatting = "标签{0}要求的参数{1}应为{2}，参数类型错误",
                    ReplaceParameters = new List<object>() { "{$intrange(min,max)}", "min", "Int" }
                };

                throw new UtilityException((int)Errors.LabelParameterTypeError, fragment, 1, 0);
            }

            if (!regex.IsMatch(parameters[1]))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterTypeError,
                    DefaultFormatting = "标签{0}要求的参数{1}应为{2}，参数类型错误",
                    ReplaceParameters = new List<object>() { "{$intrange(min,max)}", "max", "Int" }
                };

                throw new UtilityException((int)Errors.LabelParameterTypeError, fragment, 1, 0);
            }

            int min = int.Parse(parameters[0]);
            int max = int.Parse(parameters[1]);

            if (min > max)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterMinMaxError,
                    DefaultFormatting = "标签{0}要求的参数最小值和最大值错误，而实际最小值为{1}，最大值为{2}",
                    ReplaceParameters = new List<object>() { "{$intrange(min,max)}", min, max }
                };

                throw new UtilityException((int)Errors.LabelParameterMinMaxError, fragment, 1, 0);
            }

            strCode.Append($"IntRange({parameters[0]}, {parameters[1]})");

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
