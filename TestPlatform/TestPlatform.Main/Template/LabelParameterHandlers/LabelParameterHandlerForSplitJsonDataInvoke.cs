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
    ///计算校验和
    ///格式:{$splitjsondatainvoke(data,piece)}
    ///参数：1，Json字符串，2，份数
    ///返回值：校验和
    [Injection(InterfaceType = typeof(LabelParameterHandlerForSplitJsonDataInvoke), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForSplitJsonDataInvoke : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;

        public LabelParameterHandlerForSplitJsonDataInvoke(ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector)
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
                    ReplaceParameters = new List<object>() { "{$splitjsondatainvoke(data,piece)}", 2, parameters.Length }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment, 1, 0);
            }

            //Regex regex = new Regex(@"^(\-|\+)?\d+$");

            //if (!regex.IsMatch(parameters[1]))
            //{
            //    var fragment = new TextFragment()
            //    {
            //        Code = TextCodes.LabelParameterTypeError,
            //        DefaultFormatting = "标签{0}要求的参数{1}应为{2}，参数类型错误",
            //        ReplaceParameters = new List<object>() { "{$splitjsondatainvoke(data,piece)}", "piece", "Int" }
            //    };

            //    throw new UtilityException((int)Errors.LabelParameterTypeError, fragment, 1, 0);
            //}

            strCode.Append($"SplitJsonData({parameters[0]}\\,{parameters[1]})");

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
